using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public enum VIEW { TABLE = 0, SUBMISSION };

public class EditManager : MonoBehaviour {

    [SerializeField]
    Tool _curTool;

    public WoodPiece curPiece;
    public WoodPiece holdPiece;
    [SerializeField]
    float _rotSpeed;

    [SerializeField]
    float _zoomSensitivity = 0.1f;

    [SerializeField]
    GameObject _lookUpUI;
    [SerializeField]
    GameObject _lookDownUI;

    [SerializeField]
    float _fovMin;
    [SerializeField]
    float _fovMax;

    bool _active = true;
    bool _canRotate = true;

    Mouse _mouse;
    Keyboard _keyboard;
    LerpTo _cameraLerp;

    Submission _submission;

    VIEW _view;

    EditorCanvas _canvas;
    public EditAudio editAudio;

    public static EditManager Instance;

    public static UnityEvent OnPickedUpPiece = new UnityEvent();
    public static UnityEvent OnDroppedPiece = new UnityEvent();
    public static UnityEvent OnLookUp = new UnityEvent();
    public static UnityEvent OnLookDown = new UnityEvent();

    public bool Active { get => _active; }

    public bool HasPiece {get => curPiece != null; }

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
        _canvas = GetComponentInChildren<EditorCanvas>();
        editAudio = GetComponentInChildren<EditAudio>();
    }

    // Update is called once per frame
    void Update() {
        if (_active) {
            CheckInput();
        }

        if (_canRotate) {
            if (_mouse.scroll.magnitude != 0) {
                Camera.main.fieldOfView -= _mouse.scroll.up.value * _zoomSensitivity;
                if (Camera.main.fieldOfView < _fovMin) {
                    Camera.main.fieldOfView = _fovMin;
                }
                Camera.main.fieldOfView += _mouse.scroll.down.value * _zoomSensitivity;
                if (Camera.main.fieldOfView > _fovMax) {
                    Camera.main.fieldOfView = _fovMax;
                }
            }
            if (_mouse.rightButton.isPressed) {
                RotatePieceMouse();
            }
            if (_keyboard.dKey.isPressed) {
                RotatePieceZ(-1);
            }
            if (_keyboard.aKey.isPressed) {
                RotatePieceZ(1);
            }
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
        }
    }

    public void LookAtSubmission() {
        if (_view != VIEW.SUBMISSION) {
            _cameraLerp.LerpRotation(Quaternion.identity, 0.5f);
            // Save the piece we're working with
            holdPiece = curPiece;
            // Set curPiece to the submission base so we can rotate it
            curPiece = _submission.baseTransform.GetComponent<WoodPiece>();

            _view = VIEW.SUBMISSION;

            OnLookUp.Invoke();

            if (_active) {
                _lookDownUI.SetActive(true);
            }
            _lookUpUI.SetActive(false);
        }
    }

    public void LookAtTable() {
        if (_view != VIEW.TABLE) {
            _cameraLerp.LerpRotation(Quaternion.Euler(50f, 0f, 0f), 0.5f);
            // Set the curPiece back to the hold piece
            // TODO: unless we've just jointed it to the submission?
            curPiece = holdPiece;

            _view = VIEW.TABLE;

            OnLookDown.Invoke();

            if (_active) {
                _lookUpUI.SetActive(true);
            }
            _lookDownUI.SetActive(false);
        }
    }

    public void SetPieceAsSubmissionBase() {
        // Set piece as child of submission
        _submission.AddPieceAsBase(curPiece);

        // Follow the piece to the submission
        LookAtSubmission();

        // Lose reference to piece
        holdPiece = null;

        FindObjectOfType<ToolsWindow>().hasBase = true;
    }

    void RotatePieceMouse() {
        if (curPiece != null) {
            float h = _rotSpeed * _mouse.delta.x.ReadValue();
            float v = _rotSpeed * _mouse.delta.y.ReadValue();

            curPiece.transform.Rotate(Camera.main.transform.up, -h, Space.World);
            curPiece.transform.Rotate(Camera.main.transform.right, v, Space.World);

            //curPiece.transform.Rotate(new Vector3(v, -h, 0), Space.World);
        }
    }

    void RotatePieceZ(int dir) { 
        if(curPiece != null) {
            curPiece.transform.Rotate(Camera.main.transform.forward, 50f * dir * Time.deltaTime, Space.World);
            //curPiece.transform.Rotate(0f, 0f, 50f * dir * Time.deltaTime, Space.World);
        }
    }

    public void PickUpPiece(WoodPiece wPiece) {
        // Put down our current piece
        if (curPiece != null) {
            curPiece.Drop();
        }

        // Hold the new piece
        wPiece.GoTo(transform.position);
        curPiece = wPiece;

        curPiece.lerp.OnLerpFinished.AddListener(OnPickUpFinished);
    }

    void OnPickUpFinished() {
        OnPickedUpPiece.Invoke();
        
        curPiece.lerp.OnLerpFinished.RemoveListener(OnPickUpFinished);
    }

    public void DropPiece() {
        // Put down our current piece
        if (curPiece != null) {
            curPiece.Drop();
            curPiece = null;
        }

        OnDroppedPiece.Invoke();
    }

    public void SelectTool(Tool tool) {
        if (_curTool == tool) {
            // Deselect that tool
            _curTool.DeactivateTool();
            Activate();
        } else {
            if(_curTool != null) {
                _curTool.DeactivateTool();
            }
            _curTool = tool;
            _curTool.ActivateTool();
            Deactivate(false);
        }
    }

    public void Activate() {
        _active = true;
        _canRotate = true;

        // If we are being activated, we shouldn't have a curTool
        _curTool = null;

        if(_view == VIEW.SUBMISSION) {
            _lookDownUI.SetActive(true);
        } else {
            _lookUpUI.SetActive(true);
        }

        // Show the UI if we've got a piece and we're looking at the table
        if(curPiece != null && _view == VIEW.TABLE) {
            _canvas.ShowBaseUI();
        }
    }

    public void Deactivate(bool full) {
        _active = false;

        if (full) {
            _canRotate = false;
        }

        _lookUpUI.SetActive(false);
        _lookDownUI.SetActive(false);

        if (curPiece != null) {
            _canvas.HideBaseUI();
        }
    }

    public void DisableRotation() {
        _canRotate = false;
    }

    public void EnableRotation() {
        _canRotate = true;
    }

    public void ClearHoldPiece() {
        holdPiece = null;
    }
}
