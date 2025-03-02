using Core.Extensions;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Transform nextConveyorCheck;
    [SerializeField] Transform resourceCheck;
    [SerializeField] LayerMask resourceObjectLayer;
    [SerializeField] LayerMask conveyorLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D resourceObject = CheckForResources();
        Collider2D nextConveyor = FindNextConveyor();
        if (resourceObject != null && FindNextConveyor() != null && resourceObject.gameObject.CompareTag("ResourceObject") && (nextConveyor.gameObject.GetComponent<Conveyor>() || nextConveyor.gameObject.GetComponent<CoreController>()))
        {
            resourceObject.gameObject.GetComponent<Rigidbody2D>().linearVelocity = ((nextConveyor.gameObject.transform.position - transform.position) * movementSpeed);
            Debug.Log(nextConveyor.gameObject.transform.position - transform.position);
        }
        else if (resourceObject != null && nextConveyor == null)
        {
            resourceObject.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        }
    }
    private Collider2D CheckForResources()
    {
        return Physics2D.OverlapCircle(resourceCheck.position, 0.05f, resourceObjectLayer);
    }
    private Collider2D FindNextConveyor()
    {
        return Physics2D.OverlapCircle(nextConveyorCheck.position, 0.05f, conveyorLayer);
    }
}
