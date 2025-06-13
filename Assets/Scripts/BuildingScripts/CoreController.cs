using System;
using UnityEngine;

public class CoreController : MonoBehaviour
{
    [SerializeField] LayerMask resourceLayer;
    [SerializeField] Transform resourceCheck;
    [SerializeField] ResourceManager resourceManager;
    public ObjectStats objectStats;

    void Start()
    {
        resourceManager = FindFirstObjectByType<ResourceManager>();
        objectStats = GetComponent<ObjectStats>();
    }

    private void OnDisable()
    {
        FindAnyObjectByType<GameManager>().GameOver();
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
