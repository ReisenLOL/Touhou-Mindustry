using UnityEngine;
using Core.Extensions;

public class MovePlayerProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] GameObject player;
    private Vector3 lookDirection;
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > 100)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Unit") && collision.GetComponent<UnitStats>().isEnemy)
        {
            collision.gameObject.GetComponent<UnitStats>().TakeDamage(damage);
        }
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
