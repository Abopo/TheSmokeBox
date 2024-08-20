using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawCanvas : MonoBehaviour {

    [SerializeField]
    private GameObject _undoButton;
    [SerializeField]
    private GameObject _leftKeepButton;
    [SerializeField]
    private GameObject _leftDropButton;
    [SerializeField]
    private GameObject _leftDeleteButton;
    [SerializeField]
    private GameObject _rightKeepButton;
    [SerializeField]
    private GameObject _rightDropButton;
    [SerializeField]
    private GameObject _rightDeleteButton;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate() {
        //_undoButton.SetActive(true);
        // Both keep buttons are active by default
        _leftKeepButton.SetActive(true);
        _rightKeepButton.SetActive(true);

        // All others are hidden
        _leftDropButton.SetActive(false);
        _rightDropButton.SetActive(false);
        _leftDeleteButton.SetActive(false);
        _rightDeleteButton.SetActive(false);

        gameObject.SetActive(true);
    }

    public void SwapButtons(bool keptLeft) {
        //_undoButton.SetActive(false);
        // Hide the keep buttons
        _leftKeepButton.SetActive(false);
        _rightKeepButton.SetActive(false);

        // Show only the buttons of the non kept side
        if (keptLeft) {
            _rightDropButton.SetActive(true);
            _rightDeleteButton.SetActive(true);
        } else {
            _leftDropButton.SetActive(true);
            _leftDeleteButton.SetActive(true);

        }
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }
}
