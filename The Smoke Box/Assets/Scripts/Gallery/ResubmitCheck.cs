using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResubmitCheck : MonoBehaviour
{
    [SerializeField] private Button _resubmitButton;
    [SerializeField] private TMPro.TMP_Text _errorText;

    [SerializeField] List<Submission> _loadedSubmissions = new List<Submission>();

    private List<Submission> _submissionsValidForUpload = new List<Submission>();

    private void Start()
    {
        _resubmitButton.onClick.AddListener(ResubmitClicked);
        CheckForSubmissions();
    }

    private void CheckForSubmissions()
    {
        WebServiceProjectManager.Instance.GetProjects(OnProjectsRecieved, OnGetProjectsFailed);
    }

    private void OnProjectsRecieved(List<Project> projects)
    {
        _submissionsValidForUpload.Clear();
        string username = PlayerPrefs.GetString("PlayerName");
        foreach (var submission in _loadedSubmissions)
        {
            if (projects.Find(q => q.Name == submission.title && q.OwnerName == username) == null)
            {
                _submissionsValidForUpload.Add(submission);
            }
        }
        
        if (_submissionsValidForUpload.Count > 0)
        {
            _resubmitButton.gameObject.SetActive(true);
        }
        else
        {
            _resubmitButton.gameObject.SetActive(false);
        }
    }

    private void OnGetProjectsFailed(string message)
    {

    }

    private void ResubmitClicked()
    {
        _resubmitButton.gameObject.SetActive(false);
        _errorText.gameObject.SetActive(false);
        foreach (var submission in _submissionsValidForUpload)
        {
            string playerName = PlayerPrefs.GetString("PlayerName");
            WebServiceProjectManager.Instance.UploadProjectFile(playerName, submission.title, submission.pathLoaded , OnSaveCompleted, OnSaveFailed);
        }
    }

    private void OnSaveCompleted(Project project)
    {
        CheckForSubmissions();
    }

    private void OnSaveFailed(string message)
    {
        _errorText.text = message;
        _errorText.gameObject.SetActive(true);
        Debug.Log(message);
    }
}
