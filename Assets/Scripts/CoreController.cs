using UnityEngine;

public class CoreController : MonoBehaviour
{
    [SerializeField] LayerMask resourceLayer;
    [SerializeField] Transform resourceCheck;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindResources())
        {

        }
    }
    private Collider2D FindResources()
    {
        return Physics2D.OverlapBox(this.resourceCheck.position, new Vector2(3,3), resourceLayer);
    }
}
