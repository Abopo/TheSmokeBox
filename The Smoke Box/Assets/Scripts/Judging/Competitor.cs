using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum COMPETITOR { PLAYER = 0, JIMS, ALEX, FRANKY };

public class Competitor : MonoBehaviour {

    [SerializeField]
    COMPETITOR character; // Used to determine which submission to load

    [SerializeField]
    Transform _submissionPos;
    Submission _submission;

    [SerializeField]
    string[] _dialogues;
    DialogueBubble _dialogueBubble;

    private void Awake() {
        _submission = GetComponentInChildren<Submission>();
        _dialogueBubble = GetComponentInChildren<DialogueBubble>(true);
    }
    // Start is called before the first frame update
    void Start() {
        LoadSubmission();
    }

    void LoadSubmission() {
        if (character == COMPETITOR.PLAYER) {
            LoadPlayerSubmission();
        } else {
            LoadNpcSubmission();
        }
    }

    void LoadPlayerSubmission() {
        // Load our submission based on who we are, and what stage it is.
        string path = Application.persistentDataPath + "/PlayerSubmissionData" + GameManager.Instance.stage + ".json";
        _submission.LoadData(path);
    }

    void LoadNpcSubmission() {
        // NPC submissions will just be saved as prefabs ahead of time
        GameObject submissionObj = Resources.Load("Prefabs/Submissions/Submission" + character + GameManager.Instance.stage.ToString()) as GameObject;
        if (submissionObj != null) {
            GameObject submissionInstance = Instantiate(submissionObj);
            // Set the parent AFTER so the child keeps it's relative scale
            submissionInstance.transform.parent = _submissionPos;
            submissionInstance.transform.localPosition = Vector3.zero;
            _submission = submissionInstance.GetComponent<Submission>();
        }
    }

    void PositionSubmission() {
        // Move the submission until it rests on the table

        // If any part of the submission is currently touching the table, it needs to be moved upward until it isn't

        // If no part of the submission is currently touching the table, it needs to be moved downard until it is.

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue() {
        _dialogueBubble.ShowDialogues(_dialogues);
    }
}
