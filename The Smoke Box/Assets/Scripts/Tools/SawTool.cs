using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private SawCanvas _uiCanvas;


    protected override void Awake() {
        base.Awake();

        if (slicer == null) {
            slicer = new Slicer();
        }
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

    }

    public override void ActivateTool() {
        base.ActivateTool();

        gameObject.SetActive(true);
    }

    public override void DeactivateTool() {
        base.DeactivateTool();

        gameObject.SetActive(false);
    }

    public override void UseTool() {
        base.UseTool();

        SlicePiece(_editManager.curPiece);
    }

    public void SlicePiece(WoodPiece wPiece) {
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
            return;
        }

        sliceReturnValue.topGameObject.transform.position += topMoveDistance;
        sliceReturnValue.bottomGameObject.transform.position += bottomMoveDistance;
        _rightPiece = sliceReturnValue.topGameObject.GetComponent<WoodPiece>();
        _leftPiece = sliceReturnValue.bottomGameObject.GetComponent<WoodPiece>();

        // Copy over the start pos from the original piece
        _rightPiece.startPos = wPiece.GetComponent<WoodPiece>().startPos;
        _leftPiece.startPos = wPiece.GetComponent<WoodPiece>().startPos;

        // Update colliders
        _rightPiece.GetComponent<MeshCollider>().sharedMesh = null;
        _rightPiece.GetComponent<MeshCollider>().sharedMesh = _rightPiece.GetComponent<MeshFilter>().mesh;
        _leftPiece.GetComponent<MeshCollider>().sharedMesh = null;
        _leftPiece.GetComponent<MeshCollider>().sharedMesh = _leftPiece.GetComponent<MeshFilter>().mesh;

        // Move the mesh pivot point to the center of the object
        StartCoroutine(AdjustMeshPivotPoints(_rightPiece.gameObject));
        StartCoroutine(AdjustMeshPivotPoints(_leftPiece.gameObject));

        // Just hide the original piece until we decide to commit to the cut
        wPiece.gameObject.SetActive(false);

        _uiCanvas.Activate();
    }

    private IEnumerator AdjustMeshPivotPoints(GameObject piece) {
        // This doesn't work if the rotation isn't identity.
        // So, we should save the current rotation, set to identity, then reapply the rotation after.
        Quaternion curRotation = piece.transform.rotation;
        piece.transform.rotation = Quaternion.identity;

        yield return null;

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
        _uiCanvas.SwapButtons();
    }

    public void KeepRightPiece() {
        // Have the EditManager pick up the right piece
        _editManager.PickUpPiece(_rightPiece);

        // Tell the canvas to swap buttons
        _uiCanvas.SwapButtons();
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
        _uiCanvas.Deactivate();
        _editManager.Activate();
    }
}
