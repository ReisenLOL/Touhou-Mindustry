using UnityEngine;

public class CoreController : MonoBehaviour
{
    [SerializeField] LayerMask resourceLayer;
    [SerializeField] Transform resourceCheck;
    [SerializeField] ResourceManager resourceManager;

    void Start()
    {
        resourceManager = GameObject.Find("GameManager").GetComponent<ResourceManager>();
    }
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out MinedResourceType r))
        {
            resourceManager.AddResource(r.type, 1);
            Destroy(collision.gameObject);
        }
    }
}
