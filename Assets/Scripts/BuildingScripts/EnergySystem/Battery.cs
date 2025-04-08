using UnityEngine;

public class Battery : MonoBehaviour
{
    public int energyStored = 0;
    public int energyCapacity;
    public void AddEnergy(int amount)
    {
        if (energyStored < energyCapacity)
        {
            if ((energyStored + amount) < energyCapacity)
            {
                energyStored += amount;
            }
            else
            {
                energyStored = energyCapacity;
            }
        }
    }
    public bool RemoveEnergy(int amount)
    {
        if ((energyStored - amount) >= 0)
        {
            energyStored -= amount;
            return true;
        }
        return false;
    }
}
