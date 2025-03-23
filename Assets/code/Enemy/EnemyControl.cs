using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float cellSize = 1f;  // 移动距离
    public Vector2 startPosition = new Vector2(5, 5); // 初始位置（由 EnemyManager 设置）
    public LayerMask wallLayer;  // 检测墙壁的图层
    public EnemyManager enemyManager; // 引用 EnemyManager

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
            // 定义上下左右四个方向
            Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            for (int i = 0; i < directions.Length; i++)
            {
                int randIndex = Random.Range(0, directions.Length);
                Vector2 direction = directions[randIndex];
                Vector2 newPosition = targetPosition + direction;
                // 检测新位置是否有墙体
                Collider2D hit = Physics2D.OverlapPoint(new Vector2(newPosition.x * cellSize, newPosition.y * cellSize), wallLayer);
                if (hit != null)
                    continue;

                // 向 EnemyManager 申请预定该位置
                if (enemyManager != null && enemyManager.ReserveNextPosition(newPosition))
                {
                    targetPosition = newPosition;
                    isMoving = true;
                    break;
                }
            }
        }

        // 平滑移动到目标位置
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1),
            2f * Time.deltaTime);

        // 到达目标位置后释放预定
        if (Vector3.Distance(transform.position, new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1)) < 0.01f)
        {
            isMoving = false;
            if (enemyManager != null)
                enemyManager.ReleaseReservedPosition(targetPosition);
        }
    }

    // 碰撞检测：当碰到 Player 时调用 Player 的 DisableCollider 方法
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.DisableCollider();
            }

            // 扣血
            if (PlayerHealthController.instance != null)
            {
                PlayerHealthController.instance.DealDamage();
            }
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            Destroy(gameObject);
        }
    }


}