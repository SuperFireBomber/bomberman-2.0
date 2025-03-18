using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;       // 敌人预制体
    public int enemyCount = 3;             // 需要生成的敌人数量
    public float cellSize = 1f;            // 格子大小

    // 地图范围：参考 GridGenerator 中 x: -10 ~ 10, y: -6 ~ 6
    public int minX = -10;
    public int maxX = 10;
    public int minY = -6;
    public int maxY = 6;

    // 玩家初始位置，排除此位置
    public Vector2 playerStartPos = new Vector2(8, 4);

    // 存储可用的初始格子（排除墙体、玩家初始位置等）
    private List<Vector2> availablePositions = new List<Vector2>();

    // 记录已预定的“下一步”移动位置，防止敌人冲突
    private HashSet<Vector2> reservedPositions = new HashSet<Vector2>();

    void Start()
    {
        // 生成所有可用的格子（此处简单起见，仅排除玩家位置；实际项目中可用 Physics2D 检测墙体）
        CollectAvailablePositions();

        // 根据可用格子随机生成多个敌人
        SpawnEnemies();
    }

    void CollectAvailablePositions()
    {
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector2 pos = new Vector2(x, y);
                // 排除玩家初始位置
                if (pos == playerStartPos)
                    continue;
                // 若需要排除墙体，可以用 Physics2D.OverlapPoint 检测，此处略
                availablePositions.Add(pos);
            }
        }
    }

    void SpawnEnemies()
    {
        // 复制一份可用位置列表，生成时将用过的位置移除，避免重叠
        List<Vector2> spawnPositions = new List<Vector2>(availablePositions);

        for (int i = 0; i < enemyCount; i++)
        {
            if (spawnPositions.Count == 0)
            {
                Debug.LogWarning("No more available positions to spawn enemies!");
                break;
            }
            int randIndex = Random.Range(0, spawnPositions.Count);
            Vector2 spawnPos = spawnPositions[randIndex];
            spawnPositions.RemoveAt(randIndex);

            // 转换为世界坐标，注意 z 设为 -1
            Vector3 worldPos = new Vector3(spawnPos.x * cellSize, spawnPos.y * cellSize, -1);
            GameObject enemyObj = Instantiate(enemyPrefab, worldPos, Quaternion.identity);

            // 将 EnemyManager 引用传递给 EnemyControl，同时设置初始位置与格子大小
            EnemyControl enemyControl = enemyObj.GetComponent<EnemyControl>();
            if (enemyControl != null)
            {
                enemyControl.startPosition = spawnPos;
                enemyControl.cellSize = cellSize;
                enemyControl.enemyManager = this;
            }
        }
    }

    // 给 Enemy 申请预定下一步移动位置
    public bool ReserveNextPosition(Vector2 pos)
    {
        if (reservedPositions.Contains(pos))
        {
            return false;
        }
        reservedPositions.Add(pos);
        return true;
    }

    // 当 Enemy 到达目标位置后释放预定
    public void ReleaseReservedPosition(Vector2 pos)
    {
        if (reservedPositions.Contains(pos))
        {
            reservedPositions.Remove(pos);
        }
    }
}