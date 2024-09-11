using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCanvas : MonoBehaviour
{
    [SerializeField]
    GameObject _toolsWindow;

    [SerializeField]
    GameObject _useAsBaseButton;

    [SerializeField]
    GameObject _holdUI;

    [SerializeField]
    GameObject _rotates;

    Submission _submission;

    private void Awake() {
        _submission = FindObjectOfType<Submission>();
    }
    // Start is called before the first frame update
    void Start() {
        _submission.OnAddedPiece.AddListener(OnAddedPiece);
        EditManager.OnPickedUpPiece.AddListener(OnPickedUpPiece);
        EditManager.OnDroppedPiece.AddListener(OnDroppedPiece);
        EditManager.OnLookUp.AddListener(OnLookUp);
        EditManager.OnLookDown.AddListener(OnLookDown);
    }

    // Update is called once per frame
    void Update() {

    }

    void OnPickedUpPiece() {
        if (EditManager.Instance.Active) {
            ShowBaseUI();
        }

        _rotates.SetActive(true);
        //_toolsWindow.SetActive(true);
    }

    void OnDroppedPiece() {
        HideBaseUI();

        _rotates.SetActive(false);
        //_toolsWindow.SetActive(false);
    }

    void OnAddedPiece() {
        if (_submission.numPiecesUsed > 1) {
            _useAsBaseButton.SetActive(false);
        }
        _holdUI.SetActive(false);
        //_toolsWindow.SetActive(false);
    }

    public void ShowBaseUI() {
        if (_submission.numPiecesUsed < 2) {
            _useAsBaseButton.SetActive(true);
        }

        _holdUI.SetActive(true);
    }

    public void HideBaseUI() {
        _useAsBaseButton.SetActive(false);
        _holdUI.SetActive(false);
    }

    public void OnLookUp() {

    }

    public void OnLookDown() {
        if (EditManager.Instance.Active && EditManager.Instance.HasPiece) {
            _holdUI.SetActive(true);
        }

        if (EditManager.Instance.HasPiece) {
            _rotates.SetActive(true);
        } else {
            _rotates.SetActive(false);
        }
    }
}
