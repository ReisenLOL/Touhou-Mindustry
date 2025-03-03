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
        Debug.Log(col);
        if (col != null && col != this && col.transform != null && col.transform.TryGetComponent(out c))
        {
            nextConveyorCheck = c.transform;
            foreach (var item in moveResource)
            {
                if (item == null)
                {
                    continue;
                }
                item.MoveToNextConveyor(nextConveyorCheck, movementSpeed);
            }
            Debug.Log(moveResource.Count);
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
        return Physics2D.OverlapCircle(this.conveyorChecks[index].position, 5f, conveyorLayer);
    }
}