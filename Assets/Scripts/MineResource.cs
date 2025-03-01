using UnityEngine;

public class MineResource : MonoBehaviour
{
    [SerializeField] Transform resourceCheck;
    public string minedResource;
    private GameManager gameManager;
    private float _time;
    public float miningSpeed;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (FindResources().CompareTag("Resource"))
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
            if (minedResource == "Resource")
            {
                gameManager.resources++;
                _time -= miningSpeed;
            }
        }
    }
    private Collider2D FindResources()
    {
        return Physics2D.OverlapCircle(this.resourceCheck.position, 0.05f);
    }
}
