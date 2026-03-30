using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Stats")]
    public GameObject player;
    public int currentScore = 0;
    public int currentLives = 3;
    public int maxLives = 3;

    //stack instansiation 
    private CheckpointStackADT checkpointStack = new CheckpointStackADT();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        SaveState(player.transform.position);
        
    }
    public void SaveState(Vector3 checkpointPosition)
    {
        CheckpointData newSave = new CheckpointData(checkpointPosition, currentScore, currentLives);
        checkpointStack.Push(newSave);
        Debug.Log("Game has been saved at checkpoint: " + currentScore);
    }

    public void RespawnPlayer()
    {
        currentLives--; //decrease player lives when they die

        if (currentLives <= 0) 
        {
            Debug.Log("GAME OVER! (Insert game over screen here)");

            return;
        }

        CheckpointData lastSave = checkpointStack.Peek();

        if (lastSave != null)
        {
            currentScore = lastSave.savedScore;

            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null) 
            {
                playerRb.linearVelocity = Vector3.zero; //stops the player movement from the previous checkpoint

            }

            player.transform.position = lastSave.respawnPosition;

            Debug.Log("We are so back! With: "+currentLives+ " lives and a score of: "+currentScore);


        }   
    }
    public void AddLife()
    {
        //lives to be added when the player has less than the max lives
        if (currentLives < maxLives)
        {
            currentLives++;
            Debug.Log("We are so back! You now have " + currentLives + " lives.");
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        Debug.Log("Score added! Current score: " + currentScore);
    }
}

