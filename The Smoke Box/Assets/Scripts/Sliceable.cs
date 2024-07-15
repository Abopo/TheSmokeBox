using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliceable : MonoBehaviour {

    [SerializeField]
    bool _isSolid = true;

    [SerializeField]
    bool _reverseWindTriangles = false;

    [SerializeField]
    bool _useGravity = false;

    [SerializeField]
    bool _shareVertices = false;

    [SerializeField]
    bool _smoothVertices = false;

    public bool IsSolid { get => _isSolid; set => _isSolid = value; }
    public bool ReverseWindTriangles { get => _reverseWindTriangles; set => _reverseWindTriangles = value; }
    public bool UseGravity { get => _useGravity; set => _useGravity = value; }
    public bool ShareVertices { get => _shareVertices; set => _shareVertices = value; }
    public bool SmoothVertices { get => _smoothVertices; set => _smoothVertices = value; }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
