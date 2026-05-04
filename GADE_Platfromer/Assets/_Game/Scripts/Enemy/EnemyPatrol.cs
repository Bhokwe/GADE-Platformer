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
        patrolPath = new OwnLinkedList();

        foreach (Transform wp in waypointObjects)
        {
            patrolPath.Add(wp);
            
        }

        currentNode = patrolPath.head;
        agent.SetDestination(currentNode.waypoint.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == currentNode.waypoint)
        {
            currentNode = currentNode.next;
            agent.SetDestination(currentNode.waypoint.position);
        }
    }
}
