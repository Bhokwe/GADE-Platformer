using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueAsset zoneDialogue;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<DialogueManager>().StartDialogue(zoneDialogue);
            Destroy(gameObject);
        }
    }
}
