using Core.Extensions;
using UnityEngine;

public class MoveResource : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isMoving = false;
    public Transform previousConveyor;
    private Vector2 moveDirection = Vector2.zero;
    private float moveSpeed = 0f;
    public bool usingConveyorLogic;
    private bool movedLastFrame;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isMoving && !usingConveyorLogic || (usingConveyorLogic && movedLastFrame))
        {
            if (moveDirection != null)
            {
                rb.VelocityTowards(moveDirection * moveSpeed, 100f);
            }
            else
            {
                rb.VelocityTowards(Vector2.zero, 100f);
            }
            if (usingConveyorLogic)
            {
                movedLastFrame = false;
            }
        }
        else
        {
            rb.VelocityTowards(Vector2.zero, 100f);
        }
    }

    public void MoveToNextConveyor(Transform nextConveyor, float speed)
    {
        if (nextConveyor == null)
        {
            return;
        }
        moveDirection = (nextConveyor.position - transform.position).normalized;
        moveSpeed = speed;
        isMoving = true;
        if (usingConveyorLogic)
        {
            movedLastFrame = true;
        }
    }
}
