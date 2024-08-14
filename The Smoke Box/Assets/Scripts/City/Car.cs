using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Car : MonoBehaviour {

    public float lerpTime;

    LerpTo _lerp;

    [SerializeField]
    Waypoint _waypoint;

    AudioSource _audioSource;

    private void Awake() {
        _lerp = GetComponent<LerpTo>();
        _audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start() {
        _lerp.OnLerpFinished.AddListener(OnReachedWaypoint);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void GoToWaypoint() {
        StartCoroutine(GoToWaypointLater());

        // Start the acceleration audio
        _audioSource.Play();
    }

    IEnumerator GoToWaypointLater() {
        // Give the audio clip a sec to start up
        yield return new WaitForSeconds(0.5f);

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
