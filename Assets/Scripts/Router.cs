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
        Conveyor c = null;
        Collider2D col = DetectConveyors(randomIndex);
        if (col != null && col.transform != null && col.transform.TryGetComponent(out c))
        {
            if (col.transform.TryGetComponent(out ObjectStats objectStats) && objectStats.acceptingResources)
            {
                nextConveyorCheck = c.transform;
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
        return Physics2D.OverlapCircle(this.conveyorChecks[index].position, 10f, conveyorLayer);
    }
}