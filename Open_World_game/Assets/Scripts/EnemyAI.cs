using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public Animator anim;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float waypointTolerance = 0.4f;

    [Header("Chase")]
    public float detectionRadius = 12f;
    public float chaseMemory = 10f;

    [Header("Speeds")]
    public float patrolSpeed = 1.6f;
    public float chaseSpeed = 3.5f;

    [Header("Damage")]
    public int contactDamage = 10;
    public float hitCooldown = 1.5f;
    float nextHitTime = 0f;

    int patrolIndex = -1;
    float lastSeenTime = -999f;
    Vector3 lastSeenPos;

    void Awake()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
        if (!anim) anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        if (agent)
        {
            agent.speed = patrolSpeed;
            agent.stoppingDistance = 1.2f;
        }
        NextPatrolPoint();
    }

    void Update()
    {
        if (!agent) return;

        bool inRange = false;
        if (player)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            inRange = dist <= detectionRadius;
            if (inRange)
            {
                lastSeenTime = Time.time;
                lastSeenPos = player.position;
            }
        }

        bool chasing = inRange || (Time.time - lastSeenTime <= chaseMemory);

        if (chasing && player) Chase(inRange);
        else Patrol();

        agent.speed = chasing ? chaseSpeed : patrolSpeed;

        if (anim)
        {
            anim.SetBool("Running", chasing);
            anim.SetBool("Walking", !chasing);
        }
    }

    void Chase(bool seePlayer)
    {
        Vector3 target = seePlayer ? player.position : lastSeenPos;
        agent.isStopped = false;
        agent.SetDestination(target);
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        agent.isStopped = false;

        if (!agent.hasPath || agent.remainingDistance <= Mathf.Max(waypointTolerance, agent.stoppingDistance))
            NextPatrolPoint();
    }

    void NextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[patrolIndex].position);
    }

    void OnCollisionStay(Collision collision)
    {
        TryHit(collision.collider);
    }

    void OnTriggerStay(Collider other)
    {
        TryHit(other);
    }

    void TryHit(Collider col)
    {
        if (Time.time < nextHitTime) return;
        if (!col.CompareTag("Player")) return;

        var pm = col.GetComponent<PlayerManager>();
        if (pm == null) return;

        pm.TakeDamage(contactDamage);
        nextHitTime = Time.time + hitCooldown;
    }
}
