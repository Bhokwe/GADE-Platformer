using System.Collections.Generic;
using UnityEngine;

public class CheckpointStackADT
{

    //the list that will store the checkpoint data
    private List<CheckpointData> stackMemory = new List<CheckpointData>();

    public void Push(CheckpointData newCheckpoint)
    {
        stackMemory.Add(newCheckpoint);
        Debug.Log("State Saved! You are currently at Checkpoint Depth: " + stackMemory.Count);
    }
    public CheckpointData Peek() 
    {
        if (isEmpty())
        {
            Debug.LogWarning("Stack is empty brudda, I can't peek");
            return null;
        }
        return stackMemory[stackMemory.Count - 1];

    }

    //remove and return the top item for checkpoint data stack
    public CheckpointData Pop() 
    {
        if (isEmpty())
        {
            Debug.LogWarning("Stack has nuhting brudda, I can't pop");
            return null;
        }

        //getting the top item, remove it from the stack then return it
        int topIndex = stackMemory.Count - 1;
        CheckpointData topCheckpoint = stackMemory[topIndex];
        stackMemory.RemoveAt(topIndex);

        return topCheckpoint;

    }

    //the function that checks if the stack has anything in it
    public bool isEmpty() 
    {
        return stackMemory.Count == 0;
    }

  
}
