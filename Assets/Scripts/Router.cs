using Core.Extensions;
using UnityEngine;

public class Router : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Transform[] conveyorChecks;
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
        Collider2D detectedConveyor = DetectConveyors(Random.Range(0, conveyorChecks.Length));
        Debug.Log(detectedConveyor.name);
        if (resourceObject != null && detectedConveyor != null)
        {
            resourceObject.gameObject.GetComponent<Rigidbody2D>().linearVelocity = ((detectedConveyor.gameObject.transform.position - transform.position) * movementSpeed);
        }
        else if (resourceObject != null && detectedConveyor == null)
        {
            resourceObject.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        }
    }
    private Collider2D CheckForResources()
    {
        return Physics2D.OverlapCircle(resourceCheck.position, 0.05f, resourceObjectLayer);
    }
    private Collider2D DetectConveyors(int index)
    {
        return Physics2D.OverlapCircle(this.conveyorChecks[index].position, 0.05f, conveyorLayer);
    }
}
