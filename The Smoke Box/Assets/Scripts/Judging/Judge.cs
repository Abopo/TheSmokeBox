using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JUDGE { CHIPP = 0, JAMBON, PITMASTER };

public class Judge : MonoBehaviour {

    [SerializeField]
    JUDGE _judge;

    string _dialogue;

    DialogueBubble _dialogueBubble;

    private void Awake() {
        _dialogueBubble = GetComponentInChildren<DialogueBubble>();
    }
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ChooseDialogue(int character) {
        switch ((COMPETITOR)character) {
            case COMPETITOR.JIMS:
                JimsDialogue();
                break;
            case COMPETITOR.ALEX:
                AlexDialogue();
                break;
            case COMPETITOR.FRANKY:
                FrankyDialogue();
                break;
            case COMPETITOR.PLAYER:
                PlayerDialogue();
                break;
        }

        _dialogueBubble.ShowDialogue(_dialogue);
    }

    void JimsDialogue() {
        // Jims only lasts 1 stage
        switch (_judge) {
            case JUDGE.CHIPP:
                _dialogue = "Chipp Jims 1";
                break;
            case JUDGE.JAMBON:
                _dialogue = "Jambon Jims 1";
                break;
            case JUDGE.PITMASTER:
                _dialogue = "Pitmaster Jims 1";
                break;
        }
    }

    void AlexDialogue() {
        if (GameManager.Instance.stage == 1) {
            switch (_judge) {
                case JUDGE.CHIPP:
                    _dialogue = "Chipp Alex 1";
                    break;
                case JUDGE.JAMBON:
                    _dialogue = "Jambon Alex 1";
                    break;
                case JUDGE.PITMASTER:
                    _dialogue = "Pitmaster Alex 1";
                    break;
            }
        } else if (GameManager.Instance.stage == 2) {
            switch (_judge) {
                case JUDGE.CHIPP:
                    _dialogue = "Chipp Alex 2";
                    break;
                case JUDGE.JAMBON:
                    _dialogue = "Jambon Alex 2";
                    break;
                case JUDGE.PITMASTER:
                    _dialogue = "Pitmaster Alex 2";
                    break;
            }
        }
    }

    void FrankyDialogue() {
        if (GameManager.Instance.stage == 1) {
            switch (_judge) {
                case JUDGE.CHIPP:
                    _dialogue = "Chipp Franky 1";
                    break;
                case JUDGE.JAMBON:
                    _dialogue = "Jambon Franky 1";
                    break;
                case JUDGE.PITMASTER:
                    _dialogue = "Pitmaster Franky 1";
                    break;
            }
        } else if (GameManager.Instance.stage == 2) {
            switch (_judge) {
                case JUDGE.CHIPP:
                    _dialogue = "Chipp Franky 2";
                    break;
                case JUDGE.JAMBON:
                    _dialogue = "Jambon Franky 2";
                    break;
                case JUDGE.PITMASTER:
                    _dialogue = "Pitmaster Franky 2";
                    break;
            }
        } else if (GameManager.Instance.stage == 3) {
            switch (_judge) {
                case JUDGE.CHIPP:
                    _dialogue = "Chipp Franky 3";
                    break;
                case JUDGE.JAMBON:
                    _dialogue = "Jambon Franky 3";
                    break;
                case JUDGE.PITMASTER:
                    _dialogue = "Pitmaster Franky 3";
                    break;
            }
        }
    }

    void PlayerDialogue() {
        // Player's dialogue is the most complicated. It needs to be based on what the player used in their sculpture
    }
}
