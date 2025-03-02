using UnityEngine;
using Core.Extensions;

public class MoveProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    public float damage;
    public GameObject firedFrom;
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);
        float distanceToTurret = Vector3.Distance(transform.position, firedFrom.transform.position);
        if (distanceToTurret > 100)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    public void RotateToTarget(Vector2 direction)
    {
        transform.Lookat2D(direction);
    }
}
