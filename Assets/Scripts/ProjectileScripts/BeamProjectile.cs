using UnityEngine;

public class BeamProjectile : Projectile
{
    public float BeamLength;
    public float _dissipationTimeElapsed;
    public float dissipationTime;
    [SerializeField] float _time;
    [SerializeField] float beamTime;
    private void Start()
    {
        transform.localScale = new Vector2(BeamLength, transform.localScale.y);
        transform.Translate(Vector2.right * BeamLength / 2);
    }
    void Update()
    {
        _time += Time.deltaTime;
        _dissipationTimeElapsed += Time.deltaTime;
        if (!isEnemyBullet)
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
        }
        if (_dissipationTimeElapsed >= dissipationTime)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_time >= beamTime)
        {
            if (isEnemyBullet)
            {
                if (collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Unit") && !collision.gameObject.GetComponent<UnitStats>().isEnemy))
                {
                    collision.gameObject.GetComponent<UnitStats>().TakeDamage(damage);
                }
                else if (collision.gameObject.CompareTag("Building"))
                {
                    collision.gameObject.GetComponent<ObjectStats>().TakeDamage(damage);
                }
            }
            else if (collision.gameObject.CompareTag("Unit") && collision.gameObject.GetComponent<UnitStats>().isEnemy)
            {
                collision.gameObject.GetComponent<UnitStats>().TakeDamage(damage);
            }
            _time = 0;
        }
    }
}