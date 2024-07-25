using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tool : MonoBehaviour {

    [SerializeField]
    protected GameObject _toolUI;

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
    }

    public virtual void UseTool() {

    }

    public virtual void DeactivateTool() {
        gameObject.SetActive(false);
    }
}
