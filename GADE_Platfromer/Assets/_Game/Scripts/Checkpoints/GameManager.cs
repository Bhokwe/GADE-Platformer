using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Stats")]
    public GameObject player;
    public int currentScore = 0;
    public int currentLives = 3;
    public int maxLives = 3;

    [Header("Death Handling")]
    [SerializeField] private float deathCooldown = 1.5f;

    [Header("Game Over")]
    [SerializeField] private float gameOverDelay = 2f;
    [SerializeField] private bool restartLevelOnGameOver = true;

    private CheckpointStackADT checkpointStack = new CheckpointStackADT();

    public int keysCollected = 0;
    public int keysRequired;

    private static bool hasBootstrapped;
    private bool isProcessingDeath;
    private bool isGameOver;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (!hasBootstrapped)
        {
            currentScore = PlayerPrefs.GetInt("SavedScore", 0);
            currentLives = PlayerPrefs.GetInt("SavedLives", maxLives);
            hasBootstrapped = true;
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isProcessingDeath = false;
        isGameOver = false;
        RefreshLevelBinding();
        PersistStats();
    }

    public bool IsPlayerValid()
    {
        return player != null && player.activeInHierarchy;
    }

    public void EnsurePlayerBound()
    {
        if (IsPlayerValid())
        {
            return;
        }

        GameObject found = GameObject.FindGameObjectWithTag("Player");
        if (found != null)
        {
            player = found;
            Debug.Log("GameManager auto-bound player: " + player.name);
        }
    }

    public void RefreshLevelBinding()
    {
        LevelSetup setup = FindFirstObjectByType<LevelSetup>();
        if (setup != null)
        {
            setup.ApplyLevelSetup();
            return;
        }

        EnsurePlayerBound();
        if (IsPlayerValid())
        {
            SetPlayerControl(true);
        }
        else
        {
            Debug.LogError("No LevelSetup or Player found in scene: " + SceneManager.GetActiveScene().name);
        }
    }

    public void PrepareLevel(GameObject scenePlayer, Vector3 spawnPosition)
    {
        if (scenePlayer == null)
        {
            Debug.LogError("PrepareLevel called with a null player.");
            return;
        }

        player = scenePlayer;
        keysCollected = 0;
        isProcessingDeath = false;
        isGameOver = false;
        SetPlayerControl(true);

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.isKinematic = false;
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        player.transform.position = spawnPosition;
        ResetCheckpointsForNewLevel(spawnPosition);
    }

    public void SaveState(Vector3 checkpointPosition)
    {
        EnsurePlayerBound();

        if (!IsPlayerValid())
        {
            Debug.LogError("Cannot save checkpoint: player is not assigned in " +
                           SceneManager.GetActiveScene().name);
            return;
        }

        CheckpointData newSave = new CheckpointData(checkpointPosition, currentScore, currentLives);
        checkpointStack.Push(newSave);
        Debug.Log("Game has been saved! Score: " + currentScore + " Lives: " + currentLives);
    }

    public void RespawnPlayer()
    {
        if (isGameOver || isProcessingDeath)
        {
            return;
        }

        EnsurePlayerBound();

        if (!IsPlayerValid())
        {
            Debug.LogError("Cannot respawn: player is not assigned in " +
                           SceneManager.GetActiveScene().name +
                           ". Add LevelSetup and assign Scene Player.");
            return;
        }

        isProcessingDeath = true;
        currentLives--;

        if (currentLives <= 0)
        {
            currentLives = 0;
            StartCoroutine(HandleGameOver());
            return;
        }

        PersistStats();

        CheckpointData lastSave = checkpointStack.Peek();

        if (lastSave != null)
        {
            currentScore = lastSave.savedScore;

            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
            }

            player.transform.position = lastSave.respawnPosition;

            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
            }

            Debug.Log("We are so back! With: " + currentLives + " lives and a score of: " + currentScore);
        }
        else
        {
            Debug.LogWarning("No checkpoint found when respawning.");
        }

        StartCoroutine(EndDeathCooldown());
    }

    private IEnumerator HandleGameOver()
    {
        isGameOver = true;
        SetPlayerControl(false);
        PersistStats();

        Debug.Log("GAME OVER!");

        yield return new WaitForSeconds(gameOverDelay);

        if (restartLevelOnGameOver)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            StartNewGame();
            isProcessingDeath = false;
            isGameOver = false;
            SceneManager.LoadScene(currentScene);
        }
    }

    private void SetPlayerControl(bool enabled)
    {
        if (!IsPlayerValid())
        {
            return;
        }

        PlayerController movement = player.GetComponent<PlayerController>();
        if (movement != null)
        {
            movement.enabled = enabled;
        }

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null && !enabled)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }
    }

    private IEnumerator EndDeathCooldown()
    {
        yield return new WaitForSeconds(deathCooldown);
        isProcessingDeath = false;
    }

    public void AddLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            PersistStats();
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        PersistStats();
    }

    public void PersistStats()
    {
        PlayerPrefs.SetInt("SavedScore", currentScore);
        PlayerPrefs.SetInt("SavedLives", currentLives);
        PlayerPrefs.Save();
    }

    public void CompleteLevel()
    {
        PersistStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadNextLevel(string sceneName)
    {
        PersistStats();
        keysCollected = 0;
        SceneManager.LoadScene(sceneName);
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteKey("SavedScore");
        PlayerPrefs.DeleteKey("SavedLives");
        PlayerPrefs.Save();

        currentScore = 0;
        currentLives = maxLives;
        keysCollected = 0;
        checkpointStack.ClearStack();
        isProcessingDeath = false;
        isGameOver = false;
        hasBootstrapped = true;
    }

    public void ResetCheckpointsForNewLevel(Vector3 newSpawnPosition)
    {
        checkpointStack.ClearStack();
        SaveState(newSpawnPosition);
    }
}