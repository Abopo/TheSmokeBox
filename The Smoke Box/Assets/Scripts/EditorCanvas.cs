using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCanvas : MonoBehaviour {

    [SerializeField]
    GameObject _useAsBaseButton;

    [SerializeField]
    GameObject _dropButton;

    Submission _submission;

    private void Awake() {
        _submission = FindObjectOfType<Submission>();
    }
    // Start is called before the first frame update
    void Start() {
        _submission.OnAddedPiece.AddListener(OnAddedPiece);
        EditManager.OnPickedUpPiece.AddListener(OnPickedUpPiece);
        EditManager.OnDroppedPiece.AddListener(OnDroppedPiece);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPickedUpPiece() {
        if (EditManager.Instance.Active) {
            ShowBaseUI();
        }
    }

    void OnDroppedPiece() {
        HideBaseUI();
    }

    void OnAddedPiece() {
        if(_submission.hasBase) {
            _useAsBaseButton.SetActive(false);
        }
        _dropButton.SetActive(false);
    }

    public void ShowBaseUI() {
        if (!_submission.hasBase) {
            _useAsBaseButton.SetActive(true);
        }

        _dropButton.SetActive(true);
    }

    public void HideBaseUI() {
        _useAsBaseButton.SetActive(false);
        _dropButton.SetActive(false);
    }
}
