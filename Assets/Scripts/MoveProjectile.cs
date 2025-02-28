using UnityEngine;
using Core.Extensions;

public class MoveProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    private Vector3 lookDirection;
    private void Start()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane + 10;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.Lookat2D(worldPos);
    }
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
