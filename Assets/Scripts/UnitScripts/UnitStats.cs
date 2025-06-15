using System;
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
    public float maxHealthModifier = 1f;

    private void Start()
    {
        maxHealth *= maxHealthModifier;
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(damage);
        health -= damage;
        if (health <= 0)
        {
            if (isPlayer)
            {
                gameObject.transform.position = FindAnyObjectByType<CoreController>().transform.position;
                health = maxHealth;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
