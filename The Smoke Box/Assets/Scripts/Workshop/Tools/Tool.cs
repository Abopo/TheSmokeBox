using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TOOLS { JOINT, SAW, PAINT, AUGER, WEDGE, NUM_TOOLS };

public class Tool : MonoBehaviour {

    public TOOLS tool;

    [SerializeField]
    protected GameObject _toolUI;

    [SerializeField]
    protected Toggle _toolToggle;

    protected EditManager _editManager;

    protected virtual void Awake() {
        _editManager = FindFirstObjectByType<EditManager>();
    }
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()  {
        
    }

    public virtual void ActivateTool() {
        gameObject.SetActive(true);

        if (_toolUI != null) {
            _toolUI.SetActive(true);
        }

        _toolToggle.isOn = true;
    }

    public virtual void UseTool() {

    }

    public virtual void DeactivateTool() {
        gameObject.SetActive(false);
        if (_toolUI != null) {
            _toolUI.SetActive(false);
        }
        _toolToggle.isOn = false;
    }
}
