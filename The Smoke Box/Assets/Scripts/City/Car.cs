using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Car : MonoBehaviour {

    public float lerpTime;

    LerpTo _lerp;

    [SerializeField]
    Waypoint _waypoint;

    private void Awake() {
        _lerp = GetComponent<LerpTo>();
    }
    // Start is called before the first frame update
    void Start() {
        _lerp.OnLerpFinished.AddListener(OnReachedWaypoint);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void GoToWaypoint() {
        _lerp.LerpToPos(_waypoint.transform.position, lerpTime);
        _lerp.LerpRotation(_waypoint.transform.rotation, lerpTime);
    }

    public void GoToWaypoint(Waypoint waypoint) {
        _waypoint = waypoint;
        GoToWaypoint();
    }

    void OnReachedWaypoint() {
        _waypoint = _waypoint.nextWaypoint;
    }
}
