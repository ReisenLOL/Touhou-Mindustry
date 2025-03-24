using Core.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float fireRate;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public GameObject projectile;
    public float damageDealt;
    public float range;
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
    private GameObject playerCamera;
    public GameObject newPlayer;
    public bool hasFired = false;
    void Start()
    {
        playerCamera = GameObject.Find("Camera");
        playerSpriteRenderer = base.GetComponent<SpriteRenderer>();
        playerCam = GameObject.Find("Camera").GetComponent<Camera>();
        core = GameObject.Find(coreType);
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        if (hasFired)
        {
            fireRateTime += Time.deltaTime;
            //this should not be this confusing....
            //guess we'll just pause for now!
        }
        if (Input.GetMouseButton(0) && canShoot && !EventSystem.current.IsPointerOverGameObject())
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
                hasFired = true;
            }
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit && hit.transform.gameObject.CompareTag("Unit"))
            {
                GameObject unitToSwitchTo = hit.transform.gameObject;
                UnitStats unitStats = unitToSwitchTo.GetComponent<UnitStats>();
                UnitController unitController = unitToSwitchTo.GetComponent<UnitController>();
                if (!unitStats.isEnemy && !unitStats.isPlayer)
                {
                    unitStats.isPlayer = true;
                    unitToSwitchTo.AddComponent<PlayerController>();
                    playerCamera.transform.parent = unitToSwitchTo.transform;
                    playerCamera.transform.position = unitToSwitchTo.transform.position + new Vector3(0, 0, -10);
                    PlayerController newPlayerController = unitToSwitchTo.GetComponent<PlayerController>();
                    newPlayerController.speed = unitController.speed;
                    newPlayerController.projectile = unitController.projectile;
                    newPlayerController.range = unitController.range;
                    newPlayerController.damageDealt = unitController.damageDealt;
                    newPlayerController.fireRate = unitController.fireRate;
                    newPlayerController.coreType = coreType;
                    newPlayerController.newPlayer = newPlayer;
                    Destroy(unitController);
                    Destroy(unitToSwitchTo.GetComponent<UnitTargetter>());
                    Destroy(gameObject);
                }
            }
        }
        /*if (Input.GetKeyDown(KeyCode.V))
        {
            GameObject RespawnPlayer = Instantiate(newPlayer);
            RespawnPlayer.transform.position = core.transform.position;
            Destroy(gameObject);
        }*/
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
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }
}
