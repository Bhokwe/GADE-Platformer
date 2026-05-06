using UnityEngine;
using TMPro; 

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    void Update()
    {
        //safety check 
        if (GameManager.instance != null)
        {
            
            scoreText.text = "Score: " + GameManager.instance.currentScore.ToString();
            livesText.text = "Lives: " + GameManager.instance.currentLives.ToString();
        }
        else
        {
            Debug.LogWarning("UIManager cannot find the GameManager!");
        }
    }
}