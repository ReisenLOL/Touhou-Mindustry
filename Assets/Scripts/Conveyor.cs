using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] float speed = 4;
    [SerializeField] Transform nextConveyorCheck;
    [SerializeField] Transform resourceCheck;
    private float _time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        Collider2D resourceObject = CheckForResources();
        Collider2D nextConveyor = FindNextConveyor();
        Debug.Log(resourceObject);
        Debug.Log(nextConveyor);
        if (resourceObject != null && FindNextConveyor() != null && resourceObject.gameObject.CompareTag("ResourceObject") && (nextConveyor.gameObject.GetComponent<Conveyor>() || nextConveyor.gameObject.GetComponent<CoreController>()) && _time >= speed)
        {
            _time = 0;
            if (nextConveyor.gameObject.GetComponent<Conveyor>())
            {
                resourceObject.gameObject.transform.position = nextConveyor.gameObject.transform.position + new Vector3(0, 0, -1);
            }
            else if (nextConveyor.GetComponent<CoreController>())
            {
                Destroy(resourceObject.gameObject);
            }
        }
    }
    private Collider2D CheckForResources()
    {
        return Physics2D.OverlapCircle(resourceCheck.position, 0.05f);
    }
    private Collider2D FindNextConveyor()
    {
        return Physics2D.OverlapCircle(nextConveyorCheck.position, 0.05f);
    }
}
