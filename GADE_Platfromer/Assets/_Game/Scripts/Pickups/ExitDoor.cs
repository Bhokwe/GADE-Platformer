using UnityEngine;
using UnityEngine.SceneManagement; // You MUST include this to load scenes!

public class ExitDoor : MonoBehaviour
{
    public string nextLevelName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // does this broer have enough keys to open the door?
            if (GameManager.instance.keysCollected >= GameManager.instance.keysRequired)
            {
                Debug.Log("Door Unlocked! Loading next level...");

                // resets the keys 
                GameManager.instance.keysCollected = 0;

                // Load Level 2!
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                Debug.Log("Door Locked! You need more keys.");
                
            }
        }
    }
}