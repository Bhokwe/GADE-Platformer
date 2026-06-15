using System.Collections.Generic;

public class GraphNode <T>
{
    public T Data { get; set; }
    public List<GraphNode<T>> Neighbours { get; set; }

    public GraphNode (T data)
    {
        Data = data;
        Neighbours = new List<GraphNode<T>>();
    }

    public void AddEdge(GraphNode<T> neighbour)
    {
        if (!Neighbours.Contains(neighbour));
        { 
            Neighbours.Add(neighbour);        
        }
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
