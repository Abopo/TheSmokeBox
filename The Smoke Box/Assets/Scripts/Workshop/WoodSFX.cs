using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSFX : MonoBehaviour {

    [SerializeField]
    float _sfxBuffer; // The time to wait before playing another sound
    float _sfxTimer;

    AudioSource[] _audioSources;

    [SerializeField]
    AudioClip[] _woodCollisionClips;

    private void Awake() {
        _audioSources = GetComponentsInChildren<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _sfxTimer += Time.deltaTime;
    }

    public void PlayWoodCollisionSFX() {
        if(_sfxTimer >= _sfxBuffer) {
            // Find a free audio source
            foreach(AudioSource source in _audioSources) {
                if(!source.isPlaying) {
                    // Set a random collision clip
                    source.clip = _woodCollisionClips[Random.Range(0, _woodCollisionClips.Length - 1)];
                    // Play the clip
                    source.Play();
                    // reset the timer
                    _sfxTimer = 0f;
                    // Don't play anything else
                    break;
                }
            }
        }
    }
}
