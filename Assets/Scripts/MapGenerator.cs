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

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        // land or water
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float terrainValue = Mathf.PerlinNoise(x / terrainScale, y / terrainScale);
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (terrainValue > 0.25f)
                {
                    tileMap.SetTile(tilePosition, groundTile);
                }
                else if (terrainValue > 0.15f && terrainValue <= 0.25f)
                {
                    tileMap.SetTile(tilePosition, waterTile);
                }
                else
                {
                    tileMap.SetTile(tilePosition, deepWaterTile);
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
                        int randomOre = Random.Range(0, oreTiles.Length);
                        // so i guess its time to figure out HOW THE FUCK DO I GENERATE ORE VEINS!!!
                        tileMap.SetTile(tilePosition, oreTiles[randomOre]);
                    }
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
                    if (wallValue > 0.55f)
                    {
                        tileMap.SetTile(tilePosition, walltile);
                    }
                }
            }
        }
    }
}
