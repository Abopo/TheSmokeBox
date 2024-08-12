using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsWindow : MonoBehaviour {

    ToolButton[] _toolButtons;

    private void Awake() {
        _toolButtons = GetComponentsInChildren<ToolButton>();
    }
    // Start is called before the first frame update
    void Start()
    {
        // Disable all buttons by default
        for (int i = 0; i < _toolButtons.Length; i++) {
            _toolButtons[i].DisableButton();
            if (GameManager.Instance.stage <= i) {
                _toolButtons[i].gameObject.SetActive(false);
            }
        }

        EditManager.OnPickedUpPiece.AddListener(OnPickedUpPiece);
        EditManager.OnDroppedPiece.AddListener(OnDroppedPiece);
        EditManager.OnLookUp.AddListener(OnLookUp);
        EditManager.OnLookDown.AddListener(OnLookDown);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnableButtons() {
        foreach (ToolButton toolButton in _toolButtons) {
            toolButton.EnableButton();
        }
    }

    void DisableButtons() {
        foreach (ToolButton toolButton in _toolButtons) {
            toolButton.DisableButton();
        }
    }

    void OnPickedUpPiece() {
        EnableButtons();
    }

    void OnDroppedPiece() {
        DisableButtons();
    }

    void OnLookUp() {
        foreach (ToolButton toolButton in _toolButtons) {
            if (toolButton.canBeUsedOnSubmission) {
                toolButton.EnableButton();
            } else {
                toolButton.DisableButton();
            }
        }
    }

    void OnLookDown() {
        if (EditManager.Instance.HasPiece) {
            EnableButtons();
        } else {
            DisableButtons();
        }
    }
}
