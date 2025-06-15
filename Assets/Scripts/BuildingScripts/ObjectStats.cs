using System;
using UnityEngine;

public class ObjectStats : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public ObjectPrice price;
    public bool usesEnergy = false;
    public bool connectedToEnergyNode = false;
    public EnergyNode connectedEnergyNode; 
    public bool acceptingResources = true;
    public bool isCore = false;
    public Vector2 gridLocation;
    public string[] resourceTypeInput;
    public int[] inputAmount;
    public string category;
    public bool refreshBuildings = true;
    public float healthModifier = 1f;

    private void Start()
    {
        maxHealth *= healthModifier;
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
