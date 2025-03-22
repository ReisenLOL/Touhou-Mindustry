using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    Dictionary<string, int> currentResources = new();
    [SerializeField] ResourceList listOfResources;
    [SerializeField] TextMeshProUGUI resourceText;
    [SerializeField] bool debugResources;
    public void AddResource(string resource, int value)
    {
        int resourceValue = 0;
        if (!currentResources.ContainsKey(resource))
        {
            currentResources[resource] = 0;
        }
        if (currentResources.TryGetValue(resource, out resourceValue))
        {
            resourceValue += value;
            currentResources[resource] = resourceValue;
            Debug.Log("Added Resource: " + value + " " + resource);
        }
        string newValueToDisplay = "";
        for (int i = 0; i < listOfResources.resourceType.Length; i++)
        {
            int getValue = CheckResourceValue(listOfResources.resourceType[i]);
            if (getValue > 0)
            {
                newValueToDisplay += listOfResources.resourceType[i] + ": " + getValue + "\n";
            }
        }
        resourceText.text = newValueToDisplay;
    }
    public bool SubtractResource(string resource, int value)
    {
        int resourceValue = 0;
        if (!currentResources.ContainsKey(resource))
        {
            currentResources[resource] = 0;
        }
        if (currentResources.TryGetValue(resource, out resourceValue))
        {
            resourceValue -= value;
            currentResources[resource] = resourceValue;
            Debug.Log("Subtracted Resource: " + value + " " + resource);
            string newValueToDisplay = "";
            for (int i = 0; i < listOfResources.resourceType.Length; i++)
            {
                int getValue = CheckResourceValue(listOfResources.resourceType[i]);
                if (getValue > 0)
                {
                    newValueToDisplay += listOfResources.resourceType[i] + ": " + getValue + "\n";
                }
            }
            resourceText.text = newValueToDisplay;
            return true;
        }
        return false;
    }
    public int CheckResourceValue(string resource)
    {
        if (!currentResources.ContainsKey(resource))
        {
            return 0;
        }
        return currentResources[resource];
    }
    void Start()
    {
        AddResource("Fairy Compound", 100);
        if (debugResources)
        {
            AddResource("Fairy Compound", 1000);
            AddResource("Lunarian Metal", 1000);
            AddResource("Youkai Alloy", 1000);
        }  
    }
}
