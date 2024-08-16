using UnityEngine;

public class GlueBottle : MonoBehaviour {

    MouseFollow _mouseFollow;
    LerpTo _model;

    Vector3 _prevPos;

    JointTool _jointTool;

    AudioSource _audioSource;

    private void Awake() {
        _mouseFollow = GetComponent<MouseFollow>();
        _model = GetComponentInChildren<LerpTo>();
        _jointTool = GetComponentInParent<JointTool>();
        _audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate() {
        _mouseFollow.enabled = true;
    }

    public void ApplyGlue(Vector3 pos) {
        if (!_model.IsLerping) {
            _mouseFollow.enabled = false;

            _prevPos = _model.transform.position;
            _model.OnLerpFinished.AddListener(OnLerpFinished1);
            _model.LerpToPos(pos, 0.2f);

            PlayGlueClip();
        }
    }

    public void PlayGlueClip() {
        _audioSource.Play();
    }

    void OnLerpFinished1() {
        _jointTool.ActivateNextJoint();

        // Lerp back to original position
        _model.LerpToPos(_prevPos, 0.2f);

        _model.OnLerpFinished.RemoveListener(OnLerpFinished1);
        _model.OnLerpFinished.AddListener(OnLerpFinished2);
    }

    void OnLerpFinished2() {
        EditManager.Instance.LookAtSubmission();
        _model.OnLerpFinished.RemoveListener(OnLerpFinished2);

        // Hide self 
        gameObject.SetActive(false);
    }
}
