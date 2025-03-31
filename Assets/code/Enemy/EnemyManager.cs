using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Prefab and Numbers")]
    public GameObject enemy1Prefab;   // Enemy1：普通敌人（health = 1，speedMultiplier = 1）
    public int enemy1Count = 2;
    public GameObject enemy2Prefab;   // Enemy2：血量 2（需要两次炸弹伤害消失），speedMultiplier = 1
    public int enemy2Count = 0;
    public GameObject enemy3Prefab;   // Enemy3：血量 1，但速度为普通的 2 倍
    public int enemy3Count = 0;
    public GameObject enemy4Prefab;   // Enemy4：血量 3（需要 3 次炸弹伤害消失），速度为普通的 3 倍
    public int enemy4Count = 0;

    [Header("Map Generation")]
    public float cellSize = 1f;       // 格子大小
    // 地图范围，与 GridGenerator 保持一致
    public int minX = -10;
    public int maxX = 10;
    public int minY = -6;
    public int maxY = 6;
    public LayerMask wallLayer;
    // 玩家初始位置，生成敌人时排除此位置
    public Vector2 playerStartPos = new Vector2(8, 4);
    public static int currentEnemyCount = 0;
    // 内部记录可用初始位置
    private List<Vector2> availablePositions = new List<Vector2>();
    // 用于预定移动目标，防止多个敌人选择同一位置
    private HashSet<Vector2> reservedPositions = new HashSet<Vector2>();
    // New: Flag to indicate if the scene is reloading
    public static bool isSceneReloading = false;    

    void Start()
    {
        isSceneReloading = false;
        StartCoroutine(DelayedCollect());
    }
    private IEnumerator DelayedCollect()
    {
        yield return new WaitForSeconds(0.01f);
        CollectAvailablePositions();
        SpawnAllEnemies();
    }

    void CollectAvailablePositions()
    {
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector2 pos = new Vector2(x, y);
                if (pos == playerStartPos)
                    continue;
                // 检查该格子对应的世界坐标是否有墙体（或障碍），若有则跳过
                Vector2 worldPos = new Vector2(pos.x * cellSize, pos.y * cellSize);
                if (Physics2D.OverlapPoint(worldPos, wallLayer) != null)
                    continue;
                availablePositions.Add(pos);
            }
        }
    }


    void SpawnAllEnemies()
    {
        SpawnEnemiesByType(enemy1Prefab, enemy1Count, 1, 1f);  // Enemy1: health=1, speedMultiplier=1
        SpawnEnemiesByType(enemy2Prefab, enemy2Count, 2, 1f);  // Enemy2: health=2, speedMultiplier=1
        SpawnEnemiesByType(enemy3Prefab, enemy3Count, 1, 2f);  // Enemy3: health=1, speedMultiplier=2
        SpawnEnemiesByType(enemy4Prefab, enemy4Count, 3, 3f);  // Enemy4: health=3, speedMultiplier=3
    }

    // 生成某一类型的敌人，并设置其血量和速度乘数
    void SpawnEnemiesByType(GameObject prefab, int count, int health, float speedMultiplier)
    {
        // 复制一份可用位置列表，防止同一类型内部重叠
        List<Vector2> spawnPositions = new List<Vector2>(availablePositions);
        for (int i = 0; i < count; i++)
        {
            if (spawnPositions.Count == 0)
            {
                Debug.LogWarning("No available spawn positions for " + prefab.name);
                break;
            }
            int randIndex = Random.Range(0, spawnPositions.Count);
            Vector2 spawnPos = spawnPositions[randIndex];
            spawnPositions.RemoveAt(randIndex);

            Vector3 worldPos = new Vector3(spawnPos.x * cellSize, spawnPos.y * cellSize, -1);
            GameObject enemyObj = Instantiate(prefab, worldPos, Quaternion.identity);

            EnemyControl ec = enemyObj.GetComponent<EnemyControl>();
            EnemyManager.currentEnemyCount++;  // 增加计数
            if (ec != null)
            {
                ec.startPosition = spawnPos;
                ec.cellSize = cellSize;
                ec.enemyManager = this;
                ec.maxHealth = health;
                ec.speedMultiplier = speedMultiplier;
                // 你可以在 prefab 或代码中统一设置 baseSpeed，例如 2f
                ec.baseSpeed = 2f;
            }
        }
    }

    // 给 Enemy 申请预定下一步移动位置
    public bool ReserveNextPosition(Vector2 pos)
    {
        if (reservedPositions.Contains(pos))
            return false;
        reservedPositions.Add(pos);
        return true;
    }

    // 释放预定位置
    public void ReleaseReservedPosition(Vector2 pos)
    {
        if (reservedPositions.Contains(pos))
            reservedPositions.Remove(pos);
    }
}
