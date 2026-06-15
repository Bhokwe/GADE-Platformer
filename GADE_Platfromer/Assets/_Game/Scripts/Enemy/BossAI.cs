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

        if (startingWaypoint != null)
        {
            //agent.Warp(startingWaypoint.transform.position);
        }

        BuildGraph();
        GoToNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)
        { 
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoToNextWaypoint();
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

        int randomIndex = Random.Range(0, currentNode.Neighbours.Count);
        GraphNode<Transform> nextNode = currentNode.Neighbours[randomIndex];

        agent.SetDestination(nextNode.Data.position);
        currentNode = nextNode;
    }
}
