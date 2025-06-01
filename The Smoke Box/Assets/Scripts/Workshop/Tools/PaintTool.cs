using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public enum PAINTCOLOR { WHITE = 0, RED, GREEN, BLUE, YELLOW, PINK, PURPLE, ORANGE, CYAN, BLACK };

public class PaintTool : Tool {

    public LayerMask _layerMask;

    [SerializeField]
    Material _paintMaterial;
    PAINTCOLOR _paintColor;
    Color _colorToPaint;
    MaterialPropertyBlock _paintPropertyBlock;

    PaintBrush _paintBrush;
    public UnityEvent PaintToolUsed = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        _paintBrush = GetComponentInChildren<PaintBrush>();
        _paintPropertyBlock = new MaterialPropertyBlock();

        SetPaint(_paintMaterial);
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

        _editManager.Deactivate(false);
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
            _colorToPaint = Color.white;
        } else if (paintMat.name.Contains("Red")) {
            _paintColor = PAINTCOLOR.RED;
            _colorToPaint = Color.red;
        } else if (paintMat.name.Contains("Green")) {
            _paintColor = PAINTCOLOR.GREEN;
            _colorToPaint = Color.green;
        } else if (paintMat.name.Contains("Blue")) {
            _paintColor = PAINTCOLOR.BLUE;
            _colorToPaint = Color.blue;
        } else if (paintMat.name.Contains("Yellow")) {
            _paintColor = PAINTCOLOR.YELLOW;
            _colorToPaint = Color.yellow;
        } else if (paintMat.name.Contains("Pink")) {
            _paintColor = PAINTCOLOR.PINK;
            _colorToPaint = new Color(1f, 0.36f, 0.98f);
        } else if (paintMat.name.Contains("Purple")) {
            _paintColor = PAINTCOLOR.PURPLE;
            _colorToPaint = new Color(0.45f, 0.17f, 0.93f);
        } else if (paintMat.name.Contains("Orange")) {
            _paintColor = PAINTCOLOR.ORANGE;
            _colorToPaint = new Color(0.93f, 0.69f, 0.08f);
        } else if (paintMat.name.Contains("Cyan")) {
            _paintColor = PAINTCOLOR.CYAN;
            _colorToPaint = Color.cyan;
        } else if (paintMat.name.Contains("Black")) {
            _paintColor = PAINTCOLOR.BLACK;
            _colorToPaint = Color.black;
        }

        _paintPropertyBlock.SetColor("_Color", _colorToPaint);
        _paintBrush.SetPaint(_paintPropertyBlock);
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
                PaintPiece(wPiece, hitInfo.point);
            }
        }
    }

    void PaintPiece(WoodPiece wPiece, Vector3 paintPos) {
        // Set the editors curPiece materials to the paint material
        //Material[] mats = wPiece.GetComponent<MeshRenderer>().materials;
        //for (int i = 0; i < mats.Length; i++) {
        //    mats[i] = _paintMaterial;
        //}
        //wPiece.GetComponent<MeshRenderer>().materials = mats;
        wPiece.GetComponent<MeshRenderer>().SetPropertyBlock(_paintPropertyBlock);

        wPiece.paintColor = _paintColor;

        Submission.OnChanged.Invoke();
        PaintToolUsed.Invoke();

        _paintBrush.Paint(paintPos);
    }
}
