using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GalleryViewer : MonoBehaviour
{
    [SerializeField]
    float _rotSpeed;

    private Submission _submission;

    private Mouse _mouse;

    private Quaternion _startRotation;

    private void Start()
    {
        _mouse = Mouse.current;
    }

    private void Update()
    {
        if (_submission == null)
        {
            return;
        }

        if (_mouse.rightButton.isPressed)
        {
            RotateSubmissionMouse();
        }
    }

    public void SetSubmission(Submission submission)
    {
        _submission = submission;
        _startRotation = _submission.transform.rotation;
    }

    public void ClearSubmission()
    {
        _submission.transform.rotation = _startRotation;
        _submission = null;
    }

    private void RotateSubmissionMouse()
    {
        float h = _rotSpeed * _mouse.delta.x.ReadValue();
        float v = _rotSpeed * _mouse.delta.y.ReadValue();

        _submission.transform.Rotate(Camera.main.transform.up, -h, Space.World);
        _submission.transform.Rotate(Camera.main.transform.right, v, Space.World);
    }
}
