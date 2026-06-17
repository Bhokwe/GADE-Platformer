using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isActivated)
        {
            return;
        }

        if (GameManager.instance == null)
        {
            Debug.LogError("Checkpoint cannot save: GameManager is missing in this scene.");
            return;
        }

        isActivated = true;

        Debug.Log("Player successfully triggered: " + gameObject.name);

        Vector3 safeSpawn = transform.position + new Vector3(0, 1, 0);
        GameManager.instance.SaveState(safeSpawn);

        Renderer checkpointRenderer = GetComponent<Renderer>();
        if (checkpointRenderer != null)
        {
            checkpointRenderer.material.color = Color.green;
        }
    }
}
