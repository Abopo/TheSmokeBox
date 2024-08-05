using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementTracker : MonoBehaviour {

    [SerializeField]
    List<SuperTextMesh> requirementTexts = new List<SuperTextMesh>();

    Submission _submission;

    public bool allRequirementsClear;

    private void Awake() {
        _submission = FindObjectOfType<Submission>();
    }
    // Start is called before the first frame update
    void Start() {
        Submission.OnChanged.AddListener(CheckSubmission);

        for(int i = 0; i < GameManager.Instance.stage; ++i) {
            requirementTexts[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void CheckSubmission() {
        _submission.GetStats();

        allRequirementsClear = true;

        requirementTexts[0].text = " - Joined Pieces " + _submission.numPiecesUsed + "/3";
        if (_submission.numPiecesUsed >= 3) {
            // Pieces requirement cleared
            requirementTexts[0].color = new Color(1, 0.94f, 0.2f);
        } else {
            allRequirementsClear = false;
        }

        if (GameManager.Instance.stage >= 2) {
            requirementTexts[1].text = " - Cut pieces " + _submission.numCutsUsed + "/1";
            if (_submission.numCutsUsed >= 1) {
                // Cuts requirement cleared
                requirementTexts[1].color = new Color(1, 0.94f, 0.2f);
            } else {
                allRequirementsClear = false;
            }
        }

        if (GameManager.Instance.stage >= 3) {
            int validColors = 0;
            foreach(PAINTCOLOR pc in _submission.colorsUsed) {
                if(pc != PAINTCOLOR.WHITE) {
                    validColors++;
                }
            }

            requirementTexts[2].text = " - Painted pieces " + validColors + "/3";
            if (validColors >= 3) {
                // Paints requirement cleared
                requirementTexts[2].color = new Color(1, 0.94f, 0.2f);
            } else {
                allRequirementsClear = false;
            }
        }

        SuperTextMesh.RebuildAll();
    }
}
