using System.Collections.Generic;
using UnityEngine;

public class EnergyGenerator : Refinery
{
    public int energyStored;
    public int energyToProduce;
    public int energyMaxCapacity;
    Dictionary<string, int> storedEnergyResources = new();
    private float _ticktime;
    public void AddResourceForEnergy(string resource, int value)
    {
        int resourceValue = 0;
        if (!storedEnergyResources.ContainsKey(resource))
        {
            storedEnergyResources[resource] = 0;
        }
        if (storedEnergyResources.TryGetValue(resource, out resourceValue))
        {
            resourceValue += value;
            storedEnergyResources[resource] = resourceValue;
        }
    }
    public bool SubtractResourceForEnergy(string resource, int value)
    {
        int resourceValue = 0;
        if (!storedEnergyResources.ContainsKey(resource))
        {
            storedEnergyResources[resource] = 0;
        }
        if (storedEnergyResources.TryGetValue(resource, out resourceValue))
        {
            resourceValue -= value;
            storedEnergyResources[resource] = resourceValue;
            return true;
        }
        return false;
    }
    void Update()
    {
        _ticktime += Time.deltaTime;
        if (_ticktime >= tickSpeed)
        {
            bool success = false;
            for (int i = 0; i < resourcesToRefine.Length; i++)
            {
                if (!storedEnergyResources.ContainsKey(resourcesToRefine[i]))
                {
                    success = false;
                    break;
                }
                if (storedEnergyResources[resourcesToRefine[i]] >= 1 && energyStored < energyMaxCapacity)
                {
                    success = true;
                }
                else
                {
                    success = false;
                    break;
                }
            }
            if (success)
            {
                _ticktime = 0;
                for (int i = 0; i < resourcesToRefine.Length; i++)
                {
                    SubtractResourceForEnergy(resourcesToRefine[i], 1);
                }
                Debug.Log("here5");
                energyStored += energyToProduce;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != null && collision.transform.TryGetComponent(out MinedResourceType output))
        {
            for (int i = 0; i < resourcesToRefine.Length; i++)
            {
                if (output.type == resourcesToRefine[i]) // you're an idiot, sylvia. past sylvia that's very mean of you stop saying those things...
                {
                    AddResourceForEnergy(output.type, 1);
                    break;
                }
            }
            Destroy(collision.gameObject);
        }
    }
}
