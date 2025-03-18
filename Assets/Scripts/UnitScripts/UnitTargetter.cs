using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitTargetter : MonoBehaviour
{
    private GameObject core;
    private GameObject player;
    private Rigidbody2D rb;
    private Vector3 lookDirection;
    private Collider2D[] targetList;
    private GameObject closestTarget;
    private UnitController unitController;
    private UnitStats unitStats;
    private AStarPathfinder pathfinder;
    private List<Node> path;
    private int pathIndex = 0;
    public float range;
    [SerializeField] LayerMask unitLayer;
    [SerializeField] LayerMask playerLayer;
    private bool isEnemy;
    void Start()
    {
        core = GameObject.Find("Core");
        pathfinder = GameObject.Find("GameManager").GetComponent<AStarPathfinder>();
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        unitController = GetComponent<UnitController>();
        unitStats = GetComponent<UnitStats>();
        isEnemy = unitStats.isEnemy;
        closestTarget = this.gameObject;
        path = pathfinder.FindPath(transform.position, core.transform.position);
        if (path == null)
        {
            Debug.LogError("Enemy path is NULL at Start!");
        }
        else
        {
            Debug.Log("Enemy path found with " + path.Count + " nodes.");
        }
    }

    void Update()
    {
        if (isEnemy)
        {
            path = pathfinder.FindPath(transform.position, core.transform.position);
            if (path == null || pathIndex >= path.Count)
            {
                return;
            }
            if (path == null || path.Count == 0)
            {
                Debug.LogWarning("Path is NULL or empty! Enemy has nowhere to go.");
                return;
            }

            if (pathIndex >= path.Count)
            {
                Debug.Log("Enemy reached end of path.");
                return;
            }
            Vector3 targetPos = path[pathIndex].worldPosition;
            lookDirection = targetPos;

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                pathIndex++;
                Debug.Log("Enemy moving to next waypoint: " + pathIndex);
            }
        }
        else
        {
            lookDirection = (closestTarget.transform.position - transform.position).normalized;
            unitController.lookDirection = lookDirection;
        }
        unitController.lookDirection = lookDirection;
        targetList = DetectTargets();
        if (targetList.Length == 0 || closestTarget == null)
        {
            closestTarget = this.gameObject;
            unitController.closestTarget = closestTarget;
        }
        float distanceToClosestTarget = 1000000f;
        Collider2D iteration = null;
        for (int i = 0; i < targetList.Length; i++)
        {
            iteration = targetList[i];
            if (iteration == null || iteration.gameObject.TryGetComponent(out UnitStats stats) && isEnemy == stats.isEnemy)
            {
                continue;
            }
            float sqrDistance = Vector3.SqrMagnitude(transform.position - iteration.transform.position);
            if (sqrDistance < distanceToClosestTarget && iteration.gameObject != gameObject)
            {
                closestTarget = iteration.gameObject;
                unitController.closestTarget = closestTarget;
                distanceToClosestTarget = sqrDistance;
            }
        }
    }
    private Collider2D[] DetectTargets()
    {
        if (isEnemy)
        {
            return Physics2D.OverlapCircleAll(transform.position, range, ~unitLayer);
        }
        return Physics2D.OverlapCircleAll(transform.position, range, playerLayer);
    }
}
