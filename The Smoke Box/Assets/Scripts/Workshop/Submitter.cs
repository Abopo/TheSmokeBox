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

    // Start is called before the first frame update
    void Start() {
        _submission = FindObjectOfType<Submission>();
    }

    // Update is called once per frame
    void Update() {
        
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

    public void CancelSubmission() {
        _submitButton.SetActive(true);
        _confirmStuff.SetActive(false);
    }

    public void ConfirmSubmission() {
        // Hide all UI
        _submitButton.SetActive(false);
        _confirmStuff.SetActive(false);

        SaveSubmission();
    }

    void SaveSubmission() {
        // Save the current submission as a prefab
        string savePath = "Assets/Resources/Prefabs/Submissions/Submission" + GameManager.Instance.stage.ToString() + ".prefab";

        //savePath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAsset(_submission.gameObject, savePath, out prefabSuccess);
        if (prefabSuccess == true)
            Debug.Log("Submission was saved successfully");
        else
            Debug.Log("Submission failed to save" + prefabSuccess);

        // Load judging scene

    }
}
