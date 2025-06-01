using UnityEngine;

public class WedgeObject : AttachToPiece
{
    WedgeTool _wedgeTool;
    Animator _animator;

    private void Awake() {
        _wedgeTool = GetComponentInParent<WedgeTool>();
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation() {
        _isActive = false;
        _animator.Play("WedgeHit");
    }

    public void OnAnimationComplete() {
        _wedgeTool.RemovePiece();
        _isActive = true;
    }
}
