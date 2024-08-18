using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class Submission : MonoBehaviour {

    public string title;
    public Transform baseTransform;

    public bool hasBase;

    public UnityEvent OnAddedPiece = new UnityEvent();
    public static UnityEvent OnChanged = new UnityEvent();

    // Stats
    public int numPiecesUsed;
    public int numCutsUsed;
    public List<PAINTCOLOR> colorsUsed = new List<PAINTCOLOR>();
    public List<string> pieceNames = new List<string>();
    // List of meshes used?
    private List<GameObject> pieces = new List<GameObject>();

    SubmissionDataManager _submissionDataManager = new SubmissionDataManager();

    // Start is called before the first frame update
    void Start() {
        GetStats();
    }

    public void GetStats() {
        WoodPiece[] wPieces = GetComponentsInChildren<WoodPiece>();

        numPiecesUsed = -1; // Start at -1 cuz the base is technically a piece but shouldn't count
        numCutsUsed = 0;
        colorsUsed = new List<PAINTCOLOR>();

        foreach (WoodPiece piece in wPieces) {
            numPiecesUsed++;
            numCutsUsed += piece.numCuts;
            colorsUsed.Add(piece.paintColor);
            pieceNames.Add(piece.pieceName);
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
        OnChanged.Invoke();
    }

    public void SaveData(Action<Project> OnSaveCompleted, Action<string> OnSaveFailed) {
        // Save the data
        _submissionDataManager.SaveSubmissionData(this, OnSaveCompleted, OnSaveFailed);
    }

    public void LoadData(string path = "") {
        // Load the data
        _submissionDataManager.LoadSubmissionData(path);
        LoadFromSubmissionDataManager();
    }

    public void LoadData(SubmissionData data)
    {
        _submissionDataManager.submissionData = data;
        LoadFromSubmissionDataManager();
    }

    private void LoadFromSubmissionDataManager()
    {
        // clear old pieces
        foreach (var piece in pieces)
        {
            Destroy(piece.gameObject);
        }
        pieces.Clear();

        // Spawn wood pieces based on the loaded data
        WoodData[] woodData = _submissionDataManager.submissionData.woodDatas;
        UnityEngine.Object woodPieceObj = Resources.Load("Prefabs/Workshop/WoodPiece");
        GameObject tempPiece;
        WoodPiece tempWood;
        for (int i = 0; i < woodData.Length; i++)
        {
            tempPiece = Instantiate(woodPieceObj, transform) as GameObject;
            tempPiece.transform.localPosition = woodData[i].pos;
            tempPiece.transform.localRotation = Quaternion.Euler(woodData[i].rot);
            pieces.Add(tempPiece);

            // Set the mesh
            Mesh mesh = new Mesh();
            mesh.vertices = woodData[i].meshData.verts;
            mesh.uv = woodData[i].meshData.uvs;
            mesh.normals = woodData[i].meshData.normals;
            mesh.triangles = woodData[i].meshData.tris;

            tempWood = tempPiece.GetComponent<WoodPiece>();
            tempWood.SetMesh(mesh);
            tempWood.numCuts = woodData[i].numCuts;
            tempWood.pieceName = woodData[i].pieceName;
            tempWood.paintColor = woodData[i].color;

            // Destroy the rigidbody since we don't need it
            DestroyImmediate(tempWood.GetComponent<Rigidbody>());
            // We don't need any woodpiece functionality happening
            tempWood.enabled = false;

            // Set the materials (sliced pieces need an extra material per slice)
            Material material = Resources.Load<Material>("Materials/Wood/" + woodData[i].material);
            Material[] materials = new Material[woodData[i].numMats];
            for (int j = 0; j < materials.Length; j++)
            {
                materials[j] = material;
            }
            tempPiece.GetComponent<MeshRenderer>().materials = materials;
        }

        // Set rotation
        transform.localRotation = Quaternion.Euler(_submissionDataManager.submissionData.rotation);

        // Set title
        title = _submissionDataManager.submissionData.title;

        // Now that we've loaded our stuff, get our stats
        GetStats();
    }
}
