using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private Vector3 rotationAmount = new Vector3 (0, 0, 0);
    private bool isBuilding = false;
    private GameObject player;
    private PlayerController playerController;
    private Grid buildingGrid;
    public GameObject selection;
    private bool showPlaceholder;
    public GameObject placeholderObject;
    [SerializeField] TextMeshProUGUI resourceText;
    private GameObject[] buildList;
    public GameObject objectStatText;
    Dictionary<string, int> currentResources = new();
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
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        buildingGrid = GameObject.Find("BuildingGrid").GetComponent<Grid>();
        buildList = GameObject.Find("BuildContainer").GetComponent<BuildUIManager>().buildableObjects;
    }
    void Update()
    {
        resourceText.text = ("Fairy Compound: " + CheckResourceValue("Fairy Compound") + "\nLunarian Metal: " + CheckResourceValue("Lunarian Metal"));
        if (isBuilding)
        {
            string resourceToSubtract = selection.gameObject.GetComponent<ObjectStats>().price.GetResourceName();
            int cost = selection.gameObject.GetComponent<ObjectStats>().price.GetAmount();
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane + 10;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 buildingGridCenterCell = buildingGrid.GetCellCenterWorld(buildingGrid.WorldToCell(worldPos));
            if (showPlaceholder)
            {
                placeholderObject = Instantiate(selection, buildingGridCenterCell, selection.transform.rotation);
                Component[] allComponents = placeholderObject.GetComponentsInChildren(typeof(MonoBehaviour));
                foreach (Component gameObjectComponent in allComponents)
                {
                    Destroy((MonoBehaviour)gameObjectComponent);
                }
                placeholderObject.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                placeholderObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                placeholderObject.tag = "Building";
                showPlaceholder = false;
            }
            placeholderObject.transform.position = buildingGridCenterCell;
            placeholderObject.transform.rotation = Quaternion.Euler(rotationAmount);
            if (Input.GetMouseButtonDown(0) && CanPlaceThere(true) != "Building" && !EventSystem.current.IsPointerOverGameObject())
            {
                bool success = SubtractResource(resourceToSubtract, cost);
                if (success)
                {
                    Instantiate(selection, buildingGridCenterCell, Quaternion.Euler(rotationAmount));
                    Destroy(placeholderObject);
                    showPlaceholder = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (rotationAmount.z >= 360)
                {
                    rotationAmount.z = 0;
                }
                rotationAmount.z += 90;
            }
            // im cooking, no tutorials needed!
            // but how do i get it to not spam the instantiation....
            // ok lets see if this works
            // I COOKED!!!
            // ok well i have to get whatever is on that location and stop it from being allowed to build there but that seems simple enough
            // nice
        }
        //if (Input.GetMouseButtonDown(1) && CanPlaceThere(true) == "Building" && CanPlaceThere(false) == "Core" && !EventSystem.current.IsPointerOverGameObject())
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        //    GameObject refundedObject = GetObjectFromName(CanPlaceThere(false));
        //    string resourceToRefund = refundedObject.gameObject.GetComponent<ObjectStats>().price.GetResourceName();
        //    int refundAmount = refundedObject.gameObject.GetComponent<ObjectStats>().price.GetAmount();
        //    Destroy(hit.collider.gameObject);
        //}
    }
    public void EnterBuildMode()
    {
        showPlaceholder = true;
        isBuilding = true;
        playerController.canShoot = false;
    }
    public void ExitBuildMode()
    {
        objectStatText.SetActive(false);
        showPlaceholder = false;
        if (placeholderObject)
        {
            Destroy(placeholderObject);
        }
        isBuilding = false;
        playerController.canShoot = true;
    }
    string CanPlaceThere(bool returnTag)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            if (returnTag)
            {
                return hit.collider.gameObject.tag;
            }
            else
            {
                return hit.collider.gameObject.name;
            }
        }
        else
        {
            return "go ahead";
        }
    }
    public void SetSelection(string selectedObject)
    {
        for (int i = 0; i < buildList.Length; i++)
        {
            if (buildList[i].name == selectedObject)
            {
                selection = buildList[i];
                Destroy(placeholderObject);
                showPlaceholder = true;
                break;
            }
        }
        objectStatText.SetActive(true);
        objectStatText.GetComponentInChildren<TextMeshProUGUI>().text = (selection.name +"\n" + selection.gameObject.GetComponent<ObjectStats>().price.GetResourceName() + ": " + selection.gameObject.GetComponent<ObjectStats>().price.GetAmount());
    }
    public GameObject GetObjectFromName(string objectName)
    {
        for (int i = 0; i < buildList.Length; i++)
        {
            if (buildList[i].name == CanPlaceThere(false))
            {
                return buildList[i];
            }
        }
        return null;
    }
}
