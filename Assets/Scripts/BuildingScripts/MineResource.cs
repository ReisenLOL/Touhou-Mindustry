using UnityEngine;
using UnityEngine.Tilemaps;

public class MineResource : MonoBehaviour
{
    [SerializeField] Transform[] resourceChecks;
    [SerializeField] GameObject resourceObject;
    [SerializeField] Transform[] conveyorChecks;
    public string minedResource = null;
    [SerializeField] LayerMask conveyorLayer;
    [SerializeField] ResourceList resourceList;
    private GameObject resourceFolder;
    private GameManager gameManager;
    private float _time;
    public float miningSpeed;
    private Grid buildingGrid;
    private Tilemap terrainTiles;
    private bool canMine;
    void Start()
    {
        buildingGrid = GameObject.Find("BuildingGrid").GetComponent<Grid>();
        terrainTiles = GameObject.Find("TerrainTilemap").GetComponent<Tilemap>();
        resourceFolder = GameObject.Find("ResourceFolder");
        gameManager = FindFirstObjectByType<GameManager>();
        for (int i = 0; i < resourceChecks.Length; i++)
        {
            string tileName = terrainTiles.GetTile(buildingGrid.WorldToCell(resourceChecks[i].transform.position)).name;
            for (int j = 0; j < resourceList.resourceType.Length; j++)
            {
                if (tileName == resourceList.GetResourceType(j))
                {
                    if (miningSpeed > 1)
                    {
                        miningSpeed -= 1;
                    }
                    minedResource = resourceList.GetResourceType(j);
                    canMine = true;
                }
            }
        }
        miningSpeed *= gameManager.core.miningSpeedModifier;
    }

    void Update()
    {
        if (canMine)
        {
            _time += Time.deltaTime;
            if (_time >= miningSpeed)
            {
                Collider2D detectedConveyor = DetectConveyors(Random.Range(0, conveyorChecks.Length));
                if (detectedConveyor.TryGetComponent(out Conveyor isConveyor) && detectedConveyor != null && detectedConveyor.gameObject.GetComponent<ObjectStats>().acceptingResources)
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
        return Physics2D.OverlapBox(this.conveyorChecks[index].position, transform.localScale, conveyorLayer);
    }
}
