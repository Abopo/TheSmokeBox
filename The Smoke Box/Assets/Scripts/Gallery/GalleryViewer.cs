using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GalleryViewer : MonoBehaviour
{
    [SerializeField]
    float _rotSpeed;

    [SerializeField]
    float _zoomSensitivity = 0.1f;

    [SerializeField]
    float _fovMin;
    [SerializeField]
    float _fovMax;

    private Submission _submission;

    private Mouse _mouse;
    private Keyboard _keyboard;

    private Quaternion _startRotation;
    private float _startFOV;

    private void Start()
    {
        _mouse = Mouse.current;
        _keyboard = Keyboard.current;
    }

    private void Update()
    {
        if (_submission == null)
        {
            return;
        }

        CheckInput();

        if (_mouse.scroll.magnitude != 0)
        {
            Camera.main.fieldOfView -= _mouse.scroll.up.value * _zoomSensitivity;
            if (Camera.main.fieldOfView < _fovMin)
            {
                Camera.main.fieldOfView = _fovMin;
            }
            Camera.main.fieldOfView += _mouse.scroll.down.value * _zoomSensitivity;
            if (Camera.main.fieldOfView > _fovMax)
            {
                Camera.main.fieldOfView = _fovMax;
            }
        }

        if (_mouse.rightButton.isPressed)
        {
            RotateSubmissionMouse();
        }
        if (_keyboard.dKey.isPressed)
        {
            RotatePieceZ(-1);
        }
        if (_keyboard.aKey.isPressed)
        {
            RotatePieceZ(1);
        }
    }

    public void SetSubmission(Submission submission)
    {
        _submission = submission;
        _startRotation = _submission.transform.rotation;
        _startFOV = Camera.main.fieldOfView;
    }

    public void ClearSubmission()
    {
        _submission.transform.rotation = _startRotation;
        Camera.main.fieldOfView = _startFOV;
        _submission = null;
    }

    void CheckInput()
    {
        if (_submission != null)
        {
            if (_keyboard.rightArrowKey.IsPressed())
            {
                _submission.transform.Rotate(new Vector3(0, -_rotSpeed * 50 * Time.deltaTime, 0), Space.World);
            }
            if (_keyboard.leftArrowKey.IsPressed())
            {
                _submission.transform.Rotate(new Vector3(0, _rotSpeed * 50 * Time.deltaTime, 0), Space.World);
            }
            if (_keyboard.upArrowKey.IsPressed())
            {
                _submission.transform.Rotate(new Vector3(_rotSpeed * 50 * Time.deltaTime, 0, 0), Space.World);
            }
            if (_keyboard.downArrowKey.IsPressed())
            {
                _submission.transform.Rotate(new Vector3(-_rotSpeed * 50 * Time.deltaTime, 0, 0), Space.World);
            }
        }
    }

    private void RotateSubmissionMouse()
    {
        if (_submission != null)
        {
            float h = _rotSpeed * _mouse.delta.x.ReadValue();
            float v = _rotSpeed * _mouse.delta.y.ReadValue();

            _submission.transform.Rotate(Camera.main.transform.up, -h, Space.World);
            _submission.transform.Rotate(Camera.main.transform.right, v, Space.World);
        }
    }

    void RotatePieceZ(int dir)
    {
        if (_submission != null)
        {
            _submission.transform.Rotate(Camera.main.transform.forward, 50f * dir * Time.deltaTime, Space.World);
        }
    }
}
