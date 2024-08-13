using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DriveThruManager : MonoBehaviour {

    bool _inDialogue = false;

    [SerializeField]
    DriveThruDialogue _dtDialogueSO;

    int _dialogueIndex;

    [SerializeField]
    GameObject _dialogueCanvas;
    [SerializeField]
    SuperTextMesh _dialogueText;
    [SerializeField]
    RectTransform _dialogueArrow;

    [SerializeField]
    float _nextLineTime;
    float _nextLineTimer = 0f;

    bool _lineFinished = false;

    [SerializeField]
    Car _car;
    int _carMoveRatio = 0;
    [SerializeField]
    CarCam _carCam;

    [SerializeField]
    Waypoint _finalWaypoint; // For debugging

    // Start is called before the first frame update
    void Start()
    {
        StartDialogue();
    }

    void StartDialogue() {
        _dialogueIndex = 0;

        // There are 6 positions in the line
        _carMoveRatio = _dtDialogueSO.dialogueList.Length / 6;

        _dialogueText.OnCompleteEvent += OnLineFinished;

        _inDialogue = true;

        ShowNextDialogue();
    }

    // Update is called once per frame
    void Update() {
        if(_inDialogue && _lineFinished) {
            _nextLineTimer += Time.deltaTime;
            if(_nextLineTimer > _nextLineTime - 0.5f) {
                _dialogueCanvas.SetActive(false);
            }
            if (_nextLineTimer >= _nextLineTime) {
                ShowNextDialogue();
            }
        }

        // Debugging TODO: remove
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            StartCoroutine(ForceEnd());
        }
    }

    void ShowNextDialogue() {
        if (_dialogueIndex < _dtDialogueSO.dialogueList.Length) {
            // Show the dialogue canvas
            _dialogueCanvas.SetActive(true);

            // Set the line of dialogue
            _dialogueText.text = _dtDialogueSO.dialogueList[_dialogueIndex].dialogueLine;

            // Move the speaker arrow to the right spot
            switch (_dtDialogueSO.dialogueList[_dialogueIndex].speaker) {
                case FRIENDS.MC:
                    _dialogueArrow.anchoredPosition = new Vector2(-610f, _dialogueArrow.anchoredPosition.y);
                    break;
                case FRIENDS.DF:
                    _dialogueArrow.anchoredPosition = new Vector2(735f, _dialogueArrow.anchoredPosition.y);
                    break;
                case FRIENDS.BF1:
                    _dialogueArrow.anchoredPosition = new Vector2(-210f, _dialogueArrow.anchoredPosition.y);
                    break;
                case FRIENDS.BF2:
                    _dialogueArrow.anchoredPosition = new Vector2(210f, _dialogueArrow.anchoredPosition.y);
                    break;
            }

            _dialogueIndex++;
            _lineFinished = false;
            _nextLineTimer = 0f;

            CheckCarMovement();
        } else {
            // End cutscene
            EndScene();
        }
    }

    void CheckCarMovement() {
        // The car should move forward in the line as the dialogue progresses
        // We should be able to just divide the number of lines by the number of positions in line
        // and more the car accordingly
        if(_dialogueIndex % _carMoveRatio == 0) {
            // Move the line forward
            _car.GoToWaypoint();
        }
    }

    void OnLineFinished() {
        Debug.Log("Line finished.");
        _lineFinished = true;
    }

    void EndScene() {
        _inDialogue = false;

        _dialogueCanvas.SetActive(false);
        // Turn the camera to face the shop window
        _carCam.GoToShopView();
    }

    IEnumerator ForceEnd() {
        _car.GoToWaypoint(_finalWaypoint);

        yield return new WaitForSeconds(2);

        EndScene();
    }
}
