using UnityEngine;

public class CoreController : MonoBehaviour
{
    [SerializeField] LayerMask resourceLayer;
    [SerializeField] Transform resourceCheck;
    [SerializeField] GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out MinedResourceType r))
        {
            gameManager.AddResource(r.type, 1);
            Destroy(collision.gameObject);
        }
    }
}
