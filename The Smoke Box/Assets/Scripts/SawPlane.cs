using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawPlane : MonoBehaviour {

    Plane _plane;

    // Start is called before the first frame update
    void Start() {
        _plane = new Plane(Vector3.right, transform.position);
    }

    void CreatePlane() {
        var filter = GetComponent<MeshFilter>();
        Vector3 normal = Vector3.zero;

        if (filter && filter.mesh.normals.Length > 0)
            normal = filter.transform.TransformDirection(filter.mesh.normals[0]);

        _plane = new Plane(normal, transform.position);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public WoodPiece SawPiece(WoodPiece wPiece) {
        // Slice the piece
        GameObject[] pieces = Slicer.Slice(_plane, wPiece.gameObject);

        // Destroy the original, as there will be 2 game objects made
        Destroy(wPiece.gameObject);

        return pieces[0].GetComponent<WoodPiece>();
    }
}
