using System.Collections.Generic;
using UnityEngine; // Added this so we can use Unity's Random.Range

public class GraphNode<T>
{
    public T Data { get; set; }
    public List<GraphNode<T>> Neighbours { get; set; }

    public GraphNode(T data)
    {
        Data = data;
        Neighbours = new List<GraphNode<T>>();
    }

    public void AddEdge(GraphNode<T> neighbour)
    {
        
        if (!Neighbours.Contains(neighbour))
        {
            Neighbours.Add(neighbour);
        }
    }

    //randomly selects a neighbour from the list of neighbours
    public GraphNode<T> GetRandomNeighbour()
    {
        if (Neighbours.Count == 0) return null;

        int randomIndex = Random.Range(0, Neighbours.Count);
        return Neighbours[randomIndex];
    }
}

public class CustomGraph<T>
{
    public List<GraphNode<T>> Nodes { get; private set; }

    public CustomGraph()
    {
        Nodes = new List<GraphNode<T>>();
    }

    public GraphNode<T> AddNode(T data)
    {
        GraphNode<T> newNode = new GraphNode<T>(data);
        Nodes.Add(newNode);
        return newNode;
    }

    public void AddDirectedEdge(GraphNode<T> fromNode, GraphNode<T> toNode)
    {
        if (fromNode != null && toNode != null)
        {
            fromNode.AddEdge(toNode);
        }
    }
}