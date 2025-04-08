using Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class Junction : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Transform[] conveyorChecks; //YOU ARE A FUCKING DUMBASS SYLVIA wow thats very mean of you, past sylvia
    private Transform nextConveyor;
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
        // ill just. figure this out later.
        // WELL THEN NOW WHAT??
        foreach (var item in moveResource)
        {
            if (item == null)
            {
                continue;
            }
            for (int i = 0; i < conveyorChecks.Length; i++)
            {
                if (item.previousConveyor == FindNextConveyor(i).gameObject.transform)
                {
                    switch (i)
                    {
                        case 0:
                            if (FindNextConveyor(2) is not null and Collider2D c2)
                            {
                                item.MoveToNextConveyor(FindNextConveyor(2).gameObject.transform, movementSpeed);
                                item.previousConveyor = gameObject.transform;
                                item.usingConveyorLogic = false;
                            }
                            break;
                        case 1:
                            if (FindNextConveyor(3) is not null and Collider2D c3)
                            {
                                item.MoveToNextConveyor(FindNextConveyor(3).gameObject.transform, movementSpeed);
                                item.previousConveyor = gameObject.transform;
                                item.usingConveyorLogic = false;
                            }
                            break;
                        case 2:
                            if (FindNextConveyor(0) is not null and Collider2D c4)
                            {
                                item.MoveToNextConveyor(FindNextConveyor(0).gameObject.transform, movementSpeed);
                                item.previousConveyor = gameObject.transform;
                                item.usingConveyorLogic = false;
                            }
                            break;
                        case 3:
                            if (FindNextConveyor(1) is not null and Collider2D c5)
                            {
                                item.MoveToNextConveyor(FindNextConveyor(1).gameObject.transform, movementSpeed);
                                item.previousConveyor = gameObject.transform;
                                item.usingConveyorLogic = false;
                            }
                            break;
                            //i'm cooked i might aswell just start a new project lol
                            //or am i?
                            //today i will give up, tomorrow i will get ALOT done the first thing i do when i wake up, trust me.
                            //you should have never trusted me.
                    }
                }
                if (nextConveyor == null || nextConveyor.transform.TryGetComponent(out ObjectStats objectStats) && objectStats.acceptingResources == false)
                {
                    return;
                }
            }
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
    private Collider2D FindNextConveyor(int index)
    {
        return Physics2D.OverlapCircle(conveyorChecks[index].position, 0.05f, conveyorLayer);
    }
}
