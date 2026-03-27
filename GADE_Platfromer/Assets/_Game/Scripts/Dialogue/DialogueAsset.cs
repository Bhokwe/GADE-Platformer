using UnityEngine;

[CreateAssetMenu(fileName = "NewLvlDialogue", menuName = "Dialogue/LvlAsset")] 
public class DialogueAsset : ScriptableObject
{
    public DialogueItem[] conversation; //trying something here... to hold dialogue
}
