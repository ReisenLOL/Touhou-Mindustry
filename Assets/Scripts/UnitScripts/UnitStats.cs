using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public UnitPrice price;
    public GameObject upgradesTo;
    public int unitTier;
    public float health;
    public float maxHealth;
    public bool isEnemy = false;
    public bool isPlayer = false;
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (isPlayer)
            {
                Debug.Log("here");
                gameObject.transform.position = GameObject.Find("Core").transform.position;
                health = maxHealth;
            }
            else
            {
                Debug.Log("here2");
                Destroy(gameObject);
            }
        }
    }
}
