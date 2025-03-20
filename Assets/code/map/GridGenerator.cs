using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int gridWidth = 21; 
    public int gridHeight = 13; 
    public float cellSize = 1f;

    public GameObject floorPrefab;    
    public GameObject wallPrefab;     
    public GameObject obstaclePrefab; //奖励品之类的

    [Range(0f, 1f)] public float wallChance = 0.1f;      
    [Range(0f, 1f)] public float obstacleChance = 0.2f;  
    
    public int seed = 42; // 先随机一个

    void Start()
    {
        Random.InitState(seed); // 设定种子，如果需要管卡固定地形就用
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = -10; x <= 10; x++)  
        {
            for (int y = -6; y <= 6; y++) 
            {
                Vector3 floorPosition = new Vector3(x * cellSize, y * cellSize, 0); 
                Instantiate(floorPrefab, floorPosition, Quaternion.identity, transform);

                // 玩家生成位置
                if (x == 8 && y == 4) continue;

                // 玩家附近不会生成墙的位置
                if (x == -10 || x == 10 || y == -6 || y == 6)
                {
                    Vector3 wallPosition = new Vector3(x * cellSize, y * cellSize, -1); 
                    Instantiate(wallPrefab, wallPosition, Quaternion.identity, transform);
                }
                else
                {
                    // 墙壁
                    if (Random.value < wallChance)
                    {
                        Vector3 wallPosition = new Vector3(x * cellSize, y * cellSize, -1);
                        Instantiate(wallPrefab, wallPosition, Quaternion.identity, transform);
                    }
                    // 障碍物
                    else if (Random.value < obstacleChance)
                    {
                        Vector3 obstaclePosition = new Vector3(x * cellSize, y * cellSize, -1); 
                        Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}