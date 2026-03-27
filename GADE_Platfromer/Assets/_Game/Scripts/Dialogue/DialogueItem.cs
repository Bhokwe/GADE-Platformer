using UnityEngine;

[System.Serializable]

public class DialogueItem
{
    public string Name; //name of speaker
    public Sprite Icon;// speaker's icon image

    [TextArea(3, 10)]
    public string messagetxt; //dialgoue text 
}
