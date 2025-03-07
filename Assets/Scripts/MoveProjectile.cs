using UnityEngine;
using Core.Extensions;

public class MoveProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    public float damage;
    public GameObject firedFrom;
    private float distanceToTurret;
    public bool isEnemyBullet = false;
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);
        if (firedFrom != null)
        {
            distanceToTurret = Vector3.Distance(transform.position, firedFrom.transform.position);
        }
        if (distanceToTurret > 100)
        {
            Destroy(gameObject);
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
        }
    }
    public void RotateToTarget(Vector2 direction)
    {
        transform.Lookat2D(direction);
    }
}
