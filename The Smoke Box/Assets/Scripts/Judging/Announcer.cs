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
        _dialogues.Add("Welcome to the 16th annual Heartwood Woodworking competition!");
        _dialogues.Add("Today is the live judging of our Top 4 competitors!");
        _dialogues.Add("Our first entrant is Jims!");
        _dialogues.Add("Let's hear what he has to say about his submission.");
        _dialogues.Add("Alright! Let's see what the judges have to say!");
        _dialogues.Add("Our next entrant is Belle!");
        _dialogues.Add("What would you like the judges to know about your submission?");
        _dialogues.Add("Okay! Over to the judges!");
        _dialogues.Add("Next up is our reigning champion, Franky!");
        _dialogues.Add("Let's see what he's cooked up this year!");
        _dialogues.Add("Wow! I bet the judges'll like this one!");
        _dialogues.Add("Finally, our last entrant is Kaden!");
        _dialogues.Add("Whatchya got there bud?");
        _dialogues.Add("Cool! Judges?");
        _dialogues.Add("Alright! Let's take a look at the results!");
    }
    void LoadStage2Dialogues() {
        _dialogues.Add("Welcome back to the 16th annual Heartwood Woodworking competition!");
        _dialogues.Add("We're down to the Top 3 competitors!");
        _dialogues.Add("Back again is Belle. What's your piece this time?");
        _dialogues.Add("Uhhh okay...judges?");
        _dialogues.Add("Moving on, let's see what Franky's got today!");
        _dialogues.Add("I'm feeling it! But what about our judges?");
        _dialogues.Add("Last up, the rookie who's been flexing his creativity: Kaden!");
        _dialogues.Add("Alright! Let's toss it over to the judges!");
        _dialogues.Add("Let's take a look at the results!");
    }
    void LoadStage3Dialogues() {
        _dialogues.Add("You know what's up! We're down to our final 2 competitors!");
        _dialogues.Add("Which one will take this year's title? Let's take a look!");
        _dialogues.Add("Oh my! What has Franky cooked up this time for his final piece?!");
        _dialogues.Add("Deeelectable! Do the judges agree?");
        _dialogues.Add("That's gonna be hard to follow up!");
        _dialogues.Add("What has Kaden got in store for us today?");
        _dialogues.Add("I'm speechless! I hope the judges have more to say then me!");
        _dialogues.Add("This is it folks! Let's see the final results!");
    }

    public void NextDialogue() {
        if (_dialogueIndex < _dialogues.Count) {
            _dialogueBubble.ShowDialogue(_dialogues[_dialogueIndex++]);
        }
    }
}
