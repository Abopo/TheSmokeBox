using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LerpTo : MonoBehaviour {

    // Lerping
    protected bool _lerping;
    Vector3 _startPos;
    Vector3 _endPos;
    float _distance;
    float _startTime;
    public float _lerpTime = 1f;
    float _interpolation;
    float timePassed;

    public static UnityEvent SlideFinished = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (_lerping) {
            timePassed = Time.time - _startTime;

            _interpolation = timePassed / _lerpTime;

            transform.position = Vector3.Lerp(_startPos, _endPos, _interpolation);

            if (_interpolation >= 1f) {
                _lerping = false;

                SlideFinished.Invoke();
            }
        }
    }

    public void LerpToPos(Vector3 pos) {
        _lerping = true;
        _startPos = transform.position;
        _endPos = pos;

        _distance = Vector3.Distance(_startPos, _endPos);
        if (_distance <= 0) {
            // We're already at the position so just cancel
            _lerping = false;
            SlideFinished.Invoke();
        }

        _startTime = Time.time;
        _interpolation = 0f;
    }
    public void LerpToPos(Vector3 pos, float lerpTime) {
        _lerping = true;
        _startPos = transform.position;
        _endPos = pos;

        _distance = Vector3.Distance(_startPos, _endPos);
        if (_distance <= 0) {
            // We're already at the position so just cancel
            _lerping = false;
            SlideFinished.Invoke();
        }

        _lerpTime = lerpTime;
        _startTime = Time.time;
        _interpolation = 0f;
    }

}
