using UnityEngine;

public class GuideCollider : MonoBehaviour
{
    Material _matOff;
    Material _matOn;

    MeshRenderer _meshRenderer;

    public bool isSatisfied;

    private void Awake() {
        _meshRenderer = GetComponent<MeshRenderer>();

        _matOff = Resources.Load<Material>("Materials/Guide_Off");
        _matOn = Resources.Load<Material>("Materials/Guide_On");

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

        isSatisfied = true;
    }
    void SetOff() {
        if (_meshRenderer.material != _matOff) {
            _meshRenderer.material = _matOff;
        }

        isSatisfied = false;
    }
}
