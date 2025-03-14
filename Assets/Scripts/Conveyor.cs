using Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Transform nextConveyorCheck;
    [SerializeField] Transform resourceCheck;
    [SerializeField] LayerMask resourceObjectLayer;
    [SerializeField] LayerMask conveyorLayer;
    private GameObject resourceObject;
    private HashSet<MoveResource> moveResource = new();
    private int capacity = 2;
    private int resourceCount;
    private ObjectStats conveyorObjectStats;
    void Start()
    {
        conveyorObjectStats = gameObject.GetComponent<ObjectStats>();
    }

    void Update()
    {
        if (FindNextConveyor() is not null and Collider2D c)
        {
            nextConveyorCheck = FindNextConveyor().transform;
        }
        if (nextConveyorCheck == null || nextConveyorCheck.transform.TryGetComponent(out ObjectStats objectStats) && objectStats.acceptingResources == false)
        {
            return;
        }
        // ill just. figure this out later.
        // WELL THEN NOW WHAT??
        foreach (var item in moveResource)
        {
            if (item == null)
            {
                continue;
            }
            item.previousConveyor = gameObject.transform;
            item.usingConveyorLogic = true;
            item.MoveToNextConveyor(nextConveyorCheck, movementSpeed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != null && collision.transform.TryGetComponent(out MoveResource output))
        {
            resourceCount++;
            if (resourceCount >= capacity)
            {
                conveyorObjectStats.acceptingResources = false;
            }
            moveResource.Add(output);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform != null && collision.transform.TryGetComponent(out MoveResource output))
        {
            moveResource.Remove(output);
            resourceCount--;
            conveyorObjectStats.acceptingResources = true;
        }
    }
    private Collider2D FindNextConveyor()
    {
        if (nextConveyorCheck != null)
        {
            return Physics2D.OverlapCircle(nextConveyorCheck.position, 0.05f, conveyorLayer);
        }
        nextConveyorCheck = null;
        return null;
    }
}
