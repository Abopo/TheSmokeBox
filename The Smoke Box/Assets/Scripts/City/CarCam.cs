using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCam : MonoBehaviour {

    LerpTo _lerp;

    [SerializeField]
    Transform _carView;
    [SerializeField]
    Transform _shopView;

    private void Awake() {
        _lerp = GetComponent<LerpTo>();
    }
    // Start is called before the first frame update
    void Start() {
        // Make sure we start at the car view
        transform.position = _carView.position;

        _lerp.OnLerpFinished.AddListener(OnLerpFinished);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToShopView() {
        _lerp.LerpToPos(_shopView.position, 1f);
        _lerp.LerpRotation(_shopView.rotation, 1f);
    }

    void OnLerpFinished() {
        // Activate the shop?
    }
}
