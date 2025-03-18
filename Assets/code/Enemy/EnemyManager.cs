using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;       // ����Ԥ����
    public int enemyCount = 3;             // ��Ҫ���ɵĵ�������
    public float cellSize = 1f;            // ���Ӵ�С

    // ��ͼ��Χ���ο� GridGenerator �� x: -10 ~ 10, y: -6 ~ 6
    public int minX = -10;
    public int maxX = 10;
    public int minY = -6;
    public int maxY = 6;

    // ��ҳ�ʼλ�ã��ų���λ��
    public Vector2 playerStartPos = new Vector2(8, 4);

    // �洢���õĳ�ʼ���ӣ��ų�ǽ�塢��ҳ�ʼλ�õȣ�
    private List<Vector2> availablePositions = new List<Vector2>();

    // ��¼��Ԥ���ġ���һ�����ƶ�λ�ã���ֹ���˳�ͻ
    private HashSet<Vector2> reservedPositions = new HashSet<Vector2>();

    void Start()
    {
        // �������п��õĸ��ӣ��˴�����������ų����λ�ã�ʵ����Ŀ�п��� Physics2D ���ǽ�壩
        CollectAvailablePositions();

        // ���ݿ��ø���������ɶ������
        SpawnEnemies();
    }

    void CollectAvailablePositions()
    {
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector2 pos = new Vector2(x, y);
                // �ų���ҳ�ʼλ��
                if (pos == playerStartPos)
                    continue;
                // ����Ҫ�ų�ǽ�壬������ Physics2D.OverlapPoint ��⣬�˴���
                availablePositions.Add(pos);
            }
        }
    }

    void SpawnEnemies()
    {
        // ����һ�ݿ���λ���б�����ʱ���ù���λ���Ƴ��������ص�
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

            // ת��Ϊ�������꣬ע�� z ��Ϊ -1
            Vector3 worldPos = new Vector3(spawnPos.x * cellSize, spawnPos.y * cellSize, -1);
            GameObject enemyObj = Instantiate(enemyPrefab, worldPos, Quaternion.identity);

            // �� EnemyManager ���ô��ݸ� EnemyControl��ͬʱ���ó�ʼλ������Ӵ�С
            EnemyControl enemyControl = enemyObj.GetComponent<EnemyControl>();
            if (enemyControl != null)
            {
                enemyControl.startPosition = spawnPos;
                enemyControl.cellSize = cellSize;
                enemyControl.enemyManager = this;
            }
        }
    }

    // �� Enemy ����Ԥ����һ���ƶ�λ��
    public bool ReserveNextPosition(Vector2 pos)
    {
        if (reservedPositions.Contains(pos))
        {
            return false;
        }
        reservedPositions.Add(pos);
        return true;
    }

    // �� Enemy ����Ŀ��λ�ú��ͷ�Ԥ��
    public void ReleaseReservedPosition(Vector2 pos)
    {
        if (reservedPositions.Contains(pos))
        {
            reservedPositions.Remove(pos);
        }
    }
}