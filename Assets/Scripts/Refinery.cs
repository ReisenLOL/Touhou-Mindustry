using System.Collections.Generic;
using UnityEngine;

public class Refinery : MonoBehaviour
{
    public string[] resourcesToRefine;
    public string outputResource;
    [SerializeField] GameObject resourceObject;
    [SerializeField] Transform[] conveyorChecks;
    [SerializeField] LayerMask conveyorLayer;
    private GameObject resourceFolder;
    private Transform nextConveyorCheck;
    Dictionary<string, int> storedResources = new();
    private int conveyorIndex = 0;
    private float _time;
    private float tickSpeed;
    void Start()
    {
        resourceFolder = GameObject.Find("ResourceFolder");
    }
    void Update()
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
            bool success = true;
            if (success)
            {
                _time = 0;
                GameObject UnloadResource = Instantiate(resourceObject, nextConveyorCheck.position, resourceObject.transform.rotation);
                UnloadResource.GetComponent<MinedResourceType>().type = outputResource;
                UnloadResource.transform.SetParent(resourceFolder.transform);
            }
        }
    }
    private void CycleConveyorIndex()
    {
        conveyorIndex++;
        if (conveyorIndex >= conveyorChecks.Length)
        {
            conveyorIndex = 0;
        }
    }
    private Collider2D DetectConveyors(int index)
    {
        return Physics2D.OverlapCircle(this.conveyorChecks[index].position, 0.05f, conveyorLayer);
    }
}
