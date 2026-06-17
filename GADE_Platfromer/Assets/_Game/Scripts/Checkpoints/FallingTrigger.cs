using UnityEngine;

public class FallingTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player fell out of bounds!");

            if (GameManager.instance == null)
            {
                Debug.LogError("FallingTrigger fired but GameManager is missing.");
                return;
            }

            GameManager.instance.RespawnPlayer();
        }
    }
}