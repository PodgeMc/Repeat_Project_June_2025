using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player; // drag in, or leave empty to auto-find Player tag

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float waypointTolerance = 0.4f;

    [Header("Chase")]
    public float detectionRadius = 12f;   // start chasing when within this range
    public float chaseMemory = 10f;       // keep chasing this long after losing range

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
    }

    void Start()
    {
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

        if (chasing && player)
            Chase(inRange);
        else
            Patrol();
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
