using UnityEngine;
using UnityEngine.Events;

public class WedgeTool : Tool
{
    WedgeObject _chisel;
    WoodPiece _wPiece;

    public UnityEvent WedgeToolUsed = new UnityEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _chisel = GetComponentInChildren<WedgeObject>(true);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public override void UseTool() {
        base.UseTool();

        _wPiece = _chisel.curPiece;
        // Check that the piece only has 1 attach point.
        if (_wPiece.attachedPieces.Count <= 1) {
            // Full disable all edit controls
            _editManager.DisableRotation();

            // Play tool animation
            _chisel.PlayAnimation();
        }
    }

    public void RemovePiece() {
        // Remove piece from parent
        _wPiece.transform.parent = null;
        _wPiece.isLocked = false;
        _wPiece.Drop();

        // Reduce attachment counts
        foreach (WoodPiece wPiece in _wPiece.attachedPieces) {
            wPiece.attachedPieces.Remove(_wPiece);
        }
        _wPiece.attachedPieces.Clear();

        Submission.OnChanged.Invoke();
        WedgeToolUsed.Invoke();

        _editManager.EnableRotation();
    }
}
