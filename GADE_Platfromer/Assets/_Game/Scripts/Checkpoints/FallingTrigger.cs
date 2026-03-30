using UnityEngine;

public class PitfallTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // is the player cooked?
        if (other.CompareTag("Player"))
        {
            // trigger the respawn process using peek state from the GameManager
            GameManager.instance.RespawnPlayer();
        }
    }
}