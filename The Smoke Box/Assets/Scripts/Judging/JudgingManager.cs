using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class JudgingManager : MonoBehaviour {

    PlayableDirector _director;

    private void Awake() {
        _director = GetComponent<PlayableDirector>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
