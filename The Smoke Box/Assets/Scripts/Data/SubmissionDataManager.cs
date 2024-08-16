using System;
using UnityEngine;

[Serializable]
public class MeshData {
    public Vector3[] verts;
    public Vector3[] normals;
    public Vector2[] uvs;
    public int[] tris;
}

[Serializable]
public class WoodData {
    public Vector3 pos;
    public Vector3 rot;
    public MeshData meshData;
    public string material;
    public int numMats;
    public int numCuts;
    public string pieceName;
    public PAINTCOLOR color;
}    

[Serializable]
public class SubmissionData
{
    public WoodData[] woodDatas;
    public Vector3 rotation;
    public string title;
}

public class SubmissionDataManager {
    public SubmissionData submissionData = new SubmissionData();

    public void SaveSubmissionData(Submission submission, Action<Project> OnSaveCompleted, Action<string> OnSaveFailed) {
        PopulateSubmissionData(submission);

        string wood = JsonUtility.ToJson(submissionData);
        var path = Application.persistentDataPath + "/PlayerSubmissionData" + GameManager.Instance.stage + ".json";
        System.IO.File.WriteAllText(path, wood);

        // Push to server
        var playerName = PlayerPrefs.GetString("PlayerName");
        WebServiceProjectManager.Instance.UploadProjectFile(playerName, submission.title, path, OnSaveCompleted, OnSaveFailed);
    }

    void PopulateSubmissionData(Submission submission) {
        // Go through all the submissions wood pieces and save them in the data
        WoodPiece[] woodPieces = submission.GetComponentsInChildren<WoodPiece>();
        submissionData.woodDatas = new WoodData[woodPieces.Length];

        Mesh tempMesh;
        MeshData tempMeshData = new MeshData();

        for (int i = 0; i < woodPieces.Length; i++) {
            submissionData.woodDatas[i] = new WoodData();
            submissionData.woodDatas[i].pos = woodPieces[i].transform.localPosition;
            submissionData.woodDatas[i].rot = woodPieces[i].transform.localRotation.eulerAngles;

            tempMesh = woodPieces[i].GetComponent<MeshFilter>().mesh;
            if (tempMesh != null) {
                tempMeshData = new MeshData();
                tempMeshData.verts = tempMesh.vertices;
                tempMeshData.normals = tempMesh.normals;
                tempMeshData.uvs = tempMesh.uv;
                tempMeshData.tris = tempMesh.triangles;
            }
            submissionData.woodDatas[i].meshData = tempMeshData;
            // Get the material name - minus (Instance)
            submissionData.woodDatas[i].material = woodPieces[i].GetComponent<MeshRenderer>().material.name.Replace(" (Instance)", "");
            submissionData.woodDatas[i].numMats = woodPieces[i].GetComponent<MeshRenderer>().materials.Length;

            // Save stats
            submissionData.woodDatas[i].numCuts = woodPieces[i].numCuts;
            submissionData.woodDatas[i].pieceName = woodPieces[i].pieceName;
            submissionData.woodDatas[i].color = woodPieces[i].paintColor;
        }

        // The first wood piece is the base, so it determines the rotation the submission should be at
        submissionData.rotation = woodPieces[0].transform.localRotation.eulerAngles;

        submissionData.title = submission.title;
    }

    public void LoadSubmissionData(string path = "") {
        if(path == "") {
            path = Application.persistentDataPath + "/PlayerSubmissionData" + GameManager.Instance.stage + ".json";
        }
        string json = System.IO.File.ReadAllText(path);
        submissionData = JsonUtility.FromJson<SubmissionData>(json);
    }
}
