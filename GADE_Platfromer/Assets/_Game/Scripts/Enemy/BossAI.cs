using UnityEngine;
using System.Collections.Generic;

public class BossAI : MonoBehaviour
{
    public Waypoint startingWaypoint;
    public Animator animator;

    private UnityEngine.AI.NavMeshAgent agent;
    private CustomGraph<Transform> patrolGraph;
    private GraphNode<Transform> currentNode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        BuildGraph();

        // Fallback safety check: if startingWaypoint wasn't set, default to the first node created
        if (currentNode == null && patrolGraph.Nodes.Count > 0)
        {
            currentNode = patrolGraph.Nodes[0];
        }

        if (currentNode != null)
        {
            // Snap to NavMesh to avoid the "floating" bug we had in Part 2
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(transform.position, out hit, 2.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }

            // Head to the very first node!
            agent.SetDestination(currentNode.Data.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh)
        {
            return;
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                GoToNextWaypoint();
            }
        }
    }

    void BuildGraph()
    {
        patrolGraph = new CustomGraph<Transform>();
        Waypoint[] allWaypoints = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
        Dictionary<Waypoint, GraphNode<Transform>> dictionary = new Dictionary<Waypoint, GraphNode<Transform>>();

        foreach (Waypoint wp in allWaypoints)
        {
            GraphNode<Transform> node = patrolGraph.AddNode(wp.transform);
            dictionary.Add(wp, node);

            if (wp == startingWaypoint) currentNode = node;
        }

        // Build the connecting edges based on the Waypoint's nextWaypoints list
        foreach (Waypoint wp in allWaypoints)
        {
            foreach (Waypoint next in wp.nextWaypoints)
            {
                if (dictionary.ContainsKey(wp) && dictionary.ContainsKey(next))
                {
                    patrolGraph.AddDirectedEdge(dictionary[wp], dictionary[next]);
                }
            }
        }
    }

    void GoToNextWaypoint()
    {
        if (currentNode == null || currentNode.Neighbours.Count == 0) return;


        GraphNode<Transform> nextNode = currentNode.GetRandomNeighbour();

        if (nextNode != null)
        {
            agent.SetDestination(nextNode.Data.position);
            currentNode = nextNode;
        }
    }
}