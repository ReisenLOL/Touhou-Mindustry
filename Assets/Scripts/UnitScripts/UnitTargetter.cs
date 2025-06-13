using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitTargetter : MonoBehaviour
{
    private GameObject core;
    private GameObject player;
    private Rigidbody2D rb;
    private Vector3 lookDirection;
    public List<Collider2D> targetList = new();
    private GameObject closestTarget;
    private UnitController unitController;
    private UnitStats unitStats;
    public bool isEnemy;
    NavMeshAgent agent;
    void Start()
    {
        core = FindFirstObjectByType<CoreController>().GameObject();
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        unitController = GetComponent<UnitController>();
        unitStats = GetComponent<UnitStats>();
        isEnemy = unitStats.isEnemy;
        closestTarget = this.gameObject;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = unitStats.unitType.speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        GameObject detectTargetsOnTriggerEnter = new GameObject("DetectTargetsOnTriggerEnter");
        detectTargetsOnTriggerEnter.transform.parent = transform;
        detectTargetsOnTriggerEnter.transform.localPosition = Vector3.zero;
        detectTargetsOnTriggerEnter.layer = LayerMask.NameToLayer("DetectTargets");
        CircleCollider2D detectTargetsCC2D = detectTargetsOnTriggerEnter.AddComponent<CircleCollider2D>();
        detectTargetsCC2D.isTrigger = true;
        detectTargetsCC2D.radius = unitStats.unitType.range;
        detectTargetsOnTriggerEnter.AddComponent<UnitOnTriggerTarget>();
    }

    void Update()
    {
        if (isEnemy)
        {
            agent.SetDestination(core.transform.position);
            /*if (player == null)
            {
                player = FindFirstObjectByType<PlayerController>().gameObject;
            }
            float distanceToPlayer = Vector3.SqrMagnitude(player.transform.position - transform.position);
            float distanceToCore = Vector3.SqrMagnitude(core.transform.position - transform.position);
            if (distanceToPlayer < distanceToCore)
            {
                lookDirection = (player.transform.position - transform.position).normalized;
                unitController.lookDirection = lookDirection;
            }
            else
            {
                lookDirection = (core.transform.position - transform.position).normalized;
                unitController.lookDirection = lookDirection;
            }*/
        }
        else if (closestTarget != null)
        {
            lookDirection = (closestTarget.transform.position - transform.position).normalized;
            unitController.lookDirection = lookDirection;
        }
        else
        {
            Debug.LogWarning("HOW"); //uh i think thats fixed
        }
        if (targetList.Count == 0 || closestTarget == null)
        {
            closestTarget = null;
            unitController.closestTarget = closestTarget;
        }
        float distanceToClosestTarget = 1000000f;
        foreach (Collider2D target in targetList)
        {
            if (target == null || target.gameObject.TryGetComponent(out UnitStats stats) && isEnemy == stats.isEnemy)
            {
                targetList.Remove(target);
                continue;
            }
            float sqrDistance = Vector3.SqrMagnitude(transform.position - target.transform.position);
            if (sqrDistance < distanceToClosestTarget && target.gameObject != gameObject)
            {
                closestTarget = target.gameObject;
                unitController.closestTarget = closestTarget;
                distanceToClosestTarget = sqrDistance;
            }
        }
    }
}