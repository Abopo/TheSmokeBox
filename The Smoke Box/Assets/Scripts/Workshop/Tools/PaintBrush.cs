using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour {

    MouseFollow _mouseFollow;
    LerpTo _model;

    Vector3 _prevPos;

    MeshRenderer _renderer;

    AudioSource _audioSource;

    private void Awake() {
        _mouseFollow = GetComponent<MouseFollow>();
        _model = GetComponentInChildren<LerpTo>();
        _audioSource = GetComponent<AudioSource>();
        _renderer = GetComponentInChildren<MeshRenderer>();
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Activate() {
    }

    public void SetPaint(Material paintMat) {
        _renderer.material = paintMat;
    }

    public void Paint(Vector3 pos) {
        if (!_model.IsLerping) {
            _mouseFollow.enabled = false;

            _prevPos = _model.transform.position;
            _model.OnLerpFinished.AddListener(OnLerpFinished1);
            _model.LerpToPos(pos, 0.2f);

            PlayClip();
        }
    }

    public void PlayClip() {
        _audioSource.Play();
    }

    void OnLerpFinished1() {
        // Lerp back to original position
        _model.LerpToPos(_prevPos, 0.2f);

        _model.OnLerpFinished.RemoveListener(OnLerpFinished1);
        _model.OnLerpFinished.AddListener(OnLerpFinished2);
    }

    void OnLerpFinished2() {
        _mouseFollow.enabled = true;
        _model.OnLerpFinished.RemoveListener(OnLerpFinished2);
    }
}
