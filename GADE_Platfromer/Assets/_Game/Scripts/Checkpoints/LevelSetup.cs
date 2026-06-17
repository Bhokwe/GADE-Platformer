using UnityEngine;

[DefaultExecutionOrder(-100)]
public class LevelSetup : MonoBehaviour
{
    [Header("Level Configuration")]
    public Transform playerStartLocation;
    public GameObject scenePlayer;
    public int keysRequired = 0;

    [Header("Direct Play (editor / level select)")]
    [Tooltip("When this scene is opened on its own, reset lives and score instead of loading saved progress.")]
    public bool startFreshWhenOpenedDirectly = false;

    private bool createdGameManagerThisScene;

    void Awake()
    {
        if (GameManager.instance == null)
        {
            new GameObject("GameManager").AddComponent<GameManager>();
            createdGameManagerThisScene = true;
        }

        ApplyLevelSetup();
    }

    public void ApplyLevelSetup()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("LevelSetup could not find or create a GameManager in " + gameObject.scene.name);
            return;
        }

        if (scenePlayer == null || playerStartLocation == null)
        {
            Debug.LogError("LevelSetup on " + gameObject.scene.name +
                           " is missing Scene Player or Player Start Location.");
            return;
        }

        if (createdGameManagerThisScene && startFreshWhenOpenedDirectly)
        {
            GameManager.instance.StartNewGame();
        }

        GameManager.instance.keysRequired = keysRequired;
        GameManager.instance.PrepareLevel(scenePlayer, playerStartLocation.position);

        Debug.Log("Level setup complete for " + gameObject.scene.name + ". Player: " + scenePlayer.name +
                  ", Lives: " + GameManager.instance.currentLives +
                  ", Score: " + GameManager.instance.currentScore);
    }
}
