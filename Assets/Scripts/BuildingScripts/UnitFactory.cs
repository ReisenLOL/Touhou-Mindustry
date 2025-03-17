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
    [SerializeField] Transform[] upgraderChecks;
    [SerializeField] string[] requiredResources;
    [SerializeField] int[] requiredAmount;
    [SerializeField] int capacity;
    [SerializeField] Transform SpawnLocation;
    Dictionary<string, int> storedResources = new();
    private ObjectStats buildingStats;
    private GameObject unitFolder;
    private float productionTimer;
    private bool isProducing = false;
    private Collider2D UpgraderObject;
    [SerializeField] float requiredManufacturingTime;
    [SerializeField] Transform progressBar;
    [SerializeField] LayerMask conveyorLayer;
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
            progressBar.localScale = new Vector3(productionTimer / requiredManufacturingTime, progressBar.localScale.y, progressBar.localScale.z);
            bool unitIsUpgrading = false;
            for (int i = 0; i < upgraderChecks.Length; i++)
            {
                UpgraderObject = DetectUpgraders(upgraderChecks[i]);
                if (UpgraderObject != null && UpgraderObject.gameObject.TryGetComponent(out UnitUpgrader upgrader) && upgrader != null && upgrader.upgraderTier == 1)
                {
                    unitIsUpgrading = true;
                    break;
                }
            }
            if (productionTimer >= requiredManufacturingTime)
            {
                isProducing = false;
                productionTimer = 0;
                if (unitIsUpgrading)
                {
                    UpgraderObject.GetComponent<UnitUpgrader>().unitToUpgrade = unitToSpawn;
                }
                else
                {
                    GameObject newUnit = Instantiate(unitToSpawn, SpawnLocation.position, unitToSpawn.transform.rotation);
                    newUnit.transform.SetParent(unitFolder.transform);
                }
            }
        }
        if (!isProducing && unitToSpawn != null)
        {
            bool canProduce = true;
            for (int i = 0; i < requiredResources.Length; i++)
            {
                if (CheckResourceValue(requiredResources[i]) < requiredAmount[i])
                {
                    Debug.Log("stopped producing");
                    canProduce = false;
                    break;
                }
            }
            if (canProduce)
            {
                isProducing = true;
                Debug.Log("started producing");
                for (int i = 0; i < requiredResources.Length; i++)
                {
                    SubtractResource(requiredResources[i], requiredAmount[i]);
                }
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out MinedResourceType r))
        {
            for (int i = 0; i < requiredResources.Length; i++)
            {
                if (r.type == requiredResources[i] && CheckResourceValue(requiredResources[i]) < capacity)
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
    private Collider2D DetectUpgraders(Transform upgraderCheck)
    {
        return Physics2D.OverlapCircle(upgraderCheck.position, 0.05f, conveyorLayer);
    }
}
