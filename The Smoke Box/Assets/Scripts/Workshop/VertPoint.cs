using UnityEngine;

public class VertPoint : MonoBehaviour
{
    [SerializeField] Material _matOff;
    [SerializeField] Material _matOn;

    MeshRenderer _meshRenderer;

    private void Awake() {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetOn() {
        _meshRenderer.material = _matOn;
    }

    public void SetOff() {
        _meshRenderer.material = _matOff;
    }
}
