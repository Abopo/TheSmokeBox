using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCanvas : MonoBehaviour {

    [SerializeField]
    GameObject _useAsBaseButton;

    [SerializeField]
    GameObject _toolsWindow;

    [SerializeField]
    GameObject _holdUI;

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

        _toolsWindow.SetActive(true);
    }

    void OnDroppedPiece() {
        HideBaseUI();
        _toolsWindow.SetActive(false);
    }

    void OnAddedPiece() {
        if(_submission.hasBase) {
            _useAsBaseButton.SetActive(false);
        }
        _holdUI.SetActive(false);
        _toolsWindow.SetActive(false);
    }

    public void ShowBaseUI() {
        if (!_submission.hasBase) {
            _useAsBaseButton.SetActive(true);
        }

        _holdUI.SetActive(true);
    }

    public void HideBaseUI() {
        _useAsBaseButton.SetActive(false);
        _holdUI.SetActive(false);
    }
}
