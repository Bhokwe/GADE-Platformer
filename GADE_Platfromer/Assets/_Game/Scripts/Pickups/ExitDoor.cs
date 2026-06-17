using UnityEngine;

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
                if (SFXManager.Instance != null)
                {
                    SFXManager.Instance.PlaySFX("DOOR");
                }

                Debug.Log("Door Unlocked! Loading next level...");
                GameManager.instance.LoadNextLevel(nextLevelName);
            }
            else
            {
                Debug.Log("Door Locked! You need more keys.");

            }
        }
    }
}