using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShapeChecker : MonoBehaviour {

    Collider[] _colliders;

    List<Vector3> _insideVerts = new List<Vector3>(); // Verts that are contained within any colliders
    List<Vector3> _outsideVerts = new List<Vector3>(); // Verts that are not contained within any colliders

    [SerializeField] Submission _testSubmission;

    [SerializeField] Material _guideOff;
    [SerializeField] Material _guideOn;
    [SerializeField] Object _vertObj;

    List<VertPoint> _vertPoints = new List<VertPoint>();

    private void Awake() {
        _colliders = GetComponentsInChildren<Collider>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        StartCoroutine(CheckSubmissionContainment());
    }

    // Update is called once per frame
    void Update() {
    }

    /// <summary>
    /// Checks if the submission is completely within all colliders.
    /// </summary>
    IEnumerator CheckSubmissionContainment() {
        _outsideVerts.Clear();
        
        // Get every vert in the submission
        List<Vector3> allVertsWorld = GetAllVertsWorld(_testSubmission);
        // Make vert points for each vert
        InitVertPoints(allVertsWorld);


        foreach (var collider in _colliders) {
            for (int i = 0; i < allVertsWorld.Count; i++) {
                if (collider.ClosestPoint(allVertsWorld[i]) == allVertsWorld[i]) {
                    // It's inside
                    _vertPoints[i].SetOn();

                    _insideVerts.Add(allVertsWorld[i]);
                }
            }

            // Before the next collider, remove all the inside verts from the allVerts list since they are accounted for
            //foreach (var vert in _insideVerts) {
            //    allVertsWorld.Remove(vert);
            //}
        }

        // Here any verts remaining inside allVerts should be outside any collider
        foreach (var vert in allVertsWorld) {
            // Add it to the list of outside verts
            _outsideVerts.Add(vert);
        }

        yield return null;

        // This is the end of the function, for testing just run it again
        // TODO: Only run when the submission transform changes
        StartCoroutine(CheckSubmissionContainment());
    }

    List<Vector3> GetAllVertsWorld(Submission submission) {
        List<Vector3> vertsWorld = new List<Vector3>();
        Vector3 vertWorld;

        foreach (var piece in submission.WoodPieces) {
            foreach(var vert in piece.GetComponent<MeshFilter>().mesh.vertices) {
                vertWorld = submission.transform.TransformPoint(vert);
                // Try not to include duplicate verts
                if (!vertsWorld.Contains(vertWorld)) {
                    vertsWorld.Add(vertWorld);
                }
            }
        }

        return vertsWorld;
    }

    /// <summary>
    /// Places a vert point at each of the passed verts. Will create new vert point objects as needed
    /// </summary>
    /// <param name="verts"></param>
    void InitVertPoints(List<Vector3> verts) {
        VertPoint tempPoint;
        for (int i = 0; i < verts.Count; i++) {
            if(i >= _vertPoints.Count) {
                // We need to make a new vertPoint
                tempPoint = ((GameObject)Instantiate(_vertObj, transform)).GetComponent<VertPoint>();
                _vertPoints.Add(tempPoint);
            }

            _vertPoints[i].SetOff();
            _vertPoints[i].transform.position = verts[i];
        }
    }
}
