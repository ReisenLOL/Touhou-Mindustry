using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100;
    [SerializeField] float speed;
    private GameObject core;
    private GameObject player;
    private Rigidbody2D rb;
    private Vector3 lookDirection;
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
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = lookDirection * speed;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
