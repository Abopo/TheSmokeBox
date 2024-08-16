using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Submission))]
public class DownloadedProject : MonoBehaviour
{
    private Submission _submission;

    private void Awake()
    {
        _submission = GetComponent<Submission>();
    }

    public void Init(int projectID)
    {
        WebServiceProjectManager.Instance.GetProjectFile(projectID, HandleDownloadSuccess, HandleDownloadFailure);
    }

    private void HandleDownloadSuccess(SubmissionData data)
    {
        _submission.LoadData(data);
    }

    private void HandleDownloadFailure(string message)
    {
        // display error for this entry
    }
}
