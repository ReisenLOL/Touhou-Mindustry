using UnityEngine;

public class MineResource : MonoBehaviour
{
    [SerializeField] Transform resourceCheck;
    [SerializeField] GameObject resourceObject;
    [SerializeField] Transform[] conveyorChecks;
    public string minedResource;

    private GameManager gameManager;
    private float _time;
    public float miningSpeed;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (FindResources().CompareTag("ResourceVein"))
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
                if (detectedConveyor.gameObject.GetComponent<Conveyor>())
                {
                    _time -= miningSpeed;
                    Instantiate(resourceObject, detectedConveyor.transform.position + new Vector3(0, 0, -1), resourceObject.transform.rotation);
                }
            }
        }
    }
    private Collider2D FindResources()
    {
        return Physics2D.OverlapCircle(this.resourceCheck.position, 0.05f);
    }
    private Collider2D DetectConveyors(int index)
    {
        return Physics2D.OverlapCircle(this.conveyorChecks[index].position, 0.05f);
    }
}
