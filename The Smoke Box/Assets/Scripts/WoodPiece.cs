using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LerpTo))]
public class WoodPiece : MonoBehaviour {

    public Vector3 startPos;

    LerpTo _lerp;

    private void Awake() {
        startPos = transform.position;
        _lerp = GetComponent<LerpTo>();
    }
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnMouseDown() {
        if (!IsMouseOverUI()) {
            // Pick up this piece
            PickUp();
        }
    }

    private void OnMouseOver() {
        if (!IsMouseOverUI()) {
            // Show highlight outline?
        }
    }

    bool IsMouseOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }

    void PickUp() {
        // Tell the editor manager to pick us up
        EditManager.Instance.PickUpPiece(this);
    }

    public void Drop() {
        // Lerp to start position
        if(_lerp != null) {
            _lerp.LerpToPos(startPos, 0.5f);
        }
    }
}
