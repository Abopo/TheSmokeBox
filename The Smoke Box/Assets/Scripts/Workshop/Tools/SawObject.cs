using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawObject : MonoBehaviour {

    ParticleSystem[] _sawDustParticles;

    Animator _animator;
    SawTool _sawTool;

    private void Awake() {
        _sawDustParticles = GetComponentsInChildren<ParticleSystem>();
        _animator = GetComponent<Animator>();
        _sawTool = GetComponentInParent<SawTool>();
    }
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void PlayAnimation() {
        _animator.Play("SawForward");
    }

    public void TurnOnParticles() {
        foreach (var particle in _sawDustParticles) {
            particle.Play();
        }
    }

    public void DoneCutting() {
        _sawTool.ShowCut();
    }
}
