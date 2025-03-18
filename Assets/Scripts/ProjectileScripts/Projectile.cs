using Core.Extensions;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isEnemyBullet = false;
    public float speed;
    public float damage;
    public float maxRange;
    public GameObject firedFrom;
    public Vector2 directionOfTarget;
    public void RotateToTarget(Vector2 direction)
    {
        directionOfTarget = direction;
        transform.Lookat2D(direction);
    }
}
