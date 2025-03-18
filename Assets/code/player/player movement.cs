using UnityEngine;
using System.Collections;  // 引入协程所需的命名空间

public class PlayerController : MonoBehaviour
{
    public float cellSize = 1f;  // 移动距离
    public Vector2 startPosition = new Vector2(8, 4); // 初始位置
    public LayerMask wallLayer;

    private Vector2 targetPosition;
    private bool isMoving = false;
    private MeshRenderer meshRenderer;
    private Color originalColor;

    void Start()
    {
        targetPosition = startPosition;
        transform.position = new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1);

        // 获取MeshRenderer组件并保存初始材质颜色
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
        }
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W)) Move(Vector2.up);
            if (Input.GetKeyDown(KeyCode.S)) Move(Vector2.down);
            if (Input.GetKeyDown(KeyCode.A)) Move(Vector2.left);
            if (Input.GetKeyDown(KeyCode.D)) Move(Vector2.right);
        }

        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1),
            5f * Time.deltaTime);

        if (Vector3.Distance(transform.position, new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1)) < 0.01f)
        {
            isMoving = false;
        }
    }

    void Move(Vector2 direction)
    {
        Vector2 newPosition = targetPosition + direction;
        Collider2D hit = Physics2D.OverlapPoint(new Vector2(newPosition.x * cellSize, newPosition.y * cellSize), wallLayer);
        if (hit == null)
        {
            targetPosition = newPosition;
            isMoving = true;
        }
    }

    // 新增方法：调用协程禁用碰撞体并改变透明度
    public void DisableCollider()
    {
        StartCoroutine(DisableColliderCoroutine());
    }

    private IEnumerator DisableColliderCoroutine()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            // 禁用碰撞体
            col.enabled = false;
            // 修改材质颜色的alpha值，使角色变得半透明（注意：材质必须使用支持透明的Shader）
            if (meshRenderer != null)
            {
                Color c = meshRenderer.material.color;
                c.a = 0.5f;  // 设置半透明
                meshRenderer.material.color = c;
            }
            // 等待1.5秒
            yield return new WaitForSeconds(1.5f);
            // 恢复碰撞体和原始颜色
            col.enabled = true;
            if (meshRenderer != null)
            {
                meshRenderer.material.color = originalColor;
            }
        }
    }
}
