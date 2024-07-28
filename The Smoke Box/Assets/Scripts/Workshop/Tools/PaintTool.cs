using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaintTool : Tool {

    public LayerMask _layerMask;

    Material _paintMaterial;

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
    }

    public override void DeactivateTool() {
        base.DeactivateTool();

        _toolUI.SetActive(false);

        _editManager.Activate();
    }

    public void SetPaint(Material paintMat) {
        _paintMaterial = paintMat;
    }

    public override void UseTool() {
        base.UseTool();

        // Cast a ray from the mouse to the wood piece
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, 100f, _layerMask);

        // If we hit a wood piece, 
        if (hitInfo.collider != null) {
            // and it's not on the table
            if (!hitInfo.collider.GetComponent<WoodPiece>().isOnTable) {
                PaintPiece();
            }
        }
    }

    void PaintPiece() {
        // Set the editors curPiece material to the paint material
        _editManager.curPiece.GetComponent<MeshRenderer>().material = _paintMaterial;

        // TODO: Some nice paint splash effect
    }
}
