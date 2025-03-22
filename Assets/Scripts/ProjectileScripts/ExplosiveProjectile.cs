using UnityEngine;
using Core.Extensions;

public class ExplosiveProjectile : Projectile
{
    private float distanceToTurret;
    public float explosionRadius;
    private void Start()
    {
        
    }
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);
        if (firedFrom != null)
        {
            distanceToTurret = Vector3.Distance(transform.position, firedFrom.transform.position);
        }
        if (distanceToTurret > maxRange)
        {
            Destroy(gameObject);
        }
        if (!isEnemyBullet)
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] targets = GetAllTargets(explosionRadius);
        for (int i = 0; i < targets.Length; i++)
        {
            if (isEnemyBullet)
            {
                if (targets[i].gameObject.CompareTag("Player") || (targets[i].gameObject.CompareTag("Unit") && !targets[i].gameObject.GetComponent<UnitStats>().isEnemy))
                {
                    targets[i].gameObject.GetComponent<UnitStats>().TakeDamage(damage);
                    Destroy(gameObject);
                }
                else if (targets[i].gameObject.CompareTag("Building"))
                {
                    targets[i].gameObject.GetComponent<ObjectStats>().TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
            else if (targets[i].gameObject.CompareTag("Unit") && targets[i].gameObject.GetComponent<UnitStats>().isEnemy)
            {
                targets[i].gameObject.GetComponent<UnitStats>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
    private Collider2D[] GetAllTargets(float radius)
    {
        return Physics2D.OverlapCircleAll(transform.position, explosionRadius);
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius); //IT WORKS!
    } */
}
