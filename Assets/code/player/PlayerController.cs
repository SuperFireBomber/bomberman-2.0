using UnityEngine;
using System.Collections;  // 引入协程所需的命名空间

public class PlayerController : MonoBehaviour
{
    public Vector2 startPosition = new Vector2(8, 4); // 初始位置
    public LayerMask wallLayer;

    public GameObject bombPrefab;
    public int maxBombs = 1;

    private Vector2 targetPosition;
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;   // 使用 SpriteRenderer 替代 MeshRenderer
    private Color originalColor;
    public float moveSpeed = 5f;  // 控制移动速度，单位：单位/秒

    void Start()
    {
        targetPosition = startPosition;
        transform.position = new Vector3(targetPosition.x, targetPosition.y, -1);

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

        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            move += Vector3.up;
        if (Input.GetKey(KeyCode.S))
            move += Vector3.down;
        if (Input.GetKey(KeyCode.A))
            move += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            move += Vector3.right;

        // 按时间和速度累加移动
        transform.position += move.normalized * moveSpeed * Time.deltaTime;
        Debug.Log(transform.position);
        // 放置炸弹逻辑保持不变
        if (Input.GetKeyDown(KeyCode.Space) && Bomb.currentBombCount < maxBombs)
        {
            PlaceBomb();
            AudioManager.instance.PlaySFX("ignite-bomb");
        }
    }

    private void PlaceBomb()
    {
        Vector2 bombGridPos = new Vector2(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y)
        );

        Collider2D hit = Physics2D.OverlapPoint(new Vector2(bombGridPos.x, bombGridPos.y), wallLayer);
        if (hit == null)
        {
            Instantiate(bombPrefab, new Vector3(bombGridPos.x, bombGridPos.y, -1), Quaternion.identity);
        }
    }

    // 修改后的 DisableCollider 方法，使用 SpriteRenderer 来实现透明效果
    public void DisableCollider()
    {
        StartCoroutine(DisableColliderCoroutine());
    }

    private IEnumerator DisableColliderCoroutine()
    {
        // 记录原始层
        int originalLayer = gameObject.layer;
        // 设置为无敌层（确保在 Tags & Layers 中存在 “Invincible” 层）
        gameObject.layer = LayerMask.NameToLayer("Invincible");

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0.5f; // 半透明
            spriteRenderer.color = c;
        }

        yield return new WaitForSeconds(2f);

        // 恢复原始层和颜色
        gameObject.layer = originalLayer;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}
