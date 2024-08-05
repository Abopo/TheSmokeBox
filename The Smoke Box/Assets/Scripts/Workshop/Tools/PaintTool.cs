using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PAINTCOLOR { WHITE = 0, RED, GREEN, BLUE, YELLOW, PINK, PURPLE, ORANGE, CYAN, BLACK };

public class PaintTool : Tool {

    public LayerMask _layerMask;

    Material _paintMaterial;
    PAINTCOLOR _paintColor;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame) {
            UseTool();
        }
    }

    public override void ActivateTool() {
        base.ActivateTool();

        _toolUI.SetActive(true);

        _editManager.Deactivate();
    }

    public override void DeactivateTool() {
        base.DeactivateTool();

        _toolUI.SetActive(false);

        _editManager.Activate();
    }

    public void SetPaint(Material paintMat) {
        _paintMaterial = paintMat;

        if(paintMat.name.Contains("White")) {
            _paintColor = PAINTCOLOR.WHITE;
        } else if (paintMat.name.Contains("Red")) {
            _paintColor = PAINTCOLOR.RED;
        } else if (paintMat.name.Contains("Green")) {
            _paintColor = PAINTCOLOR.GREEN;
        } else if (paintMat.name.Contains("Blue")) {
            _paintColor = PAINTCOLOR.BLUE;
        } else if (paintMat.name.Contains("Yellow")) {
            _paintColor = PAINTCOLOR.YELLOW;
        } else if (paintMat.name.Contains("Pink")) {
            _paintColor = PAINTCOLOR.PINK;
        } else if (paintMat.name.Contains("Purple")) {
            _paintColor = PAINTCOLOR.PURPLE;
        } else if (paintMat.name.Contains("Orange")) {
            _paintColor = PAINTCOLOR.ORANGE;
        } else if (paintMat.name.Contains("Cyan")) {
            _paintColor = PAINTCOLOR.CYAN;
        } else if (paintMat.name.Contains("Black")) {
            _paintColor = PAINTCOLOR.BLACK;
        }
    }

    public override void UseTool() {
        base.UseTool();

        // Cast a ray from the mouse to the wood piece
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, 100f, _layerMask);

        // If we hit a wood piece, 
        if (hitInfo.collider != null) {
            // and it's not on the table
            WoodPiece wPiece = hitInfo.collider.GetComponent<WoodPiece>();
            if (!wPiece.isOnTable) {
                PaintPiece(wPiece);
            }
        }
    }

    void PaintPiece(WoodPiece wPiece) {
        // Set the editors curPiece materials to the paint material
        Material[] mats = wPiece.GetComponent<MeshRenderer>().materials;
        for (int i = 0; i < mats.Length; i++) {
            mats[i] = _paintMaterial;
        }
        wPiece.GetComponent<MeshRenderer>().materials = mats;

        wPiece.paintColor = _paintColor;

        Submission.OnChanged.Invoke();

        // TODO: Some nice paint splash effect
    }
}
