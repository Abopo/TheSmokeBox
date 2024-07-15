using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditManager : MonoBehaviour {

    [SerializeField]
    WoodPiece _curPiece;

    [SerializeField]
    float _rotSpeed;

    Mouse _mouse;
    Keyboard _keyboard;

    Plane _sawPlane;


    public static EditManager Instance;

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
        _mouse = Mouse.current;
        _keyboard = Keyboard.current;

    }

    // Update is called once per frame
    void Update() {
        CheckInput();
    }

    void CheckInput() {
        if (_curPiece != null) {
            if (_keyboard.rightArrowKey.IsPressed()) {
                _curPiece.transform.Rotate(new Vector3(0, -_rotSpeed * 50 * Time.deltaTime, 0), Space.World);
            }
            if (_keyboard.leftArrowKey.IsPressed()) {
                _curPiece.transform.Rotate(new Vector3(0, _rotSpeed * 50 * Time.deltaTime, 0), Space.World);
            }
            if (_keyboard.upArrowKey.IsPressed()) {
                _curPiece.transform.Rotate(new Vector3(_rotSpeed * 50 * Time.deltaTime, 0, 0), Space.World);
            }
            if (_keyboard.downArrowKey.IsPressed()) {
                _curPiece.transform.Rotate(new Vector3(-_rotSpeed * 50 * Time.deltaTime, 0, 0), Space.World);
            }

            if(_keyboard.spaceKey.wasPressedThisFrame) {
                SawPiece(_curPiece);
            }

            if (_mouse.rightButton.isPressed) {
                RotatePiece();
            }
        }
    }

    void RotatePiece() {
        float h = _rotSpeed * _mouse.delta.x.ReadValue();
        float v = _rotSpeed * _mouse.delta.y.ReadValue();
        _curPiece.transform.Rotate(new Vector3(v, -h, 0), Space.World);
    }

    public void PickUpPiece(WoodPiece wPiece) {
        // Put down our current piece
        if (_curPiece != null) {
            _curPiece.Drop();
        }

        // Hold the new piece
        wPiece.GetComponent<LerpTo>().LerpToPos(transform.position);
        _curPiece = wPiece;
    }

    public void SawPiece(WoodPiece wPiece) {
        //Transform the normal so that it is aligned with the object we are slicing's transform.
        Vector3 transformedNormal = ((Vector3)(wPiece.gameObject.transform.localToWorldMatrix.transpose * Vector3.right)).normalized;

        //Get the enter position relative to the object we're cutting's local transform
        Vector3 transformedStartingPoint = wPiece.gameObject.transform.InverseTransformPoint(transform.position);

        _sawPlane = new Plane(transformedNormal, transformedStartingPoint);

        // Slice the piece
        GameObject[] pieces = Slicer.Slice(_sawPlane, wPiece.gameObject);

        // Destroy the original, as there will be 2 game objects made
        Destroy(wPiece.gameObject);

        _curPiece = pieces[0].GetComponent<WoodPiece>();
    }
}
