using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(string sceneName)
    {
        EnsureGameManager();
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevelFresh(string sceneName)
    {
        EnsureGameManager();
        GameManager.instance.StartNewGame();
        SceneManager.LoadScene(sceneName);
    }

    private static void EnsureGameManager()
    {
        if (GameManager.instance != null)
        {
            return;
        }

        new GameObject("GameManager").AddComponent<GameManager>();
    }
}
