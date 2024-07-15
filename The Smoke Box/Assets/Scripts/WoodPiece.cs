using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPiece : Sliceable {

    Vector3 _startPos;

    LerpTo _lerp;

    // Start is called before the first frame update
    void Start() {
        _startPos = transform.position;
        _lerp = GetComponent<LerpTo>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnMouseDown() {
        // Pick up this piece
        PickUp();
    }

    private void OnMouseOver() {
        // Show highlight outline?
    }

    void PickUp() {
        // Tell the editor manager to pick us up
        EditManager.Instance.PickUpPiece(this);
    }

    public void Drop() {
        // Lerp to start position
        if(_lerp != null) {
            _lerp.LerpToPos(_startPos);
        }
    }
}
