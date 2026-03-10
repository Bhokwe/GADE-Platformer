using UnityEngine;
using UnityEngine.SceneManagement; //Reloads scenes

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Checks if player has fallen into dead zone
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }
}
