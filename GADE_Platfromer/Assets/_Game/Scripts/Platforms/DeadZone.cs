using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (GameManager.instance == null)
        {
            Debug.LogError("DeadZone triggered but GameManager is missing.");
            return;
        }

        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlaySFX("DIE");
        }

        GameManager.instance.RespawnPlayer();
    }
}
