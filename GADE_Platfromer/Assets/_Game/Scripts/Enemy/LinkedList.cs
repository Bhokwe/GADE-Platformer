using UnityEngine;


public class Node
{
    public Transform waypoint;
    public Node next;

    public Node(Transform wp)
    {
        waypoint = wp;
        next = null;
    }
}
public class OwnLinkedList
{
    public Node head;
    public Node tail;

    public void Add(Transform newWayPoint)
    { 
        Node newNode = new Node(newWayPoint);

        if (head == null)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            tail.next = newNode;
            tail = newNode;
        }

        tail.next = head;
    
    }



}
