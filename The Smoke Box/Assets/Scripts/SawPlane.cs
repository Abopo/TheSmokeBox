using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawPlane : MonoBehaviour {


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

}
