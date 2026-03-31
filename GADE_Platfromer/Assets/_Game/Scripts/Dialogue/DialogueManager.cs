using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI messageTxt;
    public Image iconImage;
    public GameObject messagePanel;

    public DialogueAsset currentLevelDialogue;
    private OwnQueue<DialogueItem> dialogueQueue; //declaring OwnQueue & initialised in line 17

    
    void Awake()
    {
        dialogueQueue = new OwnQueue<DialogueItem>(); // initialising queue
    }

    void Start()
    {
        if (currentLevelDialogue != null)
        {
            StartDialogue(currentLevelDialogue);
        }
        else
        {
            Debug.LogWarning("You forgot to attach Dialogue asset in inspector!");
        }
    }

            public void StartDialogue(DialogueAsset asset)
            {
                messagePanel.SetActive(true); // shows UI panel
                
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                while (!dialogueQueue.IsEmpty())
                {
                    dialogueQueue.Dequeue();
                }

                foreach (DialogueItem item in asset.conversation)
                {
                    dialogueQueue.Enqueue(item);
                }

                DisplayNextSentence();
            }

    public void DisplayNextSentence() //method for next button
    {
        if (dialogueQueue.IsEmpty()) // empty queue... ?
        { EndDialogue(); return; } // then marks end of conversation

        DialogueItem currentItem = dialogueQueue.Dequeue(); //gets new things

        nameTxt.text = currentItem.Name;
        messageTxt.text = currentItem.messagetxt;
        iconImage.sprite = currentItem.Icon;
    }

    void EndDialogue()
    {
        messagePanel.SetActive(false); //Closes UI ... yoh bro... 
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


}
