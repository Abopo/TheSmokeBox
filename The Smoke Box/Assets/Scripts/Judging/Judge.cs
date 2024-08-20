using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum JUDGE { CHIPP = 0, JAMBON, PITMASTER };

public enum TOPIC { PIECES = 0, CUTS, FAVPIECE };

public class Judge : MonoBehaviour {

    [SerializeField]
    JUDGE _judge;

    [SerializeField]
    Competitor _playerCompetitor;
    Submission _playerSubmission;

    string _dialogue;

    DialogueBubble _dialogueBubble;

    JudgingManager _judgingManager;

    private void Awake() {
        _dialogueBubble = GetComponentInChildren<DialogueBubble>(true);
        _judgingManager = FindObjectOfType<JudgingManager>();
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
        // Jims only lasts 1 stage, his piece is just a cube.
        switch (_judge) {
            case JUDGE.CHIPP:
                _dialogue = "I see the resemblance...";
                break;
            case JUDGE.JAMBON:
                _dialogue = "Very...symmetrical.";
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
                    _dialogue = "Hell yeah! This is my kind of piece!";
                    break;
                case JUDGE.JAMBON:
                    _dialogue = "Clean angles, and fabulous color scheme.";
                    break;
                case JUDGE.PITMASTER:
                    _dialogue = "go...<j=sample>FAST!";
                    break;
            }
        } else if (GameManager.Instance.stage == 2) {
            // 2nd piece is BB Chicken Sandwich. It looks very cold.
            switch (_judge) {
                case JUDGE.CHIPP:
                    _dialogue = "You know this isn't an ice sculpture competition, right?";
                    break;
                case JUDGE.JAMBON:
                    _dialogue = "A poor attempt at a sandwich...";
                    break;
                case JUDGE.PITMASTER:
                    _dialogue = "chilling...";
                    break;
            }
        }
    }

    void FrankyDialogue() {
        if (GameManager.Instance.stage == 1) {
            switch (_judge) {
                case JUDGE.CHIPP:
                    _dialogue = "What an electrifying piece!";
                    break;
                case JUDGE.JAMBON:
                    _dialogue = "Clean technique, as expected.";
                    break;
                case JUDGE.PITMASTER:
                    _dialogue = "I...like.";
                    break;
            }
        } else if (GameManager.Instance.stage == 2) {
            switch (_judge) {
                case JUDGE.CHIPP:
                    _dialogue = "YEOW! I'm sweating just thinking about it!";
                    break;
                case JUDGE.JAMBON:
                    _dialogue = "Oh my...";
                    break;
                case JUDGE.PITMASTER:
                    _dialogue = "<j=sample>HOTHOTHOTHOTHOTHOTHOTHOT";
                    break;
            }
        } else if (GameManager.Instance.stage == 3) {
            switch (_judge) {
                case JUDGE.CHIPP:
                    _dialogue = "I know it's wood, but it looks so good I could eat it!";
                    break;
                case JUDGE.JAMBON:
                    _dialogue = "It's near perfect in every way...";
                    break;
                case JUDGE.PITMASTER:
                    _dialogue = "<j=sample>GIVE ME PRETZEL!";
                    break;
            }
        }
    }

    void PlayerDialogue() {
        // Player's dialogue is the most complicated. It needs to be based on what the player used in their sculpture

        // Get the player's submission
        if (_playerCompetitor != null) {
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
        // Favorite Piece
        if (_playerSubmission.pieceNames.Contains("Cool S") && CanSay(TOPIC.FAVPIECE)) {
            _dialogue = "That piece is almost as COOL as me!";

            GameManager.Instance.chippTopics.Add(TOPIC.FAVPIECE);
            // Favorite Color
        } else if(_playerSubmission.colorsUsed.Count(n => n == PAINTCOLOR.RED) >= 2) {
            _dialogue = "The use of red is spectacular!";
        } else if (_playerSubmission.numPiecesUsed >= 6 && CanSay(TOPIC.PIECES)) {
            _dialogue = "Woah! Look at all the pieces they used!";

            // Add to the topics lists
            _judgingManager.topics.Add(TOPIC.PIECES);
            GameManager.Instance.chippTopics.Add(TOPIC.PIECES);
        } else {
            // Default line
            switch (GameManager.Instance.stage) {
                case 1:
                    _dialogue = "Loving the vibe of this piece.";
                    break;
                case 2:
                    _dialogue = "I could imagine this piece being in my foyer.";
                    break;
                case 3:
                    _dialogue = "What a Chipptastic piece!";
                    break;
                default:
                    _dialogue = "Loving the vibe of this piece.";
                    break;
            }
        }
    }

    void PlayerDialogueJambon() {
        // Priority of each dialogue will simply be via if statements
        // Favorite Piece
        if (_playerSubmission.pieceNames.Contains("Lovely") && CanSay(TOPIC.FAVPIECE)) {
            _dialogue = "Oh what a LOVELY piece!";
            
            GameManager.Instance.jambonTopics.Add(TOPIC.FAVPIECE);
        // Favorite Color
        } else if (_playerSubmission.colorsUsed.Count(n => n == PAINTCOLOR.PINK) >= 2) {
            _dialogue = "Ah, another who appreciates the depth of pink.";
        } else if (_playerSubmission.numCutsUsed >= 3 && CanSay(TOPIC.CUTS)) {
            _dialogue = "Exquisite cuts.";

            // Add to the topics lists
            _judgingManager.topics.Add(TOPIC.CUTS);
            GameManager.Instance.jambonTopics.Add(TOPIC.CUTS);
        } else if (_playerSubmission.numPiecesUsed >= 5 && CanSay(TOPIC.PIECES)) {
            _dialogue = "Despite using so many pieces, it all comes together nicely.";

            // Add to the topics lists
            _judgingManager.topics.Add(TOPIC.PIECES);
            GameManager.Instance.jambonTopics.Add(TOPIC.PIECES);
        } else {
            // Default line
            switch (GameManager.Instance.stage) {
                case 1:
                    _dialogue = "Mmm...not bad.";
                    break;
                case 2:
                    _dialogue = "An improvement from last time for certain.";
                    break;
                case 3:
                    _dialogue = "...I'm impressed.";
                    break;
                default:
                    _dialogue = "Mmm...impressive.";
                    break;
            }
        }
    }

    void PlayerDialoguePitmaster() {
        // Priority of each dialogue will simply be via if statements

        // Favorite Piece
        if(_playerSubmission.pieceNames.Contains("Dinosaur") && CanSay(TOPIC.FAVPIECE)) {
            _dialogue = "Dino dino dino<j=sample>SAAUURR!!!";
            
            GameManager.Instance.pitmasterTopics.Add(TOPIC.FAVPIECE);
        // Favorite Color
        } else if (_playerSubmission.colorsUsed.Count(n => n == PAINTCOLOR.BLACK) >= 2) {
            _dialogue = "Black...<j=sample>BLACK!";
        // Pieces
        } else if (_playerSubmission.numPiecesUsed >= 5 && CanSay(TOPIC.PIECES)) {
            _dialogue = "Many pieces...many <j=sample>POINTS!";

            // Add to the topics lists
            _judgingManager.topics.Add(TOPIC.PIECES);
            GameManager.Instance.pitmasterTopics.Add(TOPIC.PIECES);
        // Cuts
        } else if (_playerSubmission.numCutsUsed >= 3 && CanSay(TOPIC.CUTS)) {
            _dialogue = "sliced...diced...perfection.";

            // Add to the topics lists
            _judgingManager.topics.Add(TOPIC.CUTS);
            GameManager.Instance.pitmasterTopics.Add(TOPIC.CUTS);
        // Default
        } else {
            // Default line
            switch (GameManager.Instance.stage) {
                case 1:
                    _dialogue = "...spicy...";
                    break;
                case 2:
                    _dialogue = "oooo....<j=sample>OOHHHHHH";
                    break;
                case 3:
                    _dialogue = "in...<j=sample>CREDIBLE!";
                    break;
                default:
                    _dialogue = "...spicy...";
                    break;
            }
        }
    }

    bool CanSay(TOPIC topic) {
        bool canSay = true;

        if(_judgingManager.topics.Contains(topic)) {
            canSay = false;
        }

        switch (_judge) {
            case JUDGE.CHIPP:
                if (GameManager.Instance.chippTopics.Contains(topic)) {
                    canSay = false;
                }
                break;
            case JUDGE.JAMBON:
                if (GameManager.Instance.jambonTopics.Contains(topic)) {
                    canSay = false;
                }
                break;
            case JUDGE.PITMASTER:
                if (GameManager.Instance.pitmasterTopics.Contains(topic)) {
                    canSay = false;
                }
                break;
        }

        return canSay;
    }
}
