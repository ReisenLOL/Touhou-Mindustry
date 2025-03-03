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
            Debug.Log("1");
            conveyor = c;
        }
        if (conveyor == null)
        {
            Debug.Log("2");
            return;
        }
        nextConveyorCheck = conveyor.transform;
        foreach (var item in moveResource)
        {
            Debug.Log("3");
            if (item == null)
            {
                Debug.Log("4");
                continue;
            }
            item.MoveToNextConveyor(nextConveyorCheck, movementSpeed);
        }
        Debug.Log(moveResource.Count);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != null && collision.transform.TryGetComponent(out MoveResource output))
        {
            Debug.Log("5");
            moveResource.Add(output);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform != null && collision.transform.TryGetComponent(out MoveResource output))
        {
            Debug.Log("6");
            moveResource.Remove(output);
        }
    }
    private Collider2D DetectConveyors(int index)
    {
        Debug.Log("7");
        return Physics2D.OverlapCircle(this.conveyorChecks[index].position, 0.05f, conveyorLayer);
    }
}
