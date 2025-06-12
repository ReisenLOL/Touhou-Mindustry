using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] string[] ammoResources;
    [SerializeField] int ammoAmount;
    [SerializeField] int ammoCapacity;
    [SerializeField] LayerMask UnitLayer;
    private Collider2D[] enemyList;
    [SerializeField] AmmoType[] ammoTypes;
    private AmmoType loadedAmmoType;
    private GameObject closestEnemy;
    private float fireRateTime;
    public float fireRate;
    public float damageDealt;
    [SerializeField] GameObject projectile;
    private ObjectStats buildingStats;
    [SerializeField] int range;
    private bool burstFire;
    private int bulletsLeftToShoot;
    [SerializeField] float burstFireRate;
    private float burstFireRateTime;
    void Start()
    {
        buildingStats = GetComponent<ObjectStats>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyList = DetectEnemies(); //convert this to an ontriggerenter
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
            for (int i = 0; i < loadedAmmoType.bulletCount; i++)
            {
                if (loadedAmmoType.bulletCount > 1)
                {
                    burstFire = true;
                    bulletsLeftToShoot = loadedAmmoType.bulletCount;
                }
                else
                {
                    ShootProjectile(); //wow it works except i suck at my own game
                }
            }
            buildingStats.acceptingResources = true;
        }
        if (burstFire)
        {
            burstFireRateTime += Time.deltaTime;
            if (burstFireRateTime >= burstFireRate)
            {
                bulletsLeftToShoot--;
                burstFireRateTime -= burstFireRate;
                ShootProjectile();
                if (bulletsLeftToShoot == 0)
                {
                    burstFire = false;
                }
            }
        }
    }
    private Collider2D[] DetectEnemies()
    {
        return Physics2D.OverlapCircleAll(transform.position, range, UnitLayer);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out MinedResourceType r) && ammoAmount < ammoCapacity)
        {
            for (int i = 0; i < ammoResources.Length; i++)
            {
                if (r.type == ammoResources[i])
                {
                    for (int j = 0; j < ammoTypes.Length; j++)
                    {
                        if (ammoResources[i] == ammoTypes[j].ammoResource)
                        {
                            loadedAmmoType = ammoTypes[j];
                            break;
                        }
                    }
                    ammoAmount++;
                    if (ammoAmount == ammoCapacity)
                    {
                        buildingStats.acceptingResources = false;
                    }
                    break;
                    //add scriptable object for bullet pattern, bullet effects
                    //ok thats enough for today i have an OC to design for an upcoming gamejam
                    //ok thats enough for today again i'll add the flak effect tmrw
                }
            }
        }
        if (collision.transform.TryGetComponent(out MinedResourceType isResource))
        {
            Destroy(collision.gameObject);
        }
    }
    private void ShootProjectile()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
        newProjectile.transform.parent = transform;
        Projectile projectileStats = newProjectile.GetComponent<Projectile>();
        projectileStats.firedFrom = gameObject;
        projectileStats.damage = loadedAmmoType.damage;
        projectileStats.RotateToTarget(closestEnemy.transform.position);
        projectileStats.isEnemyBullet = false;
        projectileStats.maxRange = range;
    }
}
