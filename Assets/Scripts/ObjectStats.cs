using UnityEngine;

public class ObjectStats : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public ObjectPrice price;
    public bool acceptingResources = true;
    public bool isCore = false;
    public Vector2 gridLocation;
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (isCore)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
            }
            Destroy(gameObject);
        }
    }
}
