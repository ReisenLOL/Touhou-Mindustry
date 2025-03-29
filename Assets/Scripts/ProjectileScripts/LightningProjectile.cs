using Core.Extensions;
using UnityEngine;

public class LightningProjectile : Projectile
{
    public float lightningLength;
    public float _dissipationTimeElapsed;
    public float dissipationTime;
    public LayerMask AttackLayers;
    private void Start()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, directionOfTarget, lightningLength, AttackLayers);
        transform.parent = firedFrom.transform;
        transform.localScale = new Vector2(lightningLength, transform.localScale.y);
        transform.Translate(Vector2.right * lightningLength/2); //...how the hell do i make the lightning effect ...i guess ill just do sprites that look like lightning for now
        if (hit != null)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (isEnemyBullet)
                {
                    if (hit[i].collider.gameObject.CompareTag("Player") || (hit[i].collider.gameObject.CompareTag("Unit") && !hit[i].collider.gameObject.GetComponent<UnitStats>().isEnemy))
                    {
                        Debug.Log("here");
                        hit[i].collider.gameObject.GetComponent<UnitStats>().TakeDamage(damage);
                    }
                    else if (hit[i].collider.gameObject.CompareTag("Building"))
                    {
                        Debug.Log("here2");
                        hit[i].collider.gameObject.GetComponent<ObjectStats>().TakeDamage(damage);
                    }
                }
                else if (hit[i].collider.gameObject.CompareTag("Unit") && hit[i].collider.gameObject.GetComponent<UnitStats>().isEnemy)
                {
                    Debug.Log("here3");
                    hit[i].collider.gameObject.GetComponent<UnitStats>().TakeDamage(damage);
                }
            }
        }
    }
    void Update()
    {
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
}
