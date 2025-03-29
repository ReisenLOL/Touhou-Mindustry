using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public UnitPrice price;
    public GameObject upgradesTo;
    public UnitType unitType;
    public int unitTier;
    public float health;
    public float maxHealth;
    public bool isEnemy = false;
    public bool isPlayer = false;
    public float buildSpeed;
    public bool capableOfBuilding;
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (isPlayer)
            {
                gameObject.transform.position = GameObject.Find("Core").transform.position;
                health = maxHealth;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
