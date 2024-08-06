using System;
using UnityEngine;

public enum FRIENDS { MC = 0, DF, BF1, BF2 };

[Serializable]
public struct DialogueLine
{
    public string dialogueLine;
    public FRIENDS speaker;
}

[CreateAssetMenu(fileName = "DriveThruDialogue", menuName = "ScriptableObjects/DriveThruDialogue", order = 3)]
public class DriveThruDialogue : ScriptableObject {
    public DialogueLine[] dialogueList;
}
