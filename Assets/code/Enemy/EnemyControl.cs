using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float cellSize = 1f;  // 移动距离
    public Vector2 startPosition = new Vector2(5, 5); // 初始位置
    public LayerMask wallLayer;  // 用于检测墙壁的图层

    private Vector2 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = startPosition;
        transform.position = new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1);
    }

    void Update()
    {
        if (!isMoving)
        {
            // 定义四个基本方向：上、下、左、右
            Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            for (int i = 0; i < directions.Length; i++)
            {
                int randIndex = Random.Range(0, directions.Length);
                Vector2 direction = directions[randIndex];
                Vector2 newPosition = targetPosition + direction;
                Collider2D hit = Physics2D.OverlapPoint(new Vector2(newPosition.x * cellSize, newPosition.y * cellSize), wallLayer);

                if (hit == null)
                {
                    targetPosition = newPosition;
                    isMoving = true;
                    break;
                }
            }
        }

        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1),
            2f * Time.deltaTime);

        if (Vector3.Distance(transform.position, new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1)) < 0.01f)
        {
            isMoving = false;
        }
    }

    // 新增碰撞检测方法
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 获取PlayerController组件并调用禁用碰撞体的方法
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.DisableCollider();
            }
        }
    }
}
