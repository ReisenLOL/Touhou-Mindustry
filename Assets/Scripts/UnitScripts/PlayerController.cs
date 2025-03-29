using Core.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private float speed;
    private float fireRate;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private GameObject projectile;
    private float damageDealt;
    private float range;
    [SerializeField] GameManager gameManager;
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
    private GameObject newPlayer;
    public bool hasFired = false;
    private UnitType unitType;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        newPlayer = gameManager.newPlayer;
        playerCamera = GameObject.Find("Camera");
        playerSpriteRenderer = base.GetComponent<SpriteRenderer>();
        playerCam = GameObject.Find("Camera").GetComponent<Camera>();
        core = GameObject.Find(coreType);
        rb = GetComponent<Rigidbody2D>();
        unitType = GetComponent<UnitStats>().unitType;
        damageDealt = unitType.damageDealt;
        projectile = unitType.projectile;
        range = unitType.range;
        fireRate = unitType.fireRate;
        speed = unitType.speed;
    }
    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        if (hasFired)
        {
            fireRateTime += Time.deltaTime;
            //wait i think this already works? because if the player has already fired then theres no need to set it back to false as it will always be under? I DONT KNOW????
        }
        else
        {
            fireRateTime += fireRate;
        }
        if (Input.GetMouseButton(0) && canShoot && !EventSystem.current.IsPointerOverGameObject())
        {
            if (fireRateTime >= fireRate)
            {
                fireRateTime = 0;
                GameObject newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation); // this is stupid WOW THAT WORKED?!
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
                    newPlayerController.coreType = coreType;
                    unitToSwitchTo.tag = "Player";
                    unitToSwitchTo.layer = (LayerMask.NameToLayer("Player")); //player layer can walk on walls, if ground unit then this shouldnt be possible
                    gameManager.player = unitToSwitchTo;
                    gameManager.playerController = newPlayerController;
                    Destroy(unitController);
                    Destroy(unitToSwitchTo.GetComponent<UnitTargetter>());
                    Destroy(gameObject);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            GameObject RespawnPlayer = Instantiate(newPlayer);
            gameManager.player = RespawnPlayer;
            gameManager.playerController = RespawnPlayer.GetComponent<PlayerController>();
            RespawnPlayer.transform.position = core.transform.position;
            Destroy(RespawnPlayer.transform.Find("Camera").gameObject);
            playerCamera.transform.parent = RespawnPlayer.transform;
            playerCamera.transform.position = RespawnPlayer.transform.position + new Vector3(0, 0, -10);
            Destroy(gameObject);
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
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }
}
