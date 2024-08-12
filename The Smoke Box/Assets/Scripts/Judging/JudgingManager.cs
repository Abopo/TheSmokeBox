using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class JudgingManager : MonoBehaviour {

    PlayableDirector _director;

    private void Awake() {
        _director = GetComponent<PlayableDirector>();
    }
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndJudgingScene() {
        // TODO: move to next stage and drive thru scene
        GameManager.Instance.stage += 1;

        if (GameManager.Instance.stage <= 3) {
            SceneManager.LoadScene("Shop" + GameManager.Instance.stage.ToString());
        } else {
            Application.Quit();
        }
    }
}
