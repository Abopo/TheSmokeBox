using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    [SerializeField]
    Transform _creditsObj;

    [SerializeField]
    GameObject _galleryButton;

    [SerializeField]
    float _rollSpeed;

    bool _rolling;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_rolling) {
            _creditsObj.Translate(Vector3.up * _rollSpeed * Time.deltaTime, Space.World);
            if(_creditsObj.position.y >= 40) {
                ShowGalleryButton();
                _rolling = false;
            }
        }
    }

    public void RollCredits() {
        _rolling = true;
    }

    void ShowGalleryButton() {
        _galleryButton.SetActive(true);
    }

    public void LoadGallery() {
        GameManager.Instance.LoadScene("Gallery");
    }
}
