using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GameObject obstaclePrefab;

    GridManager gridManager;

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
    }
    
    void Start()
    {
        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                if (obstacleData.obstacle[index])//if obstacle present instantiate it and disallow movement to that tile
                {
                    Vector3 position = new Vector3(x*10, 0, y*10);
                    gridManager.Grid[gridManager.GetCooordinatesFromPosition(position)].isWalkable=false;
                    Instantiate(obstaclePrefab, position, Quaternion.identity);  
                }
            }
        }
    }
}

