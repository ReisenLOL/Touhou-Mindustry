using Core.Extensions;
using UnityEngine;

public class MoveResource : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool movedLastFrame = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!movedLastFrame)
        {
            rb.VelocityTowards(Vector2.zero, 10f);
        }
        movedLastFrame = false;
    }
    public void MoveToNextConveyor(Transform nextConveyor, float speed)
    {
        rb.VelocityTowards((nextConveyor.gameObject.transform.position - transform.position).normalized * speed, 10f);
        movedLastFrame = true;
    }
}
