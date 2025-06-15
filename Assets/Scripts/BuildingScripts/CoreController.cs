using System;
using UnityEngine;

public class CoreController : MonoBehaviour
{
    [SerializeField] LayerMask resourceLayer;
    [SerializeField] Transform resourceCheck;
    [SerializeField] ResourceManager resourceManager;
    public ObjectStats objectStats;
    
    //modifiers
    public float speedModifier = 1f;
    public float damageModifier = 1f;
    public float fireRateModifier = 1f;
    public float rangeModifier = 1f;
    public float buildingHealthModifier = 1f;
    public float unitHealthModifier = 1f;
    public float miningSpeedModifier = 1f;

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
