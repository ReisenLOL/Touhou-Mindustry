using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed;
    private GameObject core;
    private GameObject player;
    private Rigidbody2D rb;
    private Vector3 lookDirection;
    private Collider2D[] targetList;
    private GameObject closestTarget;
    private float fireRateTime;
    public float fireRate;
    public float damageDealt;
    [SerializeField] MoveProjectile projectile;
    [SerializeField] LayerMask unitLayer;
    void Start()
    {
        core = GameObject.Find("Core");
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float distanceToCore = Vector3.Distance(transform.position, core.transform.position);
        if (distanceToPlayer < distanceToCore)
        {
            lookDirection = (player.transform.position - transform.position).normalized;
        }
        else
        {
            lookDirection = (core.transform.position - transform.position).normalized;
        }

        targetList = DetectTargets();
        if (targetList.Length == 0)
        {
            closestTarget = this.gameObject;
        }
        float distanceToClosestTarget = 1000000f;
        Collider2D iteration = null;
        for (int i = 0; i < targetList.Length; i++)
        {
            iteration = targetList[i];
            if (iteration == null)
            {
                continue;
            }
            float sqrDistance = Vector3.SqrMagnitude(transform.position - iteration.transform.position);
            if (sqrDistance < distanceToClosestTarget && iteration.gameObject != gameObject)
            {
                closestTarget = iteration.gameObject;
                distanceToClosestTarget = sqrDistance;
            }
        }
        fireRateTime += Time.deltaTime;
        if (fireRateTime >= fireRate)
        {
            Debug.Log(closestTarget.name);
            fireRateTime -= fireRateTime;
            MoveProjectile newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
            newProjectile.firedFrom = gameObject;
            newProjectile.damage = damageDealt;
            newProjectile.RotateToTarget(closestTarget.transform.position);
        }
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = lookDirection * speed;
    }
    private Collider2D[] DetectTargets()
    {
        return Physics2D.OverlapCircleAll(transform.position, 50f, ~unitLayer);
    }
}
