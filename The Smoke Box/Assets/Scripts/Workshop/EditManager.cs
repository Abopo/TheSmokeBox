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
    float _tranSpeed;
    [SerializeField]
    float _rotSpeed;
    [SerializeField]
    float _orbitSpeed;

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
    bool _canMovePiece = true;

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
        _submission = FindFirstObjectByType<Submission>();
        _mouse = Mouse.current;
        _keyboard = Keyboard.current;
        _cameraLerp = Camera.main.GetComponent<LerpTo>();
        _canvas = GetComponentInChildren<EditorCanvas>();
        editAudio = GetComponentInChildren<EditAudio>();

        LookAtSubmission();
    }

    // Update is called once per frame
    void Update() {
        CheckInput();

        if (_canMovePiece) {
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
        }
    }

    void CheckInput() {
        if (_active) {
            if (_keyboard.spaceKey.wasPressedThisFrame) {
                if (_view == VIEW.SUBMISSION) {
                    LookAtTable();
                } else if (_view == VIEW.TABLE) {
                    LookAtSubmission();
                }
            }
            if (_keyboard.shiftKey.wasPressedThisFrame) {
                LookAtTable();
            }
        }

        if(_canMovePiece) {
            if (_keyboard.wKey.isPressed) {
                // Translate piece up
                TranslatePieceY(1);
            }
            if (_keyboard.sKey.isPressed) {
                // Translate piece down
                TranslatePieceY(-1);
            }
            if (_keyboard.aKey.isPressed) {
                // Translate piece left
                TranslatePieceX(-1);
            }
            if (_keyboard.dKey.isPressed) {
                // Translate piece right
                TranslatePieceX(1);
            }

            if (_mouse.rightButton.isPressed) {
                RotatePieceMouse();
            }
            if (_keyboard.eKey.isPressed) {
                RotatePieceZ(-1);
            }
            if (_keyboard.qKey.isPressed) {
                RotatePieceZ(1);
            }
        }

        if (_view == VIEW.SUBMISSION) {
            if (_mouse.middleButton.isPressed) {
                SubmissionOrbitMouse();
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
            _cameraLerp.LerpToOrigin(0.5f);
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

        FindFirstObjectByType<ToolsWindow>().hasBase = true;
    }

    void TranslatePieceMouse() {
        if (curPiece != null) {
            float h = _tranSpeed * _mouse.delta.x.ReadValue();
            float v = _tranSpeed * _mouse.delta.y.ReadValue();

            curPiece.transform.Translate(h, 0f, 0f, Space.World);
            curPiece.transform.Translate(v, 0f, 0f, Space.World);
        }
    }

    void TranslatePieceX(int dir) {
        if (curPiece != null) {
            if (_view == VIEW.TABLE) {
                if (dir == 1 && curPiece.transform.position.x < 2.5f ||
                    dir == -1 && curPiece.transform.position.x > -2.5f) {
                    curPiece.transform.Translate(Camera.main.transform.right * _tranSpeed * dir * Time.deltaTime, Space.World);
                }
            } else if(_view == VIEW.SUBMISSION) {
                Vector3 difference = curPiece.transform.position - _submission.transform.position;
                Vector3 direction = Camera.main.transform.right;

                float distAlongVector = Vector3.Dot(difference, direction);
                float distFromSubmission = Vector3.Distance(curPiece.transform.position, _submission.transform.position);

                // curPiece is to the right of the submission based on the camera
                if (distAlongVector >= 0f) {
                    if(dir == 1 && distFromSubmission < 3f || dir == -1) {
                        curPiece.transform.Translate(Camera.main.transform.right * _tranSpeed * dir * Time.deltaTime, Space.World);
                    }
                    // curPiece is to the left of the submission based on the camera
                } else if (distAlongVector <= 0f) {
                    if (dir == -1 && distFromSubmission < 3f || dir == 1) {
                        curPiece.transform.Translate(Camera.main.transform.right * _tranSpeed * dir * Time.deltaTime, Space.World);
                    }
                }
            }
        }
    }

    void TranslatePieceY(int dir) {
        if (curPiece != null) {
            if (_view == VIEW.TABLE) {
                if (dir == 1 && curPiece.transform.position.y < 6f ||
                    dir == -1 && curPiece.transform.position.y > 4.2f) {
                    curPiece.transform.Translate(Camera.main.transform.up * _tranSpeed * dir * Time.deltaTime, Space.World);
                }
            } else if(_view == VIEW.SUBMISSION) {
                Vector3 difference = curPiece.transform.position - _submission.transform.position;
                Vector3 direction = Camera.main.transform.up;

                float distAlongVector = Vector3.Dot(difference, direction);
                float distFromSubmission = Vector3.Distance(curPiece.transform.position, _submission.transform.position);

                // curPiece is to the right of the submission based on the camera
                if (distAlongVector >= 0f) {
                    if (dir == 1 && distFromSubmission < 3f || dir == -1) {
                        curPiece.transform.Translate(Camera.main.transform.up * _tranSpeed * dir * Time.deltaTime, Space.World);
                    }
                    // curPiece is to the left of the submission based on the camera
                } else if (distAlongVector <= 0f) {
                    if (dir == -1 && distFromSubmission < 3f || dir == 1) {
                        curPiece.transform.Translate(Camera.main.transform.up * _tranSpeed * dir * Time.deltaTime, Space.World);
                    }
                }
            }
        }
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

    void SubmissionOrbitMouse() {
        float h = _orbitSpeed * _mouse.delta.x.ReadValue();
        float v = _orbitSpeed * _mouse.delta.y.ReadValue();

        _cameraLerp.transform.RotateAround(_submission.transform.position, new Vector3(0f, 1f, 0f), h);
        _cameraLerp.transform.RotateAround(_submission.transform.position, _cameraLerp.transform.right, -v);
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
        _canMovePiece = true;

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
            _canMovePiece = false;
        }

        _lookUpUI.SetActive(false);
        _lookDownUI.SetActive(false);

        if (curPiece != null) {
            _canvas.HideBaseUI();
        }
    }

    public void DisableRotation() {
        _canMovePiece = false;
    }

    public void EnableRotation() {
        _canMovePiece = true;
    }

    public void ClearHoldPiece() {
        holdPiece = null;
    }
}
