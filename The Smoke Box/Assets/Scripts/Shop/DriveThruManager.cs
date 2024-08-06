using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    float _nextLineTime;
    float _nextLineTimer = 0f;

    bool _lineFinished = false;

    [SerializeField]
    Car _car;
    int _carMoveRatio = 0;
    [SerializeField]
    CarCam _carCam;

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
    void Update()
    {
        if(_inDialogue && _lineFinished) {
            _nextLineTimer += Time.deltaTime;
            if(_nextLineTimer > _nextLineTime - 1f) {
                _dialogueCanvas.SetActive(false);
            }
            if (_nextLineTimer >= _nextLineTime) {
                ShowNextDialogue();
            }
        }
    }

    void ShowNextDialogue() {
        if (_dialogueIndex < _dtDialogueSO.dialogueList.Length) {
            _lineFinished = true;
            _nextLineTimer = 0f;

            _dialogueCanvas.SetActive(true);

            _dialogueText.text = _dtDialogueSO.dialogueList[_dialogueIndex].dialogueLine;
            _dialogueIndex++;

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
        _lineFinished = true;
    }

    void EndScene() {
        _inDialogue = false;

        _dialogueCanvas.SetActive(false);
        // Turn the camera to face the shop window
        _carCam.GoToShopView();
    }
}
