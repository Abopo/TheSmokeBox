using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class AttachToPiece : MonoBehaviour
{
    public WoodPiece curPiece;
    public LayerMask _layerMask;
    [SerializeField]
    bool _alignNormals;
    [SerializeField]
    float _zDist;

    protected bool _isActive;

    Camera _mainCamera;

    RaycastHit _raycastHit;

    public UnityEvent OnAttach;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _mainCamera = Camera.main;
        _isActive = true;
    }

    // Update is called once per frame
    void Update() {
        if (_isActive) {
            Attach();

            if (Mouse.current.leftButton.wasPressedThisFrame) {
                ConfirmPlacement();
            }
        }
    }

    void Attach() {
        // Basically just going to go to the closest point to the mouse that's still on the face of the mesh of the piece

        if (HitPieceCheck()) {
            WoodPiece hitPiece = _raycastHit.collider.GetComponent<WoodPiece>();
            // Set this as our curPiece
            curPiece = hitPiece;
            // place self on the hit point
            transform.position = _raycastHit.point;
            if (_alignNormals) {
                // Also, align with the normal of the face we hit
                transform.rotation = Quaternion.LookRotation(_raycastHit.normal);
            }
        } else {
            FollowMouse();
        }

        //TODO: Still follow the mouse when it's not hitting anything somehow
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

    void FollowMouse() {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y, _zDist));
    }

    void ConfirmPlacement() {
        // Make sure we clicked on our piece
        if (HitPieceCheck() && curPiece != null) {
            OnAttach?.Invoke();
        }
    }
}
