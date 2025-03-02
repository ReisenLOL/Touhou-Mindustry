using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] string ammoType;
    [SerializeField] int ammoAmount;
    [SerializeField] LayerMask UnitLayer;
    private Collider2D[] enemyList;
    private GameObject closestEnemy;
    private float fireRateTime;
    private float fireRate;
    [SerializeField] MoveProjectile projectile;
    void Start()
    {
        closestEnemy = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        enemyList = DetectEnemies();
        if (enemyList.Length == 0)
        {
            closestEnemy = this.gameObject;
        }
        float distanceToClosestEnemy = Vector3.Distance(transform.position, closestEnemy.transform.position);
        for (int i = 0; i < enemyList.Length; i++)
        {
            float distanceToTurret = Vector3.Distance(transform.position, enemyList[i].transform.position);
            if (distanceToTurret < distanceToClosestEnemy)
            {
                closestEnemy = enemyList[i].gameObject;
            }
        }
        fireRateTime += Time.deltaTime;
        if (fireRateTime >= fireRate && ammoAmount > 0)
        {
            ammoAmount--;
            fireRateTime -= fireRateTime;
            MoveProjectile newProjectile = Instantiate(projectile, transform.position, Quaternion.Euler(transform.right));
            newProjectile.firedFrom = gameObject;
            newProjectile.RotateToTarget(closestEnemy.transform.position - transform.position);
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
            if (r.type == ammoType)
            {
                ammoAmount++;
            }
        }
        Destroy(collision.gameObject);
    }
}
