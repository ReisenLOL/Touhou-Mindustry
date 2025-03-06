using UnityEngine;

public class MineResource : MonoBehaviour
{
    [SerializeField] Transform resourceCheck;
    [SerializeField] GameObject resourceObject;
    [SerializeField] Transform[] conveyorChecks;
    public string minedResource;
    [SerializeField] LayerMask resourceLayer;
    [SerializeField] LayerMask conveyorLayer;

    private GameObject resourceFolder;
    private GameManager gameManager;
    private float _time;
    public float miningSpeed;
    void Start()
    {
        resourceFolder = GameObject.Find("ResourceFolder");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Collider2D foundResource = FindResources();
        if (foundResource != null && foundResource.CompareTag("ResourceVein"))
        {
            minedResource = FindResources().gameObject.GetComponent<ResourceType>().resourceType;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= miningSpeed)
        {
            Collider2D detectedConveyor = DetectConveyors(Random.Range(0, conveyorChecks.Length));
            if (detectedConveyor != null)
            {
                if (detectedConveyor.gameObject.GetComponent<Conveyor>() && detectedConveyor.gameObject.GetComponent<ObjectStats>().acceptingResources == true)
                {
                    _time -= miningSpeed;
                    GameObject ProduceResource = Instantiate(resourceObject, detectedConveyor.transform.position, resourceObject.transform.rotation);
                    ProduceResource.GetComponent<MinedResourceType>().type = minedResource;
                    ProduceResource.transform.SetParent(resourceFolder.transform);
                }
            }
        }
    }
    private Collider2D FindResources()
    {
        return Physics2D.OverlapCircle(this.resourceCheck.position, 0.05f, resourceLayer);
    }
    private Collider2D DetectConveyors(int index)
    {
        return Physics2D.OverlapCircle(this.conveyorChecks[index].position, 0.05f, conveyorLayer);
    }
}
