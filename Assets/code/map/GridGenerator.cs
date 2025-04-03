
using UnityEngine;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour
{
    public int gridWidth = 21; 
    public int gridHeight = 13;
    public float cellSize = 1f; 

    public GameObject floorPrefab; 
    public GameObject wallPrefab; 
    public GameObject obstaclePrefab;
    public List<GameObject> rewardPrefabs; 

    [Range(0f, 1f)] public float wallChance = 0.1f; 
    [Range(0f, 1f)] public float obstacleChance = 0.2f; 

    public int seed = 42; // Seed for random generation 
    public float rewardInterval = 20f;

    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>(); 
    private List<Vector3> availablePositions = new List<Vector3>(); 

    void Start()
    {
        Random.InitState(seed);
        GenerateGrid(); 
        InvokeRepeating("SpawnReward", rewardInterval, rewardInterval);
    }

    void GenerateGrid()
    {
        for (int x = -gridWidth; x <= gridWidth; x++) 
        {
            for (int y = -gridHeight; y <= gridHeight; y++) 
            {
                Vector3 floorPosition = new Vector3(x * cellSize, y * cellSize, 0); 
                Instantiate(floorPrefab, floorPosition, Quaternion.identity, transform);

                if (x == 8 && y == 4) continue; 

                if (x == -gridWidth || x == gridWidth || y == -gridHeight || y == gridHeight) 
                {
                    Vector3 wallPosition = new Vector3(x * cellSize, y * cellSize, -1);
                    Instantiate(wallPrefab, wallPosition, Quaternion.identity, transform);
                    occupiedPositions.Add(wallPosition); 
                }
                else
                {
                    if (Random.value < wallChance)// Randomly place a wall based on chance  
                    {
                        Vector3 wallPosition = new Vector3(x * cellSize, y * cellSize, -1);
                        Instantiate(wallPrefab, wallPosition, Quaternion.identity, transform);
                        occupiedPositions.Add(wallPosition);
                    }
                    else if (Random.value < obstacleChance)  // Randomly place an obstacle based on chance
                    {
                        Vector3 obstaclePosition = new Vector3(x * cellSize, y * cellSize, -1);
                        Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity, transform);
                        occupiedPositions.Add(obstaclePosition);
                    }
                    else // Position remains empty and available for reward spawning
                    {
                        Vector3 emptyPosition = new Vector3(x * cellSize, y * cellSize, -1);
                        availablePositions.Add(emptyPosition);
                    }
                }
            }
        }
    }

    void SpawnReward()
    {
        if (availablePositions.Count == 0 || rewardPrefabs.Count == 0) return; // No available positions

        int positionIndex = Random.Range(0, availablePositions.Count); 
        Vector3 spawnPosition = availablePositions[positionIndex];

        int rewardIndex = Random.Range(0, rewardPrefabs.Count);// Randomly select a reward from the list
        GameObject selectedReward = rewardPrefabs[rewardIndex];

        if (!occupiedPositions.Contains(spawnPosition)) 
        {
            Instantiate(selectedReward, spawnPosition, Quaternion.identity, transform); 
            occupiedPositions.Add(spawnPosition); 
            availablePositions.RemoveAt(positionIndex); // Remove the position from the available list
        }
    }
}
