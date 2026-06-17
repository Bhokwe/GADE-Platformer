using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypointObjects;

    private OwnLinkedList patrolPath;
    private Node currentNode;
    private NavMeshAgent agent;
    private bool isInitialized;

    void Start()
    {
        InitializePatrol();
    }

    public void InitializePatrol()
    {
        if (isInitialized)
        {
            ResumePatrol();
            return;
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("EnemyPatrol requires a NavMeshAgent on " + gameObject.name);
            return;
        }

        if (waypointObjects == null || waypointObjects.Length == 0 || waypointObjects[0] == null)
        {
            ConcreteEnemyFactory factory = FindFirstObjectByType<ConcreteEnemyFactory>();
            if (factory != null)
            {
                waypointObjects = factory.waypointObjects;
            }
        }

        if (waypointObjects == null || waypointObjects.Length == 0)
        {
            Debug.LogError("No waypoints assigned to EnemyPatrol on " + gameObject.name);
            return;
        }

        patrolPath = new OwnLinkedList();

        foreach (Transform wp in waypointObjects)
        {
            if (wp != null)
            {
                patrolPath.Add(wp);
            }
        }

        currentNode = patrolPath.head;
        isInitialized = true;
        ResumePatrol();
    }

    private void ResumePatrol()
    {
        if (agent == null || currentNode == null)
        {
            return;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }

        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.SetDestination(currentNode.waypoint.position);
        }
        else
        {
            Debug.LogWarning("EnemyPatrol agent is not on NavMesh: " + gameObject.name);
        }
    }

    void Update()
    {
        if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh)
        {
            return;
        }

        if (currentNode == null)
        {
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                MoveToNextWaypoint();
            }
        }
    }

    void MoveToNextWaypoint()
    {
        currentNode = currentNode.next;

        if (currentNode == null)
        {
            currentNode = patrolPath.head;
        }

        if (currentNode != null && agent.isOnNavMesh)
        {
            agent.SetDestination(currentNode.waypoint.position);
        }
    }
}
