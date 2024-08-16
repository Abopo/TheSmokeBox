using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class JudgingManager : MonoBehaviour {

    PlayableDirector _director;

    public List<TOPIC> topics = new List<TOPIC>();

    private void Awake() {
        _director = GetComponent<PlayableDirector>();
    }
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        // Debugging TODO: remove
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            ForceEnd();
        }
    }

    public void EndJudgingScene() {
        // TODO: move to next stage and drive thru scene
        GameManager.Instance.IncreaseStage();

        if (GameManager.Instance.stage <= 3) {
            GameManager.Instance.LoadScene("Shop" + GameManager.Instance.stage.ToString());
        } else {
            Application.Quit();
        }
    }

    void ForceEnd() {
        // Skip to the last frame of the timeline
        _director.time = _director.duration;
    }
}
