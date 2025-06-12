using System;
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
            Debug.Log("out of range");
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
                Debug.Log("playerorplayerunithit");
            }
            else if (collision.gameObject.CompareTag("Building"))
            {
                collision.gameObject.GetComponent<ObjectStats>().TakeDamage(damage);
                Destroy(gameObject);
                Debug.Log("buildinghit");
            }
        }
        else if (collision.gameObject.CompareTag("Unit") && collision.gameObject.GetComponent<UnitStats>().isEnemy)
        {
            collision.gameObject.GetComponent<UnitStats>().TakeDamage(damage);
            Destroy(gameObject);
            Debug.Log(collision.gameObject.GetComponent<UnitStats>().isEnemy);
            Debug.Log(collision.gameObject.name);
            Debug.Log("enemyhit");
        }
    }
}
