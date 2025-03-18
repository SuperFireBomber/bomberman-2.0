using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float cellSize = 1f;  // 移动距离
    public Vector2 startPosition = new Vector2(5, 5); // 你可以根据需求设置敌人的初始位置
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

            // 随机打乱顺序（可选）或直接随机选择尝试
            // 尝试所有方向，直到找到一个不撞墙的方向
            bool foundValidDirection = false;
            for (int i = 0; i < directions.Length; i++)
            {
                // 随机选择一个方向
                int randIndex = Random.Range(0, directions.Length);
                Vector2 direction = directions[randIndex];

                Vector2 newPosition = targetPosition + direction;

                // 检测新位置是否为墙壁
                Collider2D hit = Physics2D.OverlapPoint(new Vector2(newPosition.x * cellSize, newPosition.y * cellSize), wallLayer);

                if (hit == null)
                {
                    targetPosition = newPosition;
                    isMoving = true;
                    foundValidDirection = true;
                    break;
                }
            }
            // 如果四个方向都不行，则敌人保持原地不动
        }

        // 平滑移动到目标位置
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1),
            2f * Time.deltaTime);

        // 当接近目标位置时，停止移动
        if (Vector3.Distance(transform.position, new Vector3(targetPosition.x * cellSize, targetPosition.y * cellSize, -1)) < 0.01f)
        {
            isMoving = false;
        }
    }
}
