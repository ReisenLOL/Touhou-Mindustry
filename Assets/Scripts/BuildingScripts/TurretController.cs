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
    [SerializeField] GameObject projectile;
    private ObjectStats buildingStats;
    [SerializeField] int range;
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
            if (iteration == null || enemyList[i].gameObject.GetComponent<UnitStats>().isEnemy == false)
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
        if (fireRateTime >= fireRate && ammoAmount > 0 && closestEnemy != gameObject)
        {
            ammoAmount--;
            fireRateTime -= fireRateTime;
            GameObject newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
            newProjectile.transform.parent = transform;
            Projectile projectileStats = newProjectile.GetComponent<Projectile>();
            projectileStats.firedFrom = gameObject;
            projectileStats.damage = damageDealt;
            projectileStats.RotateToTarget(closestEnemy.transform.position);
            projectileStats.isEnemyBullet = false;
            projectileStats.maxRange = range;
            buildingStats.acceptingResources = true;
        }
    }
    private Collider2D[] DetectEnemies()
    {
        return Physics2D.OverlapCircleAll(transform.position, range, UnitLayer);
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
