using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class JudgingManager : MonoBehaviour {

    PlayableDirector _director;

    [SerializeField]
    AudioSource _BGM;
    [SerializeField]
    AudioClip _judgingTrack;

    public List<TOPIC> topics = new List<TOPIC>();

    private void Awake() {
        _director = GetComponent<PlayableDirector>();
    }
    // Start is called before the first frame update
    void Start() {
        PlayFanfare();
    }

    // Update is called once per frame
    void Update() {
        // Debugging TODO: remove
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            ForceEnd();
        }

        // Once the fanfare is done
        if (!_BGM.isPlaying) {
            // Play the music
            PlayMusic();
        }
    }

    public void PlayFanfare() {
        _BGM.Play();
    }

    public void PlayMusic() {
        _BGM.clip = _judgingTrack;
        _BGM.loop = true;
        _BGM.Play();
    }

    public void EndJudgingScene() {
        // TODO: move to next stage and drive thru scene
        GameManager.Instance.IncreaseStage();

        if (GameManager.Instance.stage <= 3) {
            GameManager.Instance.LoadScene("Shop" + GameManager.Instance.stage.ToString());
        } else {
            GameManager.Instance.LoadScene("Epilogue");
        }
    }

    void ForceEnd() {
        // Skip to the last frame of the timeline
        _director.time = _director.duration;
    }
}
