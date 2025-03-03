using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] string ammoType;
    [SerializeField] int ammoAmount;
    [SerializeField] int ammoCapacity;
    [SerializeField] LayerMask UnitLayer;
    private Collider2D[] enemyList;
    private GameObject closestEnemy;
    private float fireRateTime;
    public float fireRate;
    public float damageDealt;
    [SerializeField] MoveProjectile projectile;
    private ObjectStats buildingStats;
    void Start()
    {
        buildingStats = GetComponent<ObjectStats>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyList = DetectEnemies();
        if (enemyList.Length == 0)
        {
            closestEnemy = this.gameObject;
        }
        float distanceToClosestEnemy = 1000000f;
        Collider2D iteration = null;
        for (int i = 0; i < enemyList.Length; i++)
        {
            iteration = enemyList[i];
            if (iteration == null)
            {
                continue;
            }
            float sqrDistance = Vector3.SqrMagnitude(transform.position - iteration.transform.position);
            if (sqrDistance < distanceToClosestEnemy && iteration.gameObject != gameObject)
            {
                closestEnemy = iteration.gameObject;
                distanceToClosestEnemy = sqrDistance;
            }
        }
        fireRateTime += Time.deltaTime;
        if (fireRateTime >= fireRate && ammoAmount > 0 && enemyList.Length > 0)
        {
            ammoAmount--;
            fireRateTime -= fireRateTime;
            MoveProjectile newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
            newProjectile.firedFrom = gameObject;
            newProjectile.damage = damageDealt;
            newProjectile.RotateToTarget(closestEnemy.transform.position);
            buildingStats.acceptingResources = true;
        }
    }
    private Collider2D[] DetectEnemies()
    {
        return Physics2D.OverlapCircleAll(transform.position, 50f, UnitLayer);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out MinedResourceType r))
        {
            if (r.type == ammoType && ammoAmount < ammoCapacity)
            {
                ammoAmount++;
                if (ammoAmount == ammoCapacity)
                {
                    buildingStats.acceptingResources = false;
                }
            }
        }
        Destroy(collision.gameObject);
    }
}
