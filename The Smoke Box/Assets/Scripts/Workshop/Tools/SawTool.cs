using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Hanzzz.MeshSlicerFree;

public class SawTool : Tool {
    [SerializeField] private Transform slicePlane;
    [SerializeField] private Material intersectionMaterial;

    [SerializeField] private Vector3 topMoveDistance;
    [SerializeField] private Vector3 bottomMoveDistance;

    private static Slicer slicer;

    GameObject _originalPiece;
    WoodPiece _leftPiece;
    WoodPiece _rightPiece;

    [SerializeField]
    private SawCanvas _postCutCanvas;

    SawObject _sawObject;

    protected override void Awake() {
        base.Awake();

        if (slicer == null) {
            slicer = new Slicer();
        }

        _sawObject = GetComponentInChildren<SawObject>();
    }
    // Start is called before the first frame update
    void Start() {
    }

    void CreatePlane() {
        var filter = GetComponent<MeshFilter>();
        Vector3 normal = Vector3.zero;

        if (filter && filter.mesh.normals.Length > 0)
            normal = filter.transform.TransformDirection(filter.mesh.normals[0]);

        //_plane = new Plane(normal, transform.position);
    }

    // Update is called once per frame
    void Update() {
        // Scoot the slicing plane left/right
        if (Keyboard.current.qKey.isPressed) {
            if (slicePlane.transform.localPosition.x > -1.01f) {
                slicePlane.transform.Translate(-0.25f * Time.deltaTime, 0f, 0f, Space.World);
            }
        }
        if (Keyboard.current.eKey.isPressed) {
            if (slicePlane.transform.localPosition.x < 1.01f) {
                slicePlane.transform.Translate(0.25f * Time.deltaTime, 0f, 0f, Space.World);
            }
        }
    }

    public override void ActivateTool() {
        base.ActivateTool();

        slicePlane.GetChild(0).gameObject.SetActive(true);

        //slicePlane.gameObject.SetActive(true);

        gameObject.SetActive(true);

        _toolUI.SetActive(true);

        // Make sure slice plane is in the center
        slicePlane.transform.localPosition = new Vector3(0, 0, slicePlane.transform.localPosition.z);
    }

    public override void DeactivateTool() {
        base.DeactivateTool();

        gameObject.SetActive(false);

        _toolUI.SetActive(false);
    }

    public override void UseTool() {
        base.UseTool();

        // Set the intersection material to the piece's current material
        intersectionMaterial = _editManager.curPiece.GetComponent<MeshRenderer>().material;

        // Start the animation
        _sawObject.PlayAnimation();

        // Slice the piece in two
        bool success = SlicePiece(_editManager.curPiece);

        if (success) {
            // Hide the confirmation UI
            _toolUI.SetActive(false);

            // Hide the saw vizualizer
            slicePlane.GetChild(0).gameObject.SetActive(false);
        }
    }

    public bool SlicePiece(WoodPiece wPiece) {
        _originalPiece = wPiece.gameObject;
        Plane plane = new Plane(slicePlane.up, slicePlane.position);
        Slicer.SliceReturnValue sliceReturnValue;
        try {
            int triangleCount = wPiece.GetComponent<MeshFilter>().sharedMesh.triangles.Length;
            sliceReturnValue = slicer.Slice(wPiece.gameObject, plane, intersectionMaterial);
        } catch {
            sliceReturnValue = null;
        }

        if (null == sliceReturnValue) {
            return false;
        }

        sliceReturnValue.topGameObject.transform.position += topMoveDistance;
        sliceReturnValue.bottomGameObject.transform.position += bottomMoveDistance;
        _rightPiece = sliceReturnValue.topGameObject.GetComponent<WoodPiece>();
        _leftPiece = sliceReturnValue.bottomGameObject.GetComponent<WoodPiece>();

        // Copy over the data from the original piece
        _rightPiece.startPos = wPiece.startPos;
        _rightPiece.numCuts = wPiece.numCuts + 1;
        _leftPiece.startPos = wPiece.startPos;
        _leftPiece.numCuts = wPiece.numCuts + 1;

        // Update colliders
        _rightPiece.GetComponent<MeshCollider>().sharedMesh = null;
        _rightPiece.GetComponent<MeshCollider>().sharedMesh = _rightPiece.GetComponent<MeshFilter>().mesh;
        _leftPiece.GetComponent<MeshCollider>().sharedMesh = null;
        _leftPiece.GetComponent<MeshCollider>().sharedMesh = _leftPiece.GetComponent<MeshFilter>().mesh;

        // Move the mesh pivot point to the center of the object
        StartCoroutine(AdjustMeshPivotPoints(_rightPiece.gameObject));
        StartCoroutine(AdjustMeshPivotPoints(_leftPiece.gameObject));

        // Ignore collision between pieces so stuff doesn't get annoying later
        Physics.IgnoreCollision(_rightPiece.GetComponent<Collider>(), _leftPiece.GetComponent<Collider>());

        // Hide the new pieces until the saw animation is finished
        _leftPiece.gameObject.SetActive(false);
        _rightPiece.gameObject.SetActive(false);

        // Just hide the original piece until we decide to commit to the cut
        //wPiece.gameObject.SetActive(false);

        return true;
    }

    private IEnumerator AdjustMeshPivotPoints(GameObject piece) {
        // This doesn't work if the rotation isn't identity.
        // So, we should save the current rotation, set to identity, then reapply the rotation after.
        Quaternion curRotation = piece.transform.rotation;
        piece.transform.rotation = Quaternion.identity;
        // Maybe the huge scale is also messing it up?
        Vector3 curScale = piece.transform.localScale;
        piece.transform.localScale = new Vector3(1f, 1f, 1f);

        yield return new WaitForSecondsRealtime(0.1f);

        MeshCollider meshCollider = piece.GetComponent<MeshCollider>();
        Bounds meshBounds = meshCollider.bounds;
        Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
        //Determine the offset
        Vector3 offset = piece.transform.position - meshBounds.center;

        //Get the vertices from the gameObject
        Vector3[] objectVerts = mesh.vertices;
        //Loop through our vertices and add the offset
        for (int i = 0; i < objectVerts.Length; i++) {
            
            objectVerts[i] += offset;
        }
        //Assign the modified vertices to our gameObject
        mesh.vertices = objectVerts;

        yield return null;

        // Update the mesh collider
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;

        // Reapply rotation
        piece.transform.rotation = curRotation;
        // and scale
        piece.transform.localScale = curScale;
    }

    public void ShowCut() {
        // Show the new pieces
        _leftPiece.gameObject.SetActive(true);
        _rightPiece.gameObject.SetActive(true);

        // Hide the original piece until we decide to commit to the cut
        _originalPiece.gameObject.SetActive(false);

        // Show the post cut UI
        _postCutCanvas.Activate();
    }

    public void UndoCut() {
        // Delete both pieces
        Destroy(_rightPiece.gameObject);
        Destroy(_leftPiece.gameObject);

        // Restore original
        _originalPiece.SetActive(true);

        EndSlice();
    }

    public void KeepLeftPiece() {
        // Have the EditManager pick up the left piece
        _editManager.PickUpPiece(_leftPiece);

        // Tell the canvas to swap buttons
        _postCutCanvas.SwapButtons();
    }

    public void KeepRightPiece() {
        // Have the EditManager pick up the right piece
        _editManager.PickUpPiece(_rightPiece);

        // Tell the canvas to swap buttons
        _postCutCanvas.SwapButtons();
    }

    public void DropLeftPiece() {
        _leftPiece.Drop();

        // Both pieces should have been dealt with, so finish up
        EndSlice();
    }

    public void DropRightPiece() {
        _rightPiece.Drop();

        // Both pieces should have been dealt with, so finish up
        EndSlice();
    }

    public void DeleteLeftPiece() {
        Destroy(_leftPiece.gameObject);

        // Both pieces should have been dealt with, so finish up
        EndSlice();
    }

    public void DeleteRightPiece() {
        Destroy(_rightPiece.gameObject);

        // Both pieces should have been dealt with, so finish up
        EndSlice();
    }

    void EndSlice() {
        DeactivateTool();
        _postCutCanvas.Deactivate();
        _editManager.Activate();
    }
}
