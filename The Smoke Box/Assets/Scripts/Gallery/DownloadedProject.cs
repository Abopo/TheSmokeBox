using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Submission))]
public class DownloadedProject : MonoBehaviour
{
    private Submission _submission;

    private float _speed = 15f;
    private Vector3 _startPoint;
    private Vector3 targetPos;
    private bool isMoving = false;

    private void Awake()
    {
        _submission = GetComponent<Submission>();
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.001f)
            {
                transform.position = targetPos;
                isMoving = false;
            }
        }
    }

    public void Init(int projectID)
    {
        WebServiceProjectManager.Instance.GetProjectFile(projectID, HandleDownloadSuccess, HandleDownloadFailure);
    }

    public void MoveToPoint(Vector3 point)
    {
        _startPoint = transform.position;
        targetPos = point;
        isMoving = true;
    }

    public void ReturnToPoint()
    {
        targetPos = _startPoint;
        isMoving = true;
    }

    private void HandleDownloadSuccess(SubmissionData data)
    {
        _submission.LoadData(data);
    }

    private void HandleDownloadFailure(string message)
    {
        Debug.Log(message);
    }
}