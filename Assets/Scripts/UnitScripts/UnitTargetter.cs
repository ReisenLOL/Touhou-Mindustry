using UnityEngine;

public class UnitTargetter : MonoBehaviour
{
    private GameObject core;
    private GameObject player;
    private Rigidbody2D rb;
    private Vector3 lookDirection;
    private Collider2D[] targetList;
    private GameObject closestTarget;
    private UnitController unitController;
    private UnitStats unitStats;
    public float range;
    [SerializeField] LayerMask unitLayer;
    [SerializeField] LayerMask playerLayer;
    private bool isEnemy;
    void Start()
    {
        core = GameObject.Find("Core");
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        unitController = GetComponent<UnitController>();
        unitStats = GetComponent<UnitStats>();
        isEnemy = unitStats.isEnemy;
        closestTarget = this.gameObject;
    }

    void Update()
    {
        if (isEnemy)
        {
            float distanceToPlayer = Vector3.SqrMagnitude(player.transform.position - transform.position);
            float distanceToCore = Vector3.SqrMagnitude(core.transform.position - transform.position);
            if (distanceToPlayer < distanceToCore)
            {
                lookDirection = (player.transform.position - transform.position).normalized;
                unitController.lookDirection = lookDirection;
            }
            else
            {
                lookDirection = (core.transform.position - transform.position).normalized;
                unitController.lookDirection = lookDirection;
            }
        }
        else
        {
            lookDirection = (closestTarget.transform.position - transform.position).normalized;
            unitController.lookDirection = lookDirection;
        }

        targetList = DetectTargets();
        if (targetList.Length == 0 || closestTarget == null)
        {
            closestTarget = this.gameObject;
            unitController.closestTarget = closestTarget;
        }
        float distanceToClosestTarget = 1000000f;
        Collider2D iteration = null;
        for (int i = 0; i < targetList.Length; i++)
        {
            iteration = targetList[i];
            if (iteration == null || iteration.gameObject.TryGetComponent(out UnitStats stats) && isEnemy == stats.isEnemy)
            {
                continue;
            }
            float sqrDistance = Vector3.SqrMagnitude(transform.position - iteration.transform.position);
            if (sqrDistance < distanceToClosestTarget && iteration.gameObject != gameObject)
            {
                closestTarget = iteration.gameObject;
                unitController.closestTarget = closestTarget;
                distanceToClosestTarget = sqrDistance;
            }
        }
    }
    private Collider2D[] DetectTargets()
    {
        if (isEnemy)
        {
            return Physics2D.OverlapCircleAll(transform.position, range, ~unitLayer);
        }
        return Physics2D.OverlapCircleAll(transform.position, range, playerLayer);
    }
}
