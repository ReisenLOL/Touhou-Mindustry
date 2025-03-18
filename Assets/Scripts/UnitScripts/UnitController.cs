using System.Collections.Generic;
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
    private UnitStats unitStats;
    private float range;
    [SerializeField] GameObject projectile;
    private SpriteRenderer unitSpriteRenderer;
    private bool isFacingRight = true;
    void Start()
    {
        range = GetComponent<UnitTargetter>().range;
        unitSpriteRenderer = base.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        unitStats = GetComponent<UnitStats>();
    }

    void Update()
    {
        fireRateTime += Time.deltaTime;
        if (fireRateTime >= fireRate && closestTarget != null)
        {
            fireRateTime -= fireRateTime;
            //wow i need to figure this out, how do i do multiple projectile types that have a different... I GUESS IT WORKS FOR BOTH PROJECTILES RN?? wait no it doesnt, i need it to raycast the length then- could i just do it from the start of the lightning projectile?
            GameObject newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation);
            newProjectile.transform.parent = transform;
            Projectile projectileStats = newProjectile.GetComponent<Projectile>();
            projectileStats.firedFrom = gameObject;
            projectileStats.damage = damageDealt;
            projectileStats.RotateToTarget(closestTarget.transform.position);
            projectileStats.isEnemyBullet = unitStats.isEnemy;
            projectileStats.maxRange = range;
        }
        if (lookDirection.x > 0f && this.isFacingRight)
        {
            unitSpriteRenderer.flipX = true;
            this.isFacingRight = !this.isFacingRight;
        }
        else if (lookDirection.x < 0f && !this.isFacingRight)
        {
            unitSpriteRenderer.flipX = false;
            this.isFacingRight = !this.isFacingRight;
        }
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = lookDirection * speed;
    }
}
