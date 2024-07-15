using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grabbable : MonoBehaviour {

    Mouse _mouse;

    Vector3 _mOffset;
    float _zCoord;

    // Start is called before the first frame update
    void Start() {
        _mouse = Mouse.current;
    }

    // Update is called once per frame
    void Update() {
        if(_mouse.leftButton.isPressed) {
            // Is it over us?
        }
    }

    private void OnMouseDown() {
        _zCoord = Camera.main.WorldToScreenPoint(transform.position).z;

        // Store the offset
        _mOffset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag() {
        transform.position = GetMouseWorldPos() + _mOffset;
    }

    Vector3 GetMouseWorldPos() {
        Vector3 mousePos = _mouse.position.ReadValue();

        mousePos.z = _zCoord;

        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
