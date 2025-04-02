using UnityEngine;

public class ObjectStats : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public ObjectPrice price;
    public bool usesEnergy = false;
    public bool connectedToEnergyNode = false;
    public bool acceptingResources = true;
    public bool isCore = false;
    public Vector2 gridLocation;
    public string[] resourceTypeInput;
    public int[] inputAmount;
    public string category;
    public bool refreshBuildings = true;
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
