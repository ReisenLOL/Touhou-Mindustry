using Core.Extensions;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private Vector3 rotationAmount = new Vector3 (0, 0, 0);
    private bool isBuilding = false;
    public GameObject player;
    public PlayerController playerController;
    private ResourceManager resourceManager;
    private Grid buildingGrid;
    public GameObject selection;
    private bool showPlaceholder;
    public GameObject placeholderObject;
    public GameObject[] buildList;
    public TextMeshProUGUI objectStatText;
    private GameObject buildingFolder;
    private Tilemap terrainTiles;
    private LayerMask resourceVeinLayer;
    private Vector2 selectionSize;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] LayerMask buildingLayer;
    [SerializeField] GameObject showInfoButton;
    public GameObject newPlayer;
    private bool isConstructingObject;
    private float constructionTime;
    public string[] buildingCategories;
    public CoreController core;
    // oh man this is gonna take awhile....
    //Queue<GameObject> buildingQueue = new Queue<GameObject>();
    
    //wow i really think this code is inefficient but whatevs i guess
    void Start()
    {
        core = FindFirstObjectByType<CoreController>();
        buildingFolder = GameObject.Find("BuildingFolder");
        resourceManager = GetComponent<ResourceManager>();
        player = FindFirstObjectByType<PlayerController>().gameObject;
        playerController = player.GetComponent<PlayerController>();
        buildingGrid = GameObject.Find("BuildingGrid").GetComponent<Grid>();
        terrainTiles = GameObject.Find("TerrainTilemap").GetComponent<Tilemap>();
        for (int i = 0; i < buildingCategories.Length; i++)
        {
            GameObject categoryObject = new GameObject(buildingCategories[i]);
            categoryObject.transform.parent = buildingFolder.transform;
        }
    }
    void Update()
    {
        // i bet i can split the building functions out from the game controller
        // or rather probably better to just split the resource functions.
        // i need to figure out what's causing the horrific lag from the building, when i was testing the game out, it REALLY became laggier every time i entered build mode.
        if (isBuilding)
        {
            string[] resourceToSubtract = selection.gameObject.GetComponent<ObjectStats>().price.GetResourceName();
            int[] cost = selection.gameObject.GetComponent<ObjectStats>().price.GetAmount();
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane + 10;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 buildingGridEvenCell = buildingGrid.WorldToCell(worldPos);
            Vector3 buildingGridCenterCell = buildingGrid.GetCellCenterWorld(buildingGrid.WorldToCell(worldPos));
            if (showPlaceholder)
            {
                selectionSize = selection.GetComponent<BoxCollider2D>().size;
                if (selectionSize.x % 2 == 0)
                {
                    placeholderObject = Instantiate(selection, buildingGridEvenCell, selection.transform.rotation);
                }
                else
                {
                    placeholderObject = Instantiate(selection, buildingGridCenterCell, selection.transform.rotation);
                }
                Component[] allComponents = placeholderObject.GetComponentsInChildren(typeof(MonoBehaviour));
                foreach (Component gameObjectComponent in allComponents)
                {
                    Destroy((MonoBehaviour)gameObjectComponent);
                }
                placeholderObject.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.7f);
                placeholderObject.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                placeholderObject.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                placeholderObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                placeholderObject.tag = "Building";
                showPlaceholder = false;
            }
            if (selectionSize.x % 2 == 0)
            {
                placeholderObject.transform.position = buildingGridEvenCell;
            }
            else 
            {
                placeholderObject.transform.position = buildingGridCenterCell;
            }
            placeholderObject.transform.rotation = Quaternion.Euler(rotationAmount);
            if (Input.GetMouseButtonDown(0) && CanPlaceThere(true) != "Building" && !EventSystem.current.IsPointerOverGameObject() && terrainTiles.GetTile(buildingGrid.WorldToCell(worldPos)).name != "deepwater" && terrainTiles.GetTile(buildingGrid.WorldToCell(worldPos)).name != "wall")
            {
                bool success = false;
                for (int i = 0; i < resourceToSubtract.Length; i++)
                {
                   success = resourceManager.CheckResourceValue(resourceToSubtract[i]) >= cost[i];
                }
                if (success)
                {
                    //buildingQueue.Enqueue(selection);
                    //isConstructingObject = true;
                    for (int i = 0; i < resourceToSubtract.Length; i++)
                    {
                        resourceManager.SubtractResource(resourceToSubtract[i], cost[i]);
                    }
                    GameObject newObject;
                    if (selectionSize.x % 2 == 0)
                    {
                        newObject = Instantiate(selection, buildingGridEvenCell, Quaternion.Euler(rotationAmount));
                        newObject.GetComponent<ObjectStats>().gridLocation = new(buildingGridEvenCell.x, buildingGridEvenCell.y);
                    }
                    else
                    {
                        newObject = Instantiate(selection, buildingGridCenterCell, Quaternion.Euler(rotationAmount));
                        newObject.GetComponent<ObjectStats>().gridLocation = new(buildingGridCenterCell.x, buildingGridCenterCell.y);
                    }
                    newObject.transform.SetParent(GameObject.Find(newObject.GetComponent<ObjectStats>().category).transform);
                    core.GetComponent<ObjectStats>().refreshBuildings = true;
                    for (int i = 0; i < buildingFolder.transform.childCount; i++)
                    {
                        for (int j = 0; j < buildingFolder.transform.GetChild(i).childCount; i++)
                        {
                            buildingFolder.transform.GetChild(i).GetChild(j).GetComponent<ObjectStats>().refreshBuildings = true;
                        }

                    } //WHAT THE FUCK
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
            // do i keep these yapping ass comments actually why not they're getting archived here forever
        }
        if (Input.GetMouseButtonDown(1) && CanPlaceThere(true) == "Building" && CanPlaceThere(false) != "Core" && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            string[] resourceToRefund = hit.collider.gameObject.GetComponent<ObjectStats>().price.GetResourceName();
            int[] refundAmount = hit.collider.gameObject.GetComponent<ObjectStats>().price.GetAmount();
            for (int i = 0; i < resourceToRefund.Length; i++)
            {
                resourceManager.AddResource(resourceToRefund[i], refundAmount[i]);
            }
            Destroy(hit.collider.gameObject);
        }
        else if (Input.GetMouseButtonDown(1) && isBuilding)
        {
            SetBuildMode(selection.name);
            showInfoButton.SetActive(false);
        }
        if (!isBuilding)
        {
            string selectedTile = terrainTiles.GetTile(buildingGrid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition))).name;
            objectStatText.transform.parent.gameObject.SetActive(true);
            objectStatText.text = selectedTile;
        }
    }
    public void SetBuildMode(string selectedObject)
    {
        if (selectedObject == selection.name && isBuilding)
        {
            isBuilding = false;
        }
        else
        {
            isBuilding = true;
        }
        if (isBuilding)
        {
            if (placeholderObject)
            {
                Destroy(placeholderObject);
            }
            showPlaceholder = true;
            playerController.canShoot = false;
        }
        else
        {
            objectStatText.transform.parent.gameObject.SetActive(true);
            showPlaceholder = false;
            if (placeholderObject)
            {
                Destroy(placeholderObject);
            }
            playerController.canShoot = true;
        }
    }
    string CanPlaceThere(bool returnTag)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0);
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
        if (selectionSize.x > 1f)
        {
            Collider2D boxColliderHit = Physics2D.OverlapBox(placeholderObject.transform.position, selectionSize * 0.5f, 0f, buildingLayer);
            if (boxColliderHit != null)
            {
                if (returnTag)
                {
                    return boxColliderHit.gameObject.tag;
                }
                else
                {
                    return boxColliderHit.gameObject.name;
                }
            }
        }
        return "go ahead";
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
        ObjectStats selectionStats = selection.gameObject.GetComponent<ObjectStats>();
        string[] resourceToDisplay = selectionStats.price.GetResourceName();
        int[] amountToDisplay = selectionStats.price.GetAmount();
        objectStatText.transform.parent.gameObject.SetActive(true);
        string costDisplay = selection.name + "\n";
        for (int i = 0; i < resourceToDisplay.Length; i++)
        {
            costDisplay += resourceToDisplay[i] + ": " + amountToDisplay[i] + "\n";
        }
        objectStatText.text = costDisplay;
        showInfoButton.SetActive(true);
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
    private void OnDrawGizmos()
    {
        if (isBuilding)
        {
            Gizmos.DrawWireSphere(placeholderObject.transform.position, 1);
        }
    }
    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

