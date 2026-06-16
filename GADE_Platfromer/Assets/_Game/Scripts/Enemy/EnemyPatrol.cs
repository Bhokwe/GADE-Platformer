using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypointObjects;
    private OwnLinkedList patrolPath;
    private Node currentNode;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Fetch waypoints from the Factory if empty
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
            Debug.LogError("No waypoints assigned to the EnemyPatrol script: " + gameObject.name);
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

        if (currentNode != null)
        {
            agent.SetDestination(currentNode.waypoint.position);
        }
    }

    void Update()
    {
        // 1. Check if we actually have a node we are moving towards
        if (currentNode != null)
        {
            // 2. Check if the agent is close to its destination (and not still calculating a path)
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                // 3. Make sure the agent has actually stopped moving
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    MoveToNextWaypoint();
                }
            }
        }
    }

    void MoveToNextWaypoint()
    {
        // Move to the next node in the linked list
        currentNode = currentNode.next;

        // Loop back to the start if we reach the end of the list. 
        // This guarantees they complete "multiple laps" 
        if (currentNode == null)
        {
            currentNode = patrolPath.head;
        }

        // Set the new destination
        if (currentNode != null)
        {
            agent.SetDestination(currentNode.waypoint.position);
        }
    }
}