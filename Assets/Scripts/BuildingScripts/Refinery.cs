using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Refinery : MonoBehaviour
{
    public string[] resourcesToRefine;
    public string outputResource;
    [SerializeField] GameObject resourceObject;
    [SerializeField] Transform[] conveyorChecks;
    [SerializeField] LayerMask conveyorLayer;
    private List<Transform> inputConveyors = new(); //you really are an idiot sylvia
    private GameObject resourceFolder;
    private Transform nextConveyorCheck;
    private bool canProduce;
    Dictionary<string, int> storedResources = new();
    private int storedOutput;
    [SerializeField] int capacity;
    private int conveyorIndex = 0;
    private float _time;
    public float tickSpeed;
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
    void Start()
    {
        resourceFolder = GameObject.Find("ResourceFolder");
    }
    void Update()
    {
        Collider2D conveyor = DetectConveyors(conveyorIndex);
        if (conveyor == null || conveyor.GetComponent<ObjectStats>().acceptingResources == false)
        {
            CycleConveyorIndex();
            return;
        }
        if (inputConveyors.Contains(conveyor.transform))
        {
            CycleConveyorIndex();
            return;
        }
        nextConveyorCheck = conveyor.transform;
        _time += Time.deltaTime;
        if (_time >= tickSpeed)
        {
            bool success = false;
            for (int i = 0; i < resourcesToRefine.Length; i++)
            {
                if (!storedResources.ContainsKey(resourcesToRefine[i]))
                {
                    success = false;
                    break;
                }
                if (storedResources[resourcesToRefine[i]] >= 1)
                {
                    success = true;
                }
                else
                {
                    success = false;
                    break;
                }
            }
            if (success)
            {
                _time = 0;
                for (int i = 0; i < resourcesToRefine.Length; i++)
                {
                    SubtractResource(resourcesToRefine[i], 1); //currently only 1? idk i can figure out how to do separate subtractions for each one later
                }
                GameObject FusedResource = Instantiate(resourceObject, nextConveyorCheck.position, resourceObject.transform.rotation);
                FusedResource.GetComponent<MinedResourceType>().type = outputResource;
                FusedResource.transform.SetParent(resourceFolder.transform);
                CycleConveyorIndex();
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != null && collision.transform.TryGetComponent(out MinedResourceType output))
        {
            for (int i = 0; i < resourcesToRefine.Length; i++)
            {
                if (output.type == resourcesToRefine[i]) // you're an idiot, sylvia.
                {
                    Transform newInput = collision.GetComponent<MoveResource>().previousConveyor;
                    bool equalsConveyor = false;
                    if (inputConveyors != null)
                    {
                        for (int j = 0; j < inputConveyors.Count; j++)
                        {
                            if (newInput == inputConveyors[j])
                            {
                                equalsConveyor = true;
                                break;
                            }
                        }
                        if (!equalsConveyor)
                        {
                            inputConveyors.Add(newInput);
                        }
                    }
                    else
                    {
                        inputConveyors.Add(newInput);
                    }
                    // it cant remove new ones yet idk how to fix that
                    AddResource(output.type, 1);
                    break;
                }
            }
            Destroy(collision.gameObject);
        }
    }
}
