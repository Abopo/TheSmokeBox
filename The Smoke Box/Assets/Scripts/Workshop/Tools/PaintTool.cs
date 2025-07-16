using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[Serializable]
public enum PAINTCOLOR { NONE = 0, RED, GREEN, BLUE, YELLOW, PINK, PURPLE, ORANGE, CYAN, WHITE, BLACK };

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

        SetPaint(PAINTCOLOR.NONE);
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

    public void SetPaint(PaintColor color) {
        SetPaint(color.paintColor);
    }
    public void SetPaint(PAINTCOLOR color) {
        _paintColor = color;

        if (_paintColor == PAINTCOLOR.NONE) {
            _paintPropertyBlock.SetFloat("_Colorize", 0.0f);
            _paintBrush.SetPaint(_paintPropertyBlock);

            return;
        }

        switch (_paintColor) {
            case PAINTCOLOR.RED:
                _colorToPaint = new Color(0.72f, 0.1f, 0.1f);
                break;
            case PAINTCOLOR.GREEN:
                _colorToPaint = new Color(0f, 0.6f, 0f);
                break;
            case PAINTCOLOR.BLUE:
                _colorToPaint = new Color(0.1f, 0.15f, 0.8f);
                break;
            case PAINTCOLOR.YELLOW:
                _colorToPaint = new Color(0.75f, 0.75f, 0f);
                break;
            case PAINTCOLOR.PINK:
                _colorToPaint = new Color(0.75f, 0.35f, 0.75f);
                break;
            case PAINTCOLOR.PURPLE:
                _colorToPaint = new Color(0.45f, 0.2f, 0.75f);
                break;
            case PAINTCOLOR.ORANGE:
                _colorToPaint = new Color(0.85f, 0.3f, 0f);
                break;
            case PAINTCOLOR.CYAN:
                _colorToPaint = new Color(0f, 0.75f, 0.836f);
                break;
            case PAINTCOLOR.WHITE:
                _colorToPaint = new Color(0.75f, 0.75f, 0.75f);
                break;
            case PAINTCOLOR.BLACK:
                _colorToPaint = new Color(0.15f, 0.15f, 0.15f);
                break;
        }

        _paintPropertyBlock.SetColor("_Color", _colorToPaint);
        _paintPropertyBlock.SetFloat("_Colorize", 1.0f);
        _paintBrush.SetPaint(_paintPropertyBlock);
    }

    /*
    public void SetPaint(Material paintMat) {

        if(paintMat == null) {
            _paintPropertyBlock.SetFloat("Colorize", 0.0f);
            _paintBrush.SetPaint(_paintPropertyBlock);

            return;
        }

        _paintMaterial = paintMat;

        if (paintMat.name.Contains("White")) {
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
        _paintPropertyBlock.SetFloat("Colorize", 1.0f);
        _paintBrush.SetPaint(_paintPropertyBlock);
    }
    */

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
