using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hanzzz.MeshSlicerFree;

public class SawTool : MonoBehaviour
{
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

    EditManager _editManager;

    private void Awake() {
        _editManager = FindObjectOfType<EditManager>();

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

    public void SlicePiece(GameObject wPiece) {
        _originalPiece = wPiece;
        Plane plane = new Plane(slicePlane.up, slicePlane.position);
        Slicer.SliceReturnValue sliceReturnValue;
        try {
            int triangleCount = wPiece.GetComponent<MeshFilter>().sharedMesh.triangles.Length;
            sliceReturnValue = slicer.Slice(wPiece, plane, intersectionMaterial);
        } catch {
            sliceReturnValue = null;
        }

        if (null == sliceReturnValue) {
            return;
        }

        sliceReturnValue.topGameObject.transform.position += topMoveDistance;
        sliceReturnValue.bottomGameObject.transform.position += bottomMoveDistance;
        _rightPiece = sliceReturnValue.topGameObject.AddComponent<WoodPiece>();
        _leftPiece = sliceReturnValue.bottomGameObject.AddComponent<WoodPiece>();

        // Copy over the start pos from the original piece
        _rightPiece.startPos = wPiece.GetComponent<WoodPiece>().startPos;
        _leftPiece.startPos = wPiece.GetComponent<WoodPiece>().startPos;

        // Move the mesh pivot point to the center of the object
        AdjustMeshPivotPoints(_rightPiece.gameObject);
        AdjustMeshPivotPoints(_leftPiece.gameObject);

        // Add colliders
        _rightPiece.gameObject.AddComponent<MeshCollider>();
        _leftPiece.gameObject.AddComponent<MeshCollider>();

        // Just hide the original piece until we decide to commit to the cut
        wPiece.SetActive(false);

        _uiCanvas.Activate();
    }

    private void AdjustMeshPivotPoints(GameObject piece) {
        //Get the vertices from the gameObject
        Vector3[] objectVerts = piece.GetComponent<MeshFilter>().mesh.vertices;
        //Initialize an offset
        Vector3 offset = Vector3.zero;
        //Loop through our vertices and add them to our offset
        for (int i = 0; i < objectVerts.Length; i++) {
            offset += objectVerts[i];
        }
        
        //Divide our offset by the amount of vertices in the gameObject (Getting the average)
        offset = offset / objectVerts.Length; 
        //Loop through our vertices and subtract the offset
        for (int i = 0; i < objectVerts.Length; i++) {
            
            objectVerts[i] -= offset;
        }

        //Assign the modified vertices to our gameObject
        piece.GetComponent<MeshFilter>().mesh.vertices = objectVerts;
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
