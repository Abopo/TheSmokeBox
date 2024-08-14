using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JointNode : MonoBehaviour {

    public WoodPiece curPiece;

    public LayerMask _layerMask;

    public bool isActive = false;

    Camera _mainCamera;

    RaycastHit _raycastHit;

    private void Awake() {
        _mainCamera = Camera.main;
    }
    // Start is called before the first frame update
    void Start() {
        isActive = false;
    }

    // Update is called once per frame
    void Update() {
        if (isActive) {
            AttachToPiece();

            if (Mouse.current.leftButton.wasPressedThisFrame) {
                ConfirmPlacement();
            }
        }
    }

    public void Activate() {
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(true);
        StartCoroutine(ActivateLater());
    }

    // Activate 1 frame later to avoid input overflow
    IEnumerator ActivateLater() {
        yield return null;
        isActive = true;
    }

    public void Deactivate() {
        gameObject.SetActive(false);
        isActive = false;
    }

    void AttachToPiece() {
        // Basically just going to go to the closest point to the mouse that's still on the face of the mesh of the piece

        if (HitPieceCheck()) {
            // place self on the hit point
            transform.position = _raycastHit.point;
            // Also, align with the normal of the face we hit
            transform.rotation = Quaternion.LookRotation(_raycastHit.normal);
            // Set this as our curPiece
            curPiece = _raycastHit.collider.GetComponent<WoodPiece>();
        }

        //TODO: Still follow the mouse when it's not hitting anything somehow
    }

    void ConfirmPlacement() {
        // Make sure we clicked on our piece
        if (HitPieceCheck()) {
            GetComponentInParent<JointTool>().ApplyJoint();
        }
    }

    bool HitPieceCheck() {
        // Cast a ray from the mouse to the wood piece
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out _raycastHit, 100f, _layerMask);

        //Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);

        // If we hit a wood piece, 
        if (_raycastHit.collider != null) {
            // and it's not on the table
            if (!_raycastHit.collider.GetComponent<WoodPiece>().isOnTable) {
                return true;
            } 
        }

        return false;
    }

    public void ParentPiece() {
        curPiece.transform.parent = transform;
    }

    public void UnParentPiece() {
        curPiece.transform.parent = null;
        // Make sure piece is at the proper position
        curPiece.transform.position = EditManager.Instance.transform.position;
    }
}
