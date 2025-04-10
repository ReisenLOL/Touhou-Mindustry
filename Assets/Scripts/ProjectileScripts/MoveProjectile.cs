using UnityEngine;
using Core.Extensions;

public class MoveProjectile : Projectile
{
    private float distanceToTurret;
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
        if (isEnemyBullet)
        {
            if (collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Unit") && !collision.gameObject.GetComponent<UnitStats>().isEnemy))
            {
                collision.gameObject.GetComponent<UnitStats>().TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (collision.gameObject.CompareTag("Building"))
            {
                collision.gameObject.GetComponent<ObjectStats>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Unit") && collision.gameObject.GetComponent<UnitStats>().isEnemy)
        {
            collision.gameObject.GetComponent<UnitStats>().TakeDamage(damage);
            Destroy(gameObject);
            //?????? WHY IS IT MAKING THE PLAYER TAKE DAMAGE
        }
    }
}
