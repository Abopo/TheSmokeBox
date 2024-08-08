using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopArrow : MonoBehaviour {
    
    WoodShop _woodShop;

    void Awake() {
        _woodShop = GetComponentInParent<WoodShop>();
    }
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnMouseDown() {
        StartCoroutine(_woodShop.ChangePage());
    }
}
