using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] float speed;
    private Rigidbody2D rb;
    public Vector3 lookDirection;
    public GameObject closestTarget;
    private float fireRateTime;
    public float fireRate;
    public float damageDealt;
    [SerializeField] MoveProjectile projectile;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        fireRateTime += Time.deltaTime;
        if (fireRateTime >= fireRate && closestTarget != null)
        {
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
}
