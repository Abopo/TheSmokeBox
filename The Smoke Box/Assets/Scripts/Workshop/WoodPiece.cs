using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(LerpTo))]
public class WoodPiece : MonoBehaviour {

    public Vector3 startPos;
    public bool isOnTable = true;
    public bool isLocked;

    LerpTo _lerp;
    Rigidbody _rigidbody;
    Collider _collider;

    float _restTimer = 0f;

    public int numCuts; // How many times this piece has been cut
    public PAINTCOLOR paintColor = PAINTCOLOR.WHITE;

    private void Awake() {
        startPos = transform.position;
        _lerp = GetComponent<LerpTo>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if(_rigidbody != null && !_rigidbody.isKinematic && isOnTable) {
            if(_rigidbody.velocity.magnitude < 0.1f) {
                _restTimer += Time.deltaTime;
                if (_restTimer > 1f) {
                    DisablePhysics();
                }
            } else {
                _restTimer = 0f;
            }
        }
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

    private void OnCollisionEnter(Collision collision) {
        if(collision.transform.tag == "Table") {
            isOnTable = true;
        }
    }

    public void SetMesh(Mesh mesh) {
        // Need to set the mesh filters shared mesh
        GetComponent<MeshFilter>().sharedMesh = mesh;

        // Need to update the mesh collider
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void PickUp() {
        if (!isLocked && EditManager.Instance.Active) {
            // Disable our physics in case we were still moving
            DisablePhysics();
            // Tell the editor manager to pick us up
            EditManager.Instance.PickUpPiece(this);
            isOnTable = false;
        }
    }

    public void Drop() {
        /*
        // Lerp to start position
        if(!isLocked && _lerp != null) {
            _lerp.LerpToPos(startPos, 0.5f);
            isOnTable = true;
        }
        */
        if(!isLocked) {
            // Toss the piece onto the table
            EnablePhysics();
            _rigidbody.AddForce(new Vector3(Random.Range(-500f, 500f), 0f, Random.Range(200f, 800f)));
        }
    }

    void EnablePhysics() {
        if(_collider == null) {
            _collider = GetComponent<Collider>();
        }
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
    }

    void DisablePhysics() {
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }

    public void GoTo(Vector3 pos) {
        // We're being forced to go somewhere so make sure our physics are off
        DisablePhysics();

        _lerp.LerpToPos(pos, 0.5f);
    }
}
