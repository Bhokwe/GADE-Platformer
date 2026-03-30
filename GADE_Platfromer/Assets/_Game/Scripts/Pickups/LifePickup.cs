using UnityEngine;

public class LifePickup : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(Vector3.up * 60f * Time.deltaTime); // rotate the pickup 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance.currentLives < GameManager.instance.maxLives)
            {
                GameManager.instance.AddLife();
                Debug.Log("Player picked up a life! Current lives: " + GameManager.instance.currentLives);
                Destroy(gameObject);


            }
            else 
            {
                Debug.Log("You are life maxxing brudda! You are all good.");
            }
        }
    }
}
