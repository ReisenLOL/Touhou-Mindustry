using Core.Extensions;
using UnityEditor;
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
    [SerializeField] float damageDealt;
    [SerializeField] float range;
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
                GameObject newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation); // this is stupid WOW THAT WORKED?!
                newProjectile.transform.parent = transform;
                Projectile projectileStats = newProjectile.GetComponent<Projectile>();
                projectileStats.firedFrom = gameObject;
                projectileStats.damage = damageDealt;
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane + 10;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                projectileStats.RotateToTarget(worldPos);
                projectileStats.isEnemyBullet = false;
                projectileStats.maxRange = range;
            }
        }
        if (playerCam.orthographicSize > 1f || playerCam.orthographicSize > 0f && Input.mouseScrollDelta.y < 0)
        {
            playerCam.orthographicSize -= Input.mouseScrollDelta.y / 2;
        }
        else if (playerCam.orthographicSize < 0f)
        {
            playerCam.orthographicSize = 5f;
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
