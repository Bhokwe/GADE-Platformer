using UnityEngine;


public class OwnQueue<T>
{
    
    private class QueueNode
    {
        public T data;
        public QueueNode Next;

        public QueueNode(T data)
        {
            this.data = data;
            Next = null;
        }
    }

    
    private QueueNode head;
    private QueueNode tail;

    
    public void Enqueue(T item)
    {
        QueueNode newNode = new QueueNode(item);
        if (tail == null)
        {
            head = tail = newNode;
        }
        else
        {
            tail.Next = newNode;
            tail = newNode;
        }
    }

    
    public T Dequeue()
    {
        if (head == null) return default(T);

        T data = head.data;
        head = head.Next;

        if (head == null)
        { 
            tail = null; 
        }
        return data;
    }
     

    public bool IsEmpty() // Line or Queue checker
    {
        return head == null; //nothing returns and queue ends.
    }

    
}

