using Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGenerator : Refinery
{
    [SerializeField] float range;
    [SerializeField] LayerMask BuildingLayer;
    public int energyToProduce;
    private ObjectStats objectStats;
    Dictionary<string, int> storedEnergyResources = new();
    private float _ticktime;
    private List<GameObject> connectedBatteries = new();
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
    private void Start()
    {
        objectStats = GetComponent<ObjectStats>();
    }
    void Update()
    {
        _ticktime += Time.deltaTime;
        if (objectStats.refreshBuildings)
        {
            connectedBatteries.Clear();
            ConnectToBattery();
            objectStats.refreshBuildings = false;
        }
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
                if (storedEnergyResources[resourcesToRefine[i]] >= 1)
                {
                    success = true;
                }
                else
                {
                    success = false;
                    break;
                }
            }
            if (success && connectedBatteries.Count > 0)
            {
                _ticktime = 0;
                for (int i = 0; i < resourcesToRefine.Length; i++)
                {
                    SubtractResourceForEnergy(resourcesToRefine[i], 1);
                }
                for (int i = 0; i < connectedBatteries.Count; i++)
                {
                    connectedBatteries[i].GetComponent<Battery>().AddEnergy(energyToProduce);
                }
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
    private Collider2D[] DetectBuildings()
    {
        return Physics2D.OverlapCircleAll(transform.position, range, BuildingLayer);
    }
    private void ConnectToBattery()
    {
        Collider2D[] detectedBuildings = DetectBuildings();
        for (int i = 0; i < detectedBuildings.Length; i++)
        {
            if (detectedBuildings[i].gameObject.TryGetComponent(out Battery battery))
            {
                connectedBatteries.Add(detectedBuildings[i].gameObject);
            }
        }
    }
}
