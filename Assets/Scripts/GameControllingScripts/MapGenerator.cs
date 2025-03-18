using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int width = 50; // Width of the map
    public int height = 50; // Height of the map
    public float terrainScale = 10f; // Scale of Perlin noise
    public float oreScale = 15f;  // Different scale for ore veins
    public float wallScale = 8f;  // Walls should appear in patches

    public Tilemap tileMap;
    public TileBase groundTile;
    public TileBase waterTile;
    public TileBase deepWaterTile;
    public TileBase[] oreTiles;
    public TileBase walltile;
    private bool[,] oreMap; // Stores where ore should be placed
    private bool[,] visited; // Keeps track of visited tiles
    private bool[,] walkableGrid;
    public PathfindingGrid pathfindingGrid;

    void Start()
    {
        pathfindingGrid = GetComponent<PathfindingGrid>();
        GenerateMap();
        pathfindingGrid.InitializeGrid(walkableGrid);
    }

    void GenerateMap()
    {
        oreMap = new bool[width, height];
        visited = new bool[width, height];
        walkableGrid = new bool[width, height];
        // land or water
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float terrainValue = Mathf.PerlinNoise(x / terrainScale, y / terrainScale);
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (terrainValue > 0.2f)
                {
                    tileMap.SetTile(tilePosition, groundTile);
                    walkableGrid[x, y] = true;
                }
                else if (terrainValue > 0.15f && terrainValue <= 0.25f)
                {
                    tileMap.SetTile(tilePosition, waterTile);
                    walkableGrid[x, y] = true;
                }
                else
                {
                    tileMap.SetTile(tilePosition, deepWaterTile);
                    walkableGrid[x, y] = false;
                }
            }
        }
        // ore or land
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (tileMap.GetTile(tilePosition) == groundTile)
                {
                    float oreValue = Mathf.PerlinNoise(x / oreScale, y / oreScale);
                    if (oreValue > 0.75f)
                    {
                        oreMap[x, y] = true;
                    }
                }
            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (oreMap[x, y] && !visited[x, y])
                {
                    TileBase oreType = oreTiles[Random.Range(0, oreTiles.Length)];
                    FloodFillOrePatch(x, y, oreType);
                }
            }
        }
        // wall or land
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (tileMap.GetTile(tilePosition) == groundTile)
                {
                    float wallValue = Mathf.PerlinNoise(x / wallScale, y / wallScale);
                    if (wallValue > 0.7f)
                    {
                        tileMap.SetTile(tilePosition, walltile);
                        walkableGrid[x, y] = false; 
                    }
                }
            }
        }
    }
    void FloodFillOrePatch(int startX, int startY, TileBase oreType)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;

        while (queue.Count > 0)
        {
            Vector2Int pos = queue.Dequeue();
            int x = pos.x;
            int y = pos.y;

            tileMap.SetTile(new Vector3Int(x, y, 0), oreType); 
            Vector2Int[] neighbors = { new Vector2Int(x + 1, y), new Vector2Int(x - 1, y), new Vector2Int(x, y + 1), new Vector2Int(x, y - 1) };

            foreach (Vector2Int neighbor in neighbors)
            {
                int nx = neighbor.x;
                int ny = neighbor.y;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height && !visited[nx, ny] && oreMap[nx, ny])
                {
                    visited[nx, ny] = true;
                    queue.Enqueue(neighbor);
                }
            }
        }
    }
    public bool[,] GetWalkableGrid()
    {
        return walkableGrid;
    }

}
