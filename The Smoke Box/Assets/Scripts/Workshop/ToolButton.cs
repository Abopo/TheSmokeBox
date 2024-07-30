using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour {

    public bool canBeUsedOnSubmission;

    Toggle _button;

    private void Awake() {
        _button = GetComponentInChildren<Toggle>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableButton() {
        _button.interactable = true;
    }

    public void DisableButton() {
        _button.interactable = false;
    }
}
