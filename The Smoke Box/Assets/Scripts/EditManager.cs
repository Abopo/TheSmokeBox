using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class EditManager : MonoBehaviour {

    [SerializeField]
    Tool _curTool;

    public WoodPiece curPiece;
    WoodPiece _holdPiece;
    [SerializeField]
    float _rotSpeed;

    bool _active = true;

    Mouse _mouse;
    Keyboard _keyboard;
    LerpTo _cameraLerp;

    Submission _submission;

    public static EditManager Instance;

    public static UnityEvent OnPickedUpPiece = new UnityEvent();
    public static UnityEvent OnDroppedPiece = new UnityEvent();

    private void Awake() {
        SingletonCheck();
    }
    void SingletonCheck() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start() {
        _submission = FindObjectOfType<Submission>();
        _mouse = Mouse.current;
        _keyboard = Keyboard.current;
        _cameraLerp = Camera.main.GetComponent<LerpTo>();
    }

    // Update is called once per frame
    void Update() {
        if (_active) {
            CheckInput();
        }
    }

    void CheckInput() {
        if (_keyboard.wKey.wasPressedThisFrame) {
            LookAtSubmission();
        }
        if (_keyboard.sKey.wasPressedThisFrame) {
            LookAtTable();
        }

        if (curPiece != null) {
            if (_keyboard.rightArrowKey.IsPressed()) {
                curPiece.transform.Rotate(new Vector3(0, -_rotSpeed * 50 * Time.deltaTime, 0), Space.World);
            }
            if (_keyboard.leftArrowKey.IsPressed()) {
                curPiece.transform.Rotate(new Vector3(0, _rotSpeed * 50 * Time.deltaTime, 0), Space.World);
            }
            if (_keyboard.upArrowKey.IsPressed()) {
                curPiece.transform.Rotate(new Vector3(_rotSpeed * 50 * Time.deltaTime, 0, 0), Space.World);
            }
            if (_keyboard.downArrowKey.IsPressed()) {
                curPiece.transform.Rotate(new Vector3(-_rotSpeed * 50 * Time.deltaTime, 0, 0), Space.World);
            }

            if(_keyboard.spaceKey.wasPressedThisFrame) {
                _active = false;
                _curTool.UseTool();
            }

            if (_mouse.rightButton.isPressed) {
                RotatePiece();
            }
        }
    }

    public void LookAtSubmission() {
        _cameraLerp.LerpRotation(Quaternion.identity, 0.5f);
        // Save the piece we're working with
        _holdPiece = curPiece;
        // Set curPiece to the submission base so we can rotate it
        curPiece = _submission.baseTransform.GetComponent<WoodPiece>();
    }

    public void LookAtTable() {
        _cameraLerp.LerpRotation(Quaternion.Euler(50f, 0f, 0f), 0.5f);
        // Set the curPiece back to the hold piece
        // TODO: unless we've just jointed it to the submission?
        curPiece = _holdPiece;
    }

    public void SetPieceAsSubmissionBase() {
        // Set piece as child of submission
        _submission.AddPieceAsBase(curPiece);

        // Follow the piece to the submission
        LookAtSubmission();

        // Lose reference to piece
        curPiece = null;
    }

    void RotatePiece() {
        float h = _rotSpeed * _mouse.delta.x.ReadValue();
        float v = _rotSpeed * _mouse.delta.y.ReadValue();
        curPiece.transform.Rotate(new Vector3(v, -h, 0), Space.World);
    }

    public void PickUpPiece(WoodPiece wPiece) {
        // Put down our current piece
        if (curPiece != null) {
            curPiece.Drop();
        }

        // Hold the new piece
        wPiece.GoTo(transform.position);
        curPiece = wPiece;

        OnPickedUpPiece.Invoke();
    }

    public void SelectTool(Tool tool) {
        if (_curTool == tool) {
            // Deselect that tool
            _curTool.DeactivateTool();
            _curTool = null;
        } else {
            if(_curTool != null) {
                _curTool.DeactivateTool();
            }
            _curTool = tool;
            _curTool.ActivateTool();
        }
    }

    public void Activate() {
        _active = true;
    }
}
