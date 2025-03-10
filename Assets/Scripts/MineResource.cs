using UnityEngine;
using UnityEngine.Tilemaps;

public class MineResource : MonoBehaviour
{
    [SerializeField] Transform resourceCheck;
    [SerializeField] GameObject resourceObject;
    [SerializeField] Transform[] conveyorChecks;
    public string minedResource;
    [SerializeField] LayerMask conveyorLayer;
    [SerializeField] ResourceList resourceList;
    private GameObject resourceFolder;
    private GameManager gameManager;
    private float _time;
    public float miningSpeed;
    private Grid buildingGrid;
    private Tilemap terrainTiles;
    void Start()
    {
        buildingGrid = GameObject.Find("BuildingGrid").GetComponent<Grid>();
        terrainTiles = GameObject.Find("TerrainTilemap").GetComponent<Tilemap>();
        resourceFolder = GameObject.Find("ResourceFolder");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        string tileName = terrainTiles.GetTile(buildingGrid.WorldToCell(transform.position)).name;
        for (int i = 0; i < resourceList.resourceType.Length; i++)
        {
            if (tileName == resourceList.GetResourceType(i))
            {
                minedResource = resourceList.GetResourceType(i);
                break;
            }
            minedResource = null;
        }
    }

    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= miningSpeed)
        {
            Collider2D detectedConveyor = DetectConveyors(Random.Range(0, conveyorChecks.Length));
            if (detectedConveyor != null)
            {
                if (detectedConveyor.gameObject.GetComponent<Conveyor>() && detectedConveyor.gameObject.GetComponent<ObjectStats>().acceptingResources == true && minedResource != null)
                {
                    _time -= miningSpeed;
                    GameObject ProduceResource = Instantiate(resourceObject, detectedConveyor.transform.position, resourceObject.transform.rotation);
                    ProduceResource.GetComponent<MinedResourceType>().type = minedResource;
                    ProduceResource.transform.SetParent(resourceFolder.transform);
                }
            }
        }
    }
    private Collider2D DetectConveyors(int index)
    {
        return Physics2D.OverlapCircle(this.conveyorChecks[index].position, 0.05f, conveyorLayer);
    }
}
