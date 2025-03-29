using UnityEngine;
using System.Collections;  // 引入协程所需的命名空间

public class PlayerController : MonoBehaviour
{
    public float cellSize = 0.25f;  // 移动距离
    public Vector2 startPosition = new Vector2(8, 4); // 初始位置
    public LayerMask wallLayer;

    public GameObject bombPrefab;
    public int maxBombs = 1;

    private Vector2 targetPosition;
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;   // 使用 SpriteRenderer 替代 MeshRenderer
    private Color originalColor;

    void Start()
    {
        targetPosition = startPosition;
        transform.position = new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1);

        // 获取 SpriteRenderer 组件并保存原始颜色
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        // Trigger DisableCollider(无敌) at the beginning
        DisableCollider();
    }

    void Update()
    {
        // 如果 Victory 面板已显示，则不响应玩家输入
        if (VictoryManager.instance != null && (VictoryManager.instance.clearPanel.activeSelf || !VictoryManager.instance.allowMove))
            return;

        // 如果玩家未在移动时，更新 targetPosition 与当前格子对齐，防止因物理系统干扰造成位置 desync
        if (!isMoving)
        {
            targetPosition = new Vector2(
                Mathf.Round(transform.position.x / cellSize),
                Mathf.Round(transform.position.y / cellSize)
            );

            // 只有当玩家未在移动时，才能接收新的移动输入（支持长按）
            if (Input.GetKey(KeyCode.W)) StartCoroutine(Move(Vector2.up));
            if (Input.GetKey(KeyCode.S)) StartCoroutine(Move(Vector2.down));
            if (Input.GetKey(KeyCode.A)) StartCoroutine(Move(Vector2.left));
            if (Input.GetKey(KeyCode.D)) StartCoroutine(Move(Vector2.right));
        }

        // 放置炸弹
        if (Input.GetKeyDown(KeyCode.Space) && Bomb.currentBombCount < maxBombs)
        {
            PlaceBomb();
            AudioManager.instance.PlaySFX("ignite-bomb");
        }
    }

    private IEnumerator Move(Vector2 direction)
    {
        Vector2 newPosition = targetPosition + direction;

        // 检查目标位置是否有墙
        Vector2 worldPos = new Vector2(newPosition.x * cellSize, newPosition.y * cellSize);
        if (Physics2D.OverlapPoint(worldPos, wallLayer) != null)
        {
            yield break; // 退出协程，不移动
        }

        // 开始移动
        isMoving = true;
        // 暂时禁用碰撞体，避免插值过程中因物理系统干扰导致位置偏移
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(newPosition.x * cellSize, newPosition.y * cellSize, -1);

        float elapsedTime = 0f;
        float moveTime = 0.1f; // 控制移动速度

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保移动结束后的位置准确，直接赋值网格位置（避免插值带来的浮点数误差）
        transform.position = endPos;
        targetPosition = newPosition;
        isMoving = false;

        // 移动结束后重新启用碰撞体
        if (col != null) col.enabled = true;
    }

    private void PlaceBomb()
    {
        Vector2 bombGridPos = new Vector2(
            Mathf.Round(transform.position.x / cellSize),
            Mathf.Round(transform.position.y / cellSize)
        );

        Collider2D hit = Physics2D.OverlapPoint(new Vector2(bombGridPos.x * cellSize, bombGridPos.y * cellSize), wallLayer);
        if (hit == null)
        {
            Instantiate(bombPrefab, new Vector3(bombGridPos.x * cellSize, bombGridPos.y * cellSize, -1), Quaternion.identity);
        }
    }

    // 修改后的 DisableCollider 方法，使用 SpriteRenderer 来实现透明效果
    public void DisableCollider()
    {
        StartCoroutine(DisableColliderCoroutine());
    }

    private IEnumerator DisableColliderCoroutine()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = 0.5f;  // 设置半透明
                spriteRenderer.color = c;
            }

            yield return new WaitForSeconds(2f);

            col.enabled = true;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }
}
