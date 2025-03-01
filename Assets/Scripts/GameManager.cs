using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isBuilding = false;
    private string foundResource;
    private GameObject player;
    private PlayerController playerController;
    private Grid buildingGrid;
    public GameObject selection;
    private bool showPlaceholder;
    public GameObject placeholderObject;
    public int resources = 10;
    [SerializeField] TextMeshProUGUI resourceText;
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        buildingGrid = GameObject.Find("BuildingGrid").GetComponent<Grid>();
    }
    void Update()
    {
        resourceText.text = ("Placeholder Resources: " + resources);
        if (isBuilding)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane + 10;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 buildingGridCenterCell = buildingGrid.GetCellCenterWorld(buildingGrid.WorldToCell(worldPos));
            if (showPlaceholder)
            {
                placeholderObject = Instantiate(selection, buildingGridCenterCell, selection.transform.rotation);
                if (selection.GetComponent<MineResource>())
                {
                    Destroy(placeholderObject.GetComponent<MineResource>());
                }
                placeholderObject.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                placeholderObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                placeholderObject.tag = "Placeholder";
                showPlaceholder = false;
            }
            placeholderObject.transform.position = buildingGridCenterCell;
            if (Input.GetMouseButtonDown(0) && resources >= selection.GetComponent<ObjectStats>().cost && CanPlaceThere() != "Building")
            {
                resources -= selection.GetComponent<ObjectStats>().cost;
                Instantiate(selection, buildingGridCenterCell, selection.transform.rotation);
                Destroy(placeholderObject);
                showPlaceholder = true;
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
            return hit.collider.gameObject.tag;
        }
        else
        {
            return "go ahead";
        }
    }
}
