using Core.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Router : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Transform[] conveyorChecks;
    [SerializeField] Transform resourceCheck;
    [SerializeField] LayerMask resourceObjectLayer;
    [SerializeField] LayerMask conveyorLayer;
    [SerializeField] float routerCooldown = 0.1f;
    private Transform nextConveyorCheck = null;
    private HashSet<MoveResource> moveResource = new();
    private Dictionary<MoveResource, Transform> activeResource = new();
    private int conveyorIndex = 0;
    public float cooldownTimer = 0f;
    private int capacity = 2;
    private int resourceCount;
    private ObjectStats routerObjectStats;
    private void Start()
    {
        routerObjectStats = gameObject.GetComponent<ObjectStats>();
    }
    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0)
        {
            return;
        }
        Collider2D conveyor = DetectConveyors(conveyorIndex);
        if (conveyor == null)
        {
            CycleConveyorIndex();
            return;
        }
        nextConveyorCheck = conveyor.transform;
        GameObject.Find("Arrow").transform.position = nextConveyorCheck.position;

        foreach (var item in moveResource)
        {
            if (item == null || activeResource.ContainsKey(item))
            {
                continue;
            }
            if (item.previousConveyor == nextConveyorCheck || nextConveyorCheck.GetComponent<ObjectStats>().acceptingResources == false)
            {
                CycleConveyorIndex();
                continue;
            }
            item.MoveToNextConveyor(nextConveyorCheck, movementSpeed);
            activeResource[item] = nextConveyorCheck;
            cooldownTimer = routerCooldown;
            CycleConveyorIndex();
            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != null && collision.transform.TryGetComponent(out MoveResource output))
        {
            resourceCount++;
            if (resourceCount >= capacity)
            {
                //routerObjectStats.acceptingResources = false;
            }
            moveResource.Add(output);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform != null && collision.transform.TryGetComponent(out MoveResource output))
        {
            resourceCount--;
            //routerObjectStats.acceptingResources = true;
            moveResource.Remove(output);
            activeResource.Remove(output);
        }
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
/*using Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class Router : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Transform[] conveyorChecks;
    [SerializeField] Transform resourceCheck;
    [SerializeField] LayerMask resourceObjectLayer;
    [SerializeField] LayerMask conveyorLayer;
    private Transform nextConveyorCheck = null;
    //private HashSet<MoveResource> moveResource = new();
    private Dictionary<MoveResource, Transform> ConveyorLookup = new();
    void Update()
    {
        Conveyor c = null;
        int randomIndex = Random.Range(0, conveyorChecks.Length);
        GameObject col = DetectConveyors(conveyorChecks[randomIndex].position);

        if (col != null && col.transform != null && col.transform.TryGetComponent(out c))
        {
            if (col.transform.TryGetComponent(out ObjectStats objectStats) && objectStats.acceptingResources)
            {
                nextConveyorCheck = c.transform;
                foreach (var item in ConveyorLookup)
                {
                    if (item.Key == null)
                    {
                        continue;
                    }
                    if (item.Key == nextConveyorCheck)
                    {
                        continue;
                    }
                    item.Key.MoveToNextConveyor(item.Value, movementSpeed);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != null && collision.transform.TryGetComponent(out MoveResource output))
        {
            int randomIndex = Random.Range(0, conveyorChecks.Length);
            GameObject col = DetectConveyors(conveyorChecks[randomIndex].position);
            ConveyorLookup[output] = col.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform != null && collision.transform.TryGetComponent(out MoveResource output))
        {
            ConveyorLookup.Remove(output);
        }
    }

    static Dictionary<Vector2Int, GameObject> gridObjects = new();
    public static bool FindOnGrid(Vector2 position, out GameObject g)
    {
        Vector2Int gridPosition = new(position.x.ToInt(), position.y.ToInt());
        if(gridObjects.TryGetValue(gridPosition, out g))
        {
            return true;
        }
        return g != null;
    }
    private GameObject DetectConveyors(Vector3 position)
    {
        if (FindOnGrid(position, out GameObject g))
        {
            return g;
        }
        return null;
    }
}
*/