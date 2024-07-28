using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tool : MonoBehaviour {

    [SerializeField]
    protected GameObject _toolUI;

    [SerializeField]
    protected Toggle _toolToggle;

    protected EditManager _editManager;

    protected virtual void Awake() {
        _editManager = FindObjectOfType<EditManager>();
    }
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()  {
        
    }

    public virtual void ActivateTool() {
        gameObject.SetActive(true);
        _toolToggle.isOn = true;
    }

    public virtual void UseTool() {

    }

    public virtual void DeactivateTool() {
        gameObject.SetActive(false);
        _toolToggle.isOn = false;
    }
}
