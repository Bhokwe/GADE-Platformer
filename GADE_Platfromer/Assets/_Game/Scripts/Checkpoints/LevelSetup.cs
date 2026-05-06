using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    [Header("Level Configuration")]
    public Transform playerStartLocation;

    //We need to grab the Player that lives in thiw specific scene
    public GameObject scenePlayer;

    void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.player = scenePlayer;

            GameManager.instance.ResetCheckpointsForNewLevel(playerStartLocation.position);

            Debug.Log("Level 2 Setup Complete! GameManager has the new player.");
        }
    }
}