using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Handles the process of submitting the submission
public class Submitter : MonoBehaviour {

    Submission _submission;

    [SerializeField]
    GameObject _submitButton;

    [SerializeField]
    GameObject _confirmStuff;

    [SerializeField]
    GameObject _titleStuff;

    RequirementTracker _requirementTracker;

    // Start is called before the first frame update
    void Start() {
        _submission = FindObjectOfType<Submission>();
        _requirementTracker = FindObjectOfType<RequirementTracker>();

        Submission.OnChanged.AddListener(OnSubmissionChanged);
        EditManager.OnLookUp.AddListener(OnLookUp);
        EditManager.OnLookDown.AddListener(OnLookDown);
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnSubmissionChanged() {
        CheckRequirementStatus();
    }

    void CheckRequirementStatus() {
        if (_requirementTracker != null && _requirementTracker.allRequirementsClear) {
            _submitButton.SetActive(true);
        } else {
            _submitButton.SetActive(false);
        }
    }

    void OnLookUp() {
        CheckRequirementStatus();
    }

    void OnLookDown() {
        _submitButton.SetActive(false);
    }

    public void BeginSubmissionProcess() {
        _submitButton.SetActive(false);
        _confirmStuff.SetActive(true);

        // Drop any pieces held by the edit manager
        //EditManager.Instance.DropPiece();
        // Deactivate the edit manager
        EditManager.Instance.Deactivate();

        // Ask player to select proper rotation

    }

    public void ShowTitleSubmission() {
        _submitButton.SetActive(false);
        _confirmStuff.SetActive(false);
        _titleStuff.SetActive(true);
    }

    public void SetTitle(string inTitle) {
        _submission.title = inTitle;
    }

    public void CancelSubmission() {
        _submitButton.SetActive(true);
        _confirmStuff.SetActive(false);
        _titleStuff.SetActive(false);
    }

    public void ConfirmSubmission() {
        // Hide all UI
        _submitButton.SetActive(false);
        _confirmStuff.SetActive(false);
        _titleStuff.SetActive(false);

        SaveSubmission();
    }

    void SaveSubmission() {
        // Run the save function of the submission
        _submission.SaveData();

        // Load judging scene
        GameManager.Instance.LoadScene("JudgingScene");
    }
}
