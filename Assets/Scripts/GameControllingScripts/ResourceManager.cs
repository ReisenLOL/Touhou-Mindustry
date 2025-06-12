using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public class ResourceUIDisplayValues
    {
        public GameObject resourceUI;
        public string resourceType;
    }
    Dictionary<string, int> currentResources = new();
    [SerializeField] ResourceList listOfResources;
    [SerializeField] bool debugResources;
    public List<ResourceUIDisplayValues> ResourceUIs = new();
    public GameObject ResourceUITemplate;
    public Transform ResourceUIPanel;
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
        for (int i = 0; i < listOfResources.resourceType.Length; i++)
        {
            int getValue = CheckResourceValue(listOfResources.resourceType[i]);
            if (getValue > 0)
            {
                if (ResourceUIPanel.Find(listOfResources.resourceType[i]))
                {
                    ResourceUIPanel.Find(listOfResources.resourceType[i]).GetComponentInChildren<TextMeshProUGUI>().text = listOfResources.resourceType[i] + ": " + getValue; //bad code
                }
                else
                {
                    GameObject newResourceUI = Instantiate(ResourceUITemplate, ResourceUIPanel);
                    newResourceUI.SetActive(true);
                    newResourceUI.name = listOfResources.resourceType[i];
                    newResourceUI.GetComponentInChildren<TextMeshProUGUI>().text = listOfResources.resourceType[i] + ": " + getValue;
                    newResourceUI.GetComponentInChildren<Image>().color = listOfResources.resourceColors[i];
                }
            }
        }
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
            for (int i = 0; i < listOfResources.resourceType.Length; i++)
            {
                int getValue = CheckResourceValue(listOfResources.resourceType[i]);
                if (getValue > 0)
                {
                    ResourceUIPanel.Find(listOfResources.resourceType[i]).GetComponentInChildren<TextMeshProUGUI>().text = listOfResources.resourceType[i] + ": " + getValue; //bad code
                }
            }
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
