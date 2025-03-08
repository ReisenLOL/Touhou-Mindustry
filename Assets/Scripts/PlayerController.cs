using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float fireRate;
    public float health = 100;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [SerializeField] GameObject projectile;
    private Vector3 mousePos;
    // i gotta find a better name, something fitting for the core
    // i have an idea - what if there were multiple types of cores with different stuff
    // if i use that idea, then i guess ill use the word core
    // i remember get and set, i learnt it back when i used godot
    // eh ill figure it out later
    private GameObject core;
    public string coreType;
    private float fireRateTime;
    public bool canShoot = true;
    private Camera playerCam;
    private bool isFacingRight = true;
    private SpriteRenderer playerSpriteRenderer;
    void Start()
    {
        playerSpriteRenderer = base.GetComponent<SpriteRenderer>();
        playerCam = GameObject.Find("Camera").GetComponent<Camera>();
        core = GameObject.Find(coreType);
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        mousePos = Input.mousePosition;
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        fireRateTime += Time.deltaTime;
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && canShoot && !EventSystem.current.IsPointerOverGameObject())
        {
            if (fireRateTime >= fireRate)
            {
                fireRateTime = 0;
                Instantiate(projectile, transform.position, projectile.transform.rotation);
            }
        }
        if (playerCam.orthographicSize > 1f || playerCam.orthographicSize > 0f && Input.mouseScrollDelta.y < 0)
        {
            playerCam.orthographicSize -= Input.mouseScrollDelta.y / 2;
        }
        if (this.moveInput.x > 0f && this.isFacingRight)
        {
            playerSpriteRenderer.flipX = true;
            this.isFacingRight = !this.isFacingRight;
        }
        else if (this.moveInput.x < 0f && !this.isFacingRight)
        {
            playerSpriteRenderer.flipX = false;
            this.isFacingRight = !this.isFacingRight;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }
}
