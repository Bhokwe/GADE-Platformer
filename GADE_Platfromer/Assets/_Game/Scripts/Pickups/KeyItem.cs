using UnityEngine;

public class KeyItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // adding the key to the player 
            GameManager.instance.keysCollected++;
            Debug.Log("Key collected! Total: " + GameManager.instance.keysCollected);

            // Destroy the physical key object
            Destroy(gameObject);
        }
    }
}