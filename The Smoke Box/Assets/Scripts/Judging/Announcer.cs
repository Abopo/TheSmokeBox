using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Announcer : MonoBehaviour {

    List<string> _dialogues = new List<string> ();
    int _dialogueIndex = 0;

    DialogueBubble _dialogueBubble;

    private void Awake() {
        _dialogueBubble = GetComponent<DialogueBubble> ();
    }
    // Start is called before the first frame update
    void Start() {
        PopulateDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PopulateDialogue() {
        if (GameManager.Instance.stage == 1) {
            LoadStage1Dialogues();
        } else if (GameManager.Instance.stage == 2) {
            LoadStage2Dialogues();
        } else if (GameManager.Instance.stage == 3) {
            LoadStage3Dialogues();
        }
    }

    void LoadStage1Dialogues() {
        _dialogues.Add("Welcome to the 16th annual Whakado City Woodworking competition!");
        _dialogues.Add("Our first entrant is Jims!");
        _dialogues.Add("Let's hear what he has to say about his submission.");
        _dialogues.Add("Alright! Let's see what the judges have to say!");
        _dialogues.Add("Our next entrant is Alexandria!");
        _dialogues.Add("What would you like the judges to know about your submission?");
        _dialogues.Add("Okay! Over to the judges!");
        _dialogues.Add("Next up is our reigning champion, Franky!");
        _dialogues.Add("Let's see what he's cooked up this year!");
        _dialogues.Add("Wow! I bet the judges'll like this one!");
        _dialogues.Add("Finally, our last entrant is Kaden!");
        _dialogues.Add("Whatchya got there bud?");
        _dialogues.Add("Cool! Judges?");
    }
    void LoadStage2Dialogues() {

    }
    void LoadStage3Dialogues() {

    }

    public void NextDialogue() {
        if (_dialogueIndex < _dialogues.Count) {
            _dialogueBubble.ShowDialogue(_dialogues[_dialogueIndex++]);
        }
    }
}
