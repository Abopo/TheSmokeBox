using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(LerpTo))]
public class WoodPiece : MonoBehaviour {

    public bool isOnTable;
    public bool isLocked;

    public LerpTo lerp;
    Rigidbody _rigidbody;
    MeshCollider _collider;

    float _restTimer = 0f;

    public ShopItemData _shopData;

    // stats
    public int numCuts; // How many times this piece has been cut
    public PAINTCOLOR paintColor = PAINTCOLOR.WHITE;
    public List<WoodPiece> attachedPieces = new List<WoodPiece>(); // How many other pieces are attached to this piece
    public string pieceName;

    WoodSFX _woodSFX;

    public ShopItemData Data { get => _shopData; set => SetData(value); }

    public Material Material {
        get {
            return GetComponent<MeshRenderer>().material;
        }
        set {
            GetComponent<MeshRenderer>().material = value;
        }
    }


    private void Awake() {
        lerp = GetComponent<LerpTo>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<MeshCollider>();
        _woodSFX = FindFirstObjectByType<WoodSFX>();
    }
    // Start is called before the first frame update
    void Start() {
        if(_shopData == null) {
            _shopData = ScriptableObject.CreateInstance<ShopItemData>();
        }

        Initialize();
    }

    public void MakeDebugData() {
        if (_shopData != null) {
            _shopData.price = Random.Range(1, 5);
        }
    }

    void Initialize() {
        // If the material has Trans in it, it's already been set (probably by the joint tool)
        if (!Material.name.Contains("Trans")) {
            // Set material to the wood type
            switch (_shopData.type) {
                case WOOD_TYPE.ACACIA:
                    Material = Resources.Load<Material>("Materials/Wood/Wood_Acacia");
                    break;
                case WOOD_TYPE.ASH:
                    Material = Resources.Load<Material>("Materials/Wood/Wood_Ash");
                    break;
                case WOOD_TYPE.BEECH:
                    Material = Resources.Load<Material>("Materials/Wood/Wood_Beech");
                    break;
                case WOOD_TYPE.OAK:
                    Material = Resources.Load<Material>("Materials/Wood/Wood_Oak");
                    break;
                case WOOD_TYPE.SPRUCE:
                    Material = Resources.Load<Material>("Materials/Wood/Wood_Spruce");
                    break;
                case WOOD_TYPE.WALNUT:
                    Material = Resources.Load<Material>("Materials/Wood/Wood_Walnut");
                    break;
            }
        }

        if (GetComponent<Slice>() != null) {
            GetComponent<Slice>().sliceOptions.insideMaterial = Material;
        }
    }

    // Update is called once per frame
    void Update() {
        if(_rigidbody != null && !_rigidbody.isKinematic && isOnTable) {
            if(_rigidbody.linearVelocity.magnitude < 0.1f) {
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

        if (_woodSFX != null) {
            _woodSFX.PlayWoodCollisionSFX();
        }
    }

    public void SetData(ShopItemData wData) {
        _shopData = wData;

        SetMesh(wData.mesh);

        pieceName = wData.itemName;
    }

    public void SetMesh(Mesh mesh) {
        // Need to set the mesh filters shared mesh
        GetComponent<MeshFilter>().sharedMesh = mesh;

        // Need to update the mesh collider
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void PickUp() {
        if (!isLocked && EditManager.Instance != null && EditManager.Instance.Active) {
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
            _collider = GetComponent<MeshCollider>();
        }
        _collider.enabled = true;
        // Since we're turning off kinematic, we also need to set the collider to convex
        _collider.convex = true;

        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
    }

    void DisablePhysics() {
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;

        // At this point we're no longer moving and can set our collider to non-convex
        _collider.convex = false;
    }

    public void GoTo(Vector3 pos) {
        // We're being forced to go somewhere so make sure our physics are off
        DisablePhysics();

        lerp.LerpToPos(pos, 0.5f);
    }

    // Take in a wood piece and copy all it's data
    public void CopyWoodData(WoodPiece originalPiece) {
        gameObject.layer = originalPiece.gameObject.layer;
        gameObject.layer = originalPiece.gameObject.layer;

        _shopData = originalPiece.Data;
        numCuts = originalPiece.numCuts;
        paintColor = originalPiece.paintColor;
        pieceName = originalPiece.pieceName;
    }
}
