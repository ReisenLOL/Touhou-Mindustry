using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    [SerializeField] GameObject unitToSpawn;
    [SerializeField] string requiredResource;
    [SerializeField] int requiredAmount;
    [SerializeField] int capacity;
    [SerializeField] Transform SpawnLocation;
    private int storedResources;
    private ObjectStats buildingStats;
    private GameObject unitFolder;
    private float productionTimer;
    private bool isProducing = false;
    [SerializeField] float requiredManufacturingTime;
    [SerializeField] Transform progressBar;
    void Start()
    {
        unitFolder = GameObject.Find("UnitFolder");
        buildingStats = GetComponent<ObjectStats>();
    }
    void Update()
    {
        if (isProducing)
        {
            productionTimer += Time.deltaTime;
            progressBar.localScale = new Vector3(productionTimer/requiredManufacturingTime, progressBar.localScale.y, progressBar.localScale.z);
            if (productionTimer >= requiredManufacturingTime)
            {
                isProducing = false;
                productionTimer = 0;
                GameObject newUnit = Instantiate(unitToSpawn, SpawnLocation.position, unitToSpawn.transform.rotation);
                newUnit.transform.SetParent(unitFolder.transform);
            }
        }
        if (storedResources >= requiredAmount && !isProducing)
        {
            isProducing = true;
            storedResources -= requiredAmount;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out MinedResourceType r))
        {
            if (r.type == requiredResource && storedResources < capacity)
            {
                storedResources++;
                if (storedResources == capacity)
                {
                    buildingStats.acceptingResources = false;
                }
            }
        }
        Destroy(collision.gameObject);
    }
}
