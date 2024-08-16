using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    Submission[] _submissions;

    [SerializeField] GameObject _userNameEntry;
    [SerializeField] TMPro.TMP_InputField _nameInput;

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

        _nameInput.text = PlayerPrefs.GetString("PlayerName");
        _userNameEntry.SetActive(true);
    }

    public void ConfirmNameEntry()
    {
        PlayerPrefs.SetString("PlayerName", _nameInput.text);
        SceneManager.LoadScene("Shop1");
    }

    public void CancelNameEntry()
    {
        _userNameEntry.SetActive(false);
    }

    public void Gallery() {
        SceneManager.LoadScene("Gallery");
    }

    public void Quit() {
        Application.Quit();
    }
}
