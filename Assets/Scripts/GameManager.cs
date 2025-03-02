using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

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
        resourceText.text = ("Fairy Compound: " + CheckResourceValue("Fairy Compound"));
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
            if (Input.GetMouseButtonDown(0) && CanPlaceThere() != "Building" && !EventSystem.current.IsPointerOverGameObject())
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
    }
    public void EnterBuildMode()
    {
        showPlaceholder = true;
        isBuilding = true;
        playerController.canShoot = false;
    }
    public void ExitBuildMode()
    {
        showPlaceholder = false;
        if (placeholderObject)
        {
            Destroy(placeholderObject);
        }
        isBuilding = false;
        playerController.canShoot = true;
    }
    string CanPlaceThere()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            return hit.collider.gameObject.tag;
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
    }
}
