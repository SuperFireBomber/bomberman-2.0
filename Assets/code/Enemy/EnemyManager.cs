using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Prefab and Numbers")]
    public GameObject enemy1Prefab;   // Enemy1����ͨ���ˣ�health = 1��speedMultiplier = 1��
    public int enemy1Count = 2;
    public GameObject enemy2Prefab;   // Enemy2��Ѫ�� 2����Ҫ����ը���˺���ʧ����speedMultiplier = 1
    public int enemy2Count = 0;
    public GameObject enemy3Prefab;   // Enemy3��Ѫ�� 1�����ٶ�Ϊ��ͨ�� 2 ��
    public int enemy3Count = 0;
    public GameObject enemy4Prefab;   // Enemy4��Ѫ�� 3����Ҫ 3 ��ը���˺���ʧ�����ٶ�Ϊ��ͨ�� 3 ��
    public int enemy4Count = 0;

    [Header("Map Generation")]
    public float cellSize = 1f;       // ���Ӵ�С
    // ��ͼ��Χ���� GridGenerator ����һ��
    public int minX = -10;
    public int maxX = 10;
    public int minY = -6;
    public int maxY = 6;
    // ��ҳ�ʼλ�ã����ɵ���ʱ�ų���λ��
    public Vector2 playerStartPos = new Vector2(8, 4);

    // �ڲ���¼���ó�ʼλ�ã��򵥴��������ǽ�壩
    private List<Vector2> availablePositions = new List<Vector2>();
    // ����Ԥ���ƶ�Ŀ�꣬��ֹ�������ѡ��ͬһλ��
    private HashSet<Vector2> reservedPositions = new HashSet<Vector2>();

    void Start()
    {
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

    // ����ĳһ���͵ĵ��ˣ���������Ѫ�����ٶȳ���
    void SpawnEnemiesByType(GameObject prefab, int count, int health, float speedMultiplier)
    {
        // ����һ�ݿ���λ���б���ֹͬһ�����ڲ��ص�
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
            if (ec != null)
            {
                ec.startPosition = spawnPos;
                ec.cellSize = cellSize;
                ec.enemyManager = this;
                ec.maxHealth = health;
                ec.speedMultiplier = speedMultiplier;
                // ������� prefab �������ͳһ���� baseSpeed������ 2f
                ec.baseSpeed = 2f;
            }
        }
    }

    // �� Enemy ����Ԥ����һ���ƶ�λ��
    public bool ReserveNextPosition(Vector2 pos)
    {
        if (reservedPositions.Contains(pos))
            return false;
        reservedPositions.Add(pos);
        return true;
    }

    // �ͷ�Ԥ��λ��
    public void ReleaseReservedPosition(Vector2 pos)
    {
        if (reservedPositions.Contains(pos))
            reservedPositions.Remove(pos);
    }
}
