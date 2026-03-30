using UnityEngine;

[System.Serializable]
public class CheckpointData
{
    public Vector3 respawnPosition;
    public int savedScore;
    public int savedLives;


    //constructor to initialize the checkpoint data together
    public CheckpointData(Vector3 position, int score, int lives)
    {
        respawnPosition = position;
        savedScore = score;
        savedLives = lives;
    }

}
