using UnityEngine;

public class GuideCollider : MonoBehaviour
{
    [SerializeField] Material _matOff;
    [SerializeField] Material _matOn;

    MeshRenderer _meshRenderer;

    private void Awake() {
        _meshRenderer = GetComponent<MeshRenderer>();
        SetOff();
    }

    private void OnTriggerStay(Collider other) {
        SetOn();
    }
    private void OnTriggerExit(Collider other) {
        SetOff();
    }

    void SetOn() {
        if (_meshRenderer.material != _matOn) {
            _meshRenderer.material = _matOn;
        }
    }
    void SetOff() {
        if (_meshRenderer.material != _matOff) {
            _meshRenderer.material = _matOff;
        }
    }
}
