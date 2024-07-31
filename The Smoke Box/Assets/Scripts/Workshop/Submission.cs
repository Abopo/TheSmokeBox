using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class Submission : MonoBehaviour {
    
    public Transform baseTransform;

    public bool hasBase;

    public UnityEvent OnAddedPiece = new UnityEvent();

    // Stats
    public int numPiecesUsed;
    public int numCutsUsed;
    public List<PAINTCOLOR> colorsUsed = new List<PAINTCOLOR>();
    // List of meshes used?

    SubmissionDataManager _submissionDataManager = new SubmissionDataManager();

    // Start is called before the first frame update
    void Start() {
        GetStats();
    }

    void GetStats() {
        WoodPiece[] wPieces = GetComponentsInChildren<WoodPiece>();

        numPiecesUsed = 0;
        numCutsUsed = 0;
        colorsUsed = new List<PAINTCOLOR>();

        foreach (WoodPiece piece in wPieces) {
            numPiecesUsed++;
            numCutsUsed += piece.numCuts;
            colorsUsed.Add(piece.paintColor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPieceAsBase(WoodPiece wPiece) {
        wPiece.transform.parent = baseTransform;
        wPiece.GoTo(transform.position);
        wPiece.isLocked = true;

        hasBase = true;
        OnAddedPiece.Invoke();
    }

    public void SaveData() {
        // Save the data
        _submissionDataManager.SaveSubmissionData(this);
    }

    public void LoadData(string path = "") {
        // Load the data
        _submissionDataManager.LoadSubmissionData(path);

        // Spawn wood pieces based on the loaded data
        WoodData[] woodData = _submissionDataManager.submissionData.woodDatas;
        Object woodPieceObj = Resources.Load("Prefabs/Workshop/WoodPiece");
        GameObject tempObject;
        for (int i = 0; i < woodData.Length; i++) {
            tempObject = Instantiate(woodPieceObj, transform) as GameObject;
            tempObject.transform.localPosition = woodData[i].pos;
            tempObject.transform.localRotation = Quaternion.Euler(woodData[i].rot);

            // Set the mesh
            Mesh mesh = new Mesh();
            mesh.vertices = woodData[i].meshData.verts;
            mesh.uv = woodData[i].meshData.uvs;
            mesh.normals = woodData[i].meshData.normals;
            mesh.triangles = woodData[i].meshData.tris;
            tempObject.GetComponent<MeshFilter>().mesh = null;
            tempObject.GetComponent<MeshFilter>().mesh = mesh;

            // Set the materials (sliced pieces need an extra material per slice)
            Material material = Resources.Load<Material>("Materials/Wood/" + woodData[i].material);
            Material[] materials = new Material[woodData[i].numMats];
            for (int j = 0; j < materials.Length; j++) {
                materials[j] = material;
            }
            tempObject.GetComponent<MeshRenderer>().materials = materials;
        }

        // Set rotation
        transform.localRotation = Quaternion.Euler(_submissionDataManager.submissionData.rotation);

        // Now that we've loaded our stuff, get our stats
        GetStats();
    }
}
