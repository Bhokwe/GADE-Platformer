using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            Debug.Log("Player successfully triggered: " + gameObject.name);

            Vector3 safeSpawn = transform.position + new Vector3(0, 1, 0); // adjusted the spawn position to be slightly above the checkpoint
            GameManager.instance.SaveState(safeSpawn);

            GetComponent<Renderer>().material.color = Color.green; // change the checkpoint color to green to indicate it's activated
        }
    }
}
