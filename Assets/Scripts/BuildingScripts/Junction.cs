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
                Debug.Log("here1");
                if (FindNextConveyor(i) is not null and Collider2D c)
                {
                    Debug.Log("here2");
                    if (item.previousConveyor.transform == FindNextConveyor(i).transform)
                    {
                        Debug.Log("here3");
                        int nextIndex = i + 2;
                        if (nextIndex < conveyorChecks.Length)
                        {
                            nextIndex = 3 - i;
                        }
                        Debug.Log(nextIndex);
                        if (FindNextConveyor(nextIndex) is not null and Collider2D c2)
                        {
                            item.MoveToNextConveyor(FindNextConveyor(nextIndex).gameObject.transform, movementSpeed);
                            item.previousConveyor = gameObject.transform;
                            item.usingConveyorLogic = false;
                        }
                        //warcrime time
                        //its half hardcoded so it is a warcrime!
                        //...ill figure this out tomorrow
                    }
                }
                if (nextConveyor == null || nextConveyor.transform.TryGetComponent(out ObjectStats objectStats) && objectStats.acceptingResources == false)
                {
                    Debug.Log("here4");
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
