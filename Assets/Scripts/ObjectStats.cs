using UnityEngine;

public class ObjectStats : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public ObjectPrice price;
    public bool acceptingResources = true;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
