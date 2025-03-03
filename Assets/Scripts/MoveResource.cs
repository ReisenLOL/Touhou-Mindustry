using Core.Extensions;
using UnityEngine;

public class MoveResource : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool movedLastFrame = false;
    public Transform previousConveyor;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!movedLastFrame)
        {
            rb.VelocityTowards(Vector2.zero, 100f);
        }
        movedLastFrame = false;
    }
    public void MoveToNextConveyor(Transform nextConveyor, float speed)
    {
        rb.VelocityTowards((nextConveyor.gameObject.transform.position - transform.position).normalized * speed, 100f);
        movedLastFrame = true;
    }
}
