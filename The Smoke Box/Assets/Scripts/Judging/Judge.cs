using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum JUDGE { CHIPP = 0, JAMBON, PITMASTER };

public class Judge : MonoBehaviour {

    [SerializeField]
    JUDGE _judge;

    [SerializeField]
    Competitor _playerCompetitor;
    Submission _playerSubmission;

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
                _dialogue = "I see the resemblance...";
                break;
            case JUDGE.JAMBON:
                _dialogue = "Very...symmetrical";
                break;
            case JUDGE.PITMASTER:
                _dialogue = "square.";
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

        // Get the player's submission
        if(_playerCompetitor != null) {
            _playerSubmission = _playerCompetitor.GetComponentInChildren<Submission>();
        }

        // Figure out dialogue based on the stats
        switch (_judge) {
            case JUDGE.CHIPP:
                PlayerDialogueChipp();
                break;
            case JUDGE.JAMBON:
                PlayerDialogueJambon();
                break;
            case JUDGE.PITMASTER:
                PlayerDialoguePitmaster();
                break;
        }
    }

    void PlayerDialogueChipp() {
        // Priority of each dialogue will simply be via if statements
        if(_playerSubmission.colorsUsed.Count(n => n == PAINTCOLOR.RED) >= 2) {
            _dialogue = "The use of red is spectacular!";
        } else if(_playerSubmission.numPiecesUsed >= 5) {
            _dialogue = "Woah! Look at all the pieces they used!";
        } else {
            // Default line
            _dialogue = "Loving the vibe of this piece.";
        }
    }

    void PlayerDialogueJambon() {
        // Priority of each dialogue will simply be via if statements
        if (_playerSubmission.colorsUsed.Count(n => n == PAINTCOLOR.PINK) >= 2) {
            _dialogue = "Ah, another who appreciates the depth of pink.";
        } else if (_playerSubmission.numCutsUsed >= 5) {
            _dialogue = "Exquisite cuts.";
        } else if (_playerSubmission.numCutsUsed >= 5) {
            _dialogue = "Despite using so many pieces, it all comes together nicely.";
        } else {
            // Default line
            _dialogue = "Mmm…impressive.";
        }
    }

    void PlayerDialoguePitmaster() {
        // Priority of each dialogue will simply be via if statements
        if (_playerSubmission.colorsUsed.Count(n => n == PAINTCOLOR.BLACK) >= 2) {
            _dialogue = "Black…BLACK!";
        } else if (_playerSubmission.numPiecesUsed >= 5) {
            _dialogue = "Many pieces…many POINTS!";
        } else {
            // Default line
            _dialogue = "...spicy...";
        }
    }
}
