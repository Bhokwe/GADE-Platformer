using UnityEngine;

public class TriggerButton : MonoBehaviour
{
    [Header("The Platform to Activate")]
    public SecretMovingPlatform targetPlatform;

    [Header("Visual Feedback (Optional)")]
    public Material activatedMaterial;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        //ONLY the hero can activate it
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            Debug.Log("Trigger Activated by Player! Secret platform is moving.");

            // Tell the linked platform to start moving
            if (targetPlatform != null)
            {
                targetPlatform.ActivatePlatform();
            }
            else
            {
                Debug.LogWarning("Trigger pressed, but no target platform is assigned!");
            }

            // Change color to show it was pressed
            Renderer rend = GetComponent<Renderer>();
            if (rend != null && activatedMaterial != null)
            {
                rend.material = activatedMaterial;
            }
        }
    }
}