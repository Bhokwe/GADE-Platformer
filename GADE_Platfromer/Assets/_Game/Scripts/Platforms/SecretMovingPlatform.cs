using UnityEngine;

public class SecretMovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform targetWaypoint; // Where the platform should move to
    public float moveSpeed = 3f;

    private bool isActivated = false;
    private Vector3 destinationPosition;

    // This is called by the TriggerButton script
    public void ActivatePlatform()
    {
        isActivated = true;
        Debug.Log("✅ " + gameObject.name + " received the activation signal!");

        if (targetWaypoint != null)
        {
            // Lock in the destination coordinates. 
            // This fixes the "carrot on a stick" bug if the waypoint was accidentally made a child!
            destinationPosition = targetWaypoint.position;

            // Detach the waypoint from the platform just to be absolutely safe
            targetWaypoint.SetParent(null);
        }
        else
        {
            Debug.LogError("❌ " + gameObject.name + " wants to move, but its Target Waypoint is MISSING in the Inspector!");
        }
    }

    void Update()
    {
        // Only move if the trigger has been pressed
        if (isActivated && targetWaypoint != null)
        {
            // Move steadily towards the locked destination position
            transform.position = Vector3.MoveTowards(transform.position, destinationPosition, moveSpeed * Time.deltaTime);

            // Optional: Stop moving exactly when it reaches the target to save performance
            if (Vector3.Distance(transform.position, destinationPosition) < 0.01f)
            {
                isActivated = false;
                Debug.Log("🎯 " + gameObject.name + " has arrived at its destination!");
            }
        }
    }

    // Changed from OnTrigger to OnCollision so the platform can be SOLID!
    // IMPORTANT: Make sure "Is Trigger" is UNCHECKED on the platform's collider in Unity!
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}