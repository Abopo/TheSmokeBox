using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    Submission[] _submissions;

    // Start is called before the first frame update
    void Start()
    {
        LoadSubmissions();
    }

    void LoadSubmissions() {
        // Load our submission based on who we are, and what stage it is.
        string path;
        for(int i = 0; i < _submissions.Length; i++) {
            path = Application.persistentDataPath + "/PlayerSubmissionData" + (i+1) + ".json";
            if (File.Exists(path)) {
                _submissions[i].LoadData(path);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        SceneManager.LoadScene("Shop1");
    }

    public void Gallery() {

    }

    public void Quit() {
        Application.Quit();
    }
}
