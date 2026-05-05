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

        //is the array empty OR full of nulls, fetch them from the Factory
        if (waypointObjects == null || waypointObjects.Length == 0 || waypointObjects[0] == null)
        {
            // Find the Factory in the current scene
            ConcreteEnemyFactory factory = FindFirstObjectByType<ConcreteEnemyFactory>();

            if (factory != null)
            {
                // Grab the waypoints from the factory
                waypointObjects = factory.waypointObjects;
            }
        }

        // Final safety check to prevent crashes
        if (waypointObjects == null || waypointObjects.Length == 0)
        {
            Debug.LogError("No waypoints assigned to the EnemyPatrol script: " + gameObject.name);
            return;
        }

        patrolPath = new OwnLinkedList();

        foreach (Transform wp in waypointObjects)
        {
            // Only add valid waypoints to prevent null references later
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

    private void OnTriggerEnter(Collider other)
    {
        //Need to check the currentNode is not null
        if(currentNode!=null && other.transform == currentNode.waypoint)
        {
            currentNode = currentNode.next;

            if (currentNode != null)
            {
                agent.SetDestination(currentNode.waypoint.position);

            }
        }

        
        //infinite loop maybe?
        //if (other.transform == currentNode.waypoint)
        //{
        //    currentNode = currentNode.next;
        //    agent.SetDestination(currentNode.waypoint.position);
        //}
    }
}
