using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unloader : MonoBehaviour
{
    private string selectedResource;
    [SerializeField] GameObject resourceObject;
    private float movementspeed;
    [SerializeField] Transform[] conveyorChecks;
    [SerializeField] LayerMask conveyorLayer;
    private GameObject resourceFolder;
    [SerializeField] ResourceList resourceList;
    [SerializeField] GameObject selectionBox;
    [SerializeField] GameObject templateButton;
    [SerializeField] GameObject selectionUI;
    [SerializeField] GameObject selectionUIContainer;
    private GameManager gameManager;
    private bool outputFromCore;
    private Transform nextConveyorCheck = null;
    private int conveyorIndex = 0;
    private float _time;
    private float tickSpeed = 0.5f;
    private void SelectResource(string selection)
    {
        selectedResource = selection;
    }
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        for (int i = 0; i < conveyorChecks.Length; i++)
        {
            Collider2D conveyor = DetectConveyors(i);
            if (conveyor != null)
            {
                if (conveyor.gameObject.TryGetComponent(out CoreController output) && output != null)
                {
                    outputFromCore = true;
                }
            }
        }
        resourceFolder = GameObject.Find("ResourceFolder");
        for (int i = 0; i < resourceList.resourceType.Length; i++)
        {
            GameObject newButton = Instantiate(templateButton);
            newButton.SetActive(true);
            newButton.transform.SetParent(selectionUI.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = resourceList.resourceType[i];
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectResource(newButton.GetComponentInChildren<TextMeshProUGUI>().text));
        }
    }
    void Update()
    {
        if (outputFromCore && selectedResource != null)
        {
            Collider2D conveyor = DetectConveyors(conveyorIndex);
            if (conveyor == null || conveyor.gameObject.TryGetComponent(out CoreController output) && output == true)
            {
                CycleConveyorIndex();
                return;
            }
            if (GetComponent<ObjectStats>().acceptingResources == false)
            {
                CycleConveyorIndex();
                return;
            }
            nextConveyorCheck = conveyor.transform;
            _time += Time.deltaTime;
            if (_time >= tickSpeed)
            {
                bool success = gameManager.CheckResourceValue(selectedResource) >= 1;
                if (success)
                {
                    _time = 0;
                    gameManager.SubtractResource(selectedResource, 1);
                    GameObject UnloadResource = Instantiate(resourceObject, nextConveyorCheck.position, resourceObject.transform.rotation);
                    UnloadResource.GetComponent<MinedResourceType>().type = selectedResource;
                    UnloadResource.transform.SetParent(resourceFolder.transform);
                }
            }
        }
    }
    private void OnMouseDown()
    {
        selectionBox.SetActive(!selectionBox.activeSelf);
        selectionUIContainer.SetActive(!selectionUIContainer.activeSelf);
    }
    private Collider2D DetectConveyors(int index)
    {
        return Physics2D.OverlapCircle(this.conveyorChecks[index].position, 0.05f, conveyorLayer);
    }
    private void CycleConveyorIndex()
    {
        conveyorIndex++;
        if (conveyorIndex >= conveyorChecks.Length)
        {
            conveyorIndex = 0;
        }
    }

}
