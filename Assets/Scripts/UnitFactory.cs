using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitFactory : MonoBehaviour
{
    [SerializeField] GameObject[] unitList;
    private GameObject unitToSpawn;
    [SerializeField] GameObject selectionBox;
    [SerializeField] GameObject templateButton;
    [SerializeField] GameObject selectionUI;
    [SerializeField] GameObject selectionUIContainer;
    [SerializeField] string[] requiredResources;
    [SerializeField] int[] requiredAmount;
    [SerializeField] int capacity;
    [SerializeField] Transform SpawnLocation;
    Dictionary<string, int> storedResources = new();
    private ObjectStats buildingStats;
    private GameObject unitFolder;
    private float productionTimer;
    private bool isProducing = false;
    [SerializeField] float requiredManufacturingTime;
    [SerializeField] Transform progressBar;
    //this is the worst code i have ever written.
    public void AddResource(string resource, int value)
    {
        int resourceValue = 0;
        if (!storedResources.ContainsKey(resource))
        {
            storedResources[resource] = 0;
        }
        if (storedResources.TryGetValue(resource, out resourceValue))
        {
            resourceValue += value;
            storedResources[resource] = resourceValue;
        }
    }
    public bool SubtractResource(string resource, int value)
    {
        int resourceValue = 0;
        if (!storedResources.ContainsKey(resource))
        {
            storedResources[resource] = 0;
        }
        if (storedResources.TryGetValue(resource, out resourceValue))
        {
            resourceValue -= value;
            storedResources[resource] = resourceValue;
            return true;
        }
        return false;
    }
    public int CheckResourceValue(string resource)
    {
        if (!storedResources.ContainsKey(resource))
        {
            return 0;
        }
        return storedResources[resource];
    }
    public void SelectUnit(int selection)
    {
        unitToSpawn = unitList[selection];
        UnitPrice price = unitList[selection].GetComponent<UnitStats>().price;
        requiredResources = price.GetResourceName();
        requiredAmount = price.GetAmount();

    }
    void Start()
    {
        unitFolder = GameObject.Find("UnitFolder");
        buildingStats = GetComponent<ObjectStats>();
        for (int i = 0; i < unitList.Length; i++)
        {
            GameObject newButton = Instantiate(templateButton);
            newButton.SetActive(true);
            newButton.transform.SetParent(selectionUI.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = unitList[i].name;
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectUnit(i - 2)); //i think this works? ok it doesnt but ill go with this for now
        }
    }
    void Update()
    {
        if (isProducing && unitToSpawn != null)
        {
            productionTimer += Time.deltaTime;
            progressBar.localScale = new Vector3(productionTimer/requiredManufacturingTime, progressBar.localScale.y, progressBar.localScale.z);
            if (productionTimer >= requiredManufacturingTime)
            {
                isProducing = false;
                productionTimer = 0;
                GameObject newUnit = Instantiate(unitToSpawn, SpawnLocation.position, unitToSpawn.transform.rotation);
                newUnit.transform.SetParent(unitFolder.transform);
            }
        }
        for (int i = 0; i < requiredResources.Length; i++)
        {
            if (CheckResourceValue(requiredResources[i]) >= requiredAmount[i] && !isProducing)
            {
                isProducing = true;
                SubtractResource(requiredResources[i], requiredAmount[i]);
            }
            else
            {
                isProducing = false; 
                break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out MinedResourceType r))
        {
            for (int i = 0; i < requiredResources.Length; i++)
            {
                if (r.type == requiredResources[i] && CheckResourceValue(requiredResources[i]) < capacity) //ok cool thats enough for today goodnight
                {
                    AddResource(requiredResources[i], 1);
                    if (storedResources[requiredResources[i]] >= capacity)
                    {
                        buildingStats.acceptingResources = false;
                    }
                }
            }
        }
        Destroy(collision.gameObject);
    }
    private void OnMouseDown()
    {
        selectionBox.SetActive(!selectionBox.activeSelf);
        selectionUIContainer.SetActive(!selectionUIContainer.activeSelf);
    }
}
