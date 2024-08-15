using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditAudio : MonoBehaviour {

    [SerializeField]
    AudioClip _jointClip;

    AudioSource _audioSource;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayJointClip() {
        _audioSource.clip = _jointClip;
        _audioSource.Play();
    }
}
