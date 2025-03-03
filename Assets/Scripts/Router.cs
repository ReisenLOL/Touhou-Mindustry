using Core.Extensions;
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
    private HashSet<MoveResource> moveResource = new();
    void Update()
    {
        int randomIndex = Random.Range(0, conveyorChecks.Length);
        Collider2D conveyor = null;
        if (DetectConveyors(randomIndex) is not null and Collider2D c)
        {
            conveyor = c;
        }
        if (conveyor == null)
        {
            return;
        }
        nextConveyorCheck = conveyor.transform;
        foreach (var item in moveResource)
        {
            if (item == null)
            {
                continue;
            }
            if (item.previousConveyor == nextConveyorCheck)
            {
                continue;
            }
            item.MoveToNextConveyor(nextConveyorCheck, movementSpeed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != null && collision.transform.TryGetComponent(out MoveResource output))
        {
            moveResource.Add(output);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform != null && collision.transform.TryGetComponent(out MoveResource output))
        {
            moveResource.Remove(output);
        }
    }
    private Collider2D DetectConveyors(int index)
    {
        return Physics2D.OverlapCircle(this.conveyorChecks[index].position, 0.05f, conveyorLayer);
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