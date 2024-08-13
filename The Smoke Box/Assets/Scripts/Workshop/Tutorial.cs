using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour {

    [SerializeField]
    GameObject _pickUpTut;

    [SerializeField]
    GameObject _rotateTut;

    bool _done;

    // Start is called before the first frame update
    void Start() {
        if (GameManager.Instance.stage == 1) {
            EditManager.OnPickedUpPiece.AddListener(OnPickUpPiece);
            StartCoroutine(ShowFirstTut());
        }
    }

    IEnumerator ShowFirstTut() {
        yield return new WaitForSeconds(4);

        _pickUpTut.SetActive(true);
    }

    void OnPickUpPiece() {
        if (!_done) {
            StartCoroutine(ShowRotateTut());
        }
    }

    IEnumerator ShowRotateTut() {
        _pickUpTut.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        _rotateTut.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_rotateTut.activeSelf) {
            if (Mouse.current.rightButton.wasPressedThisFrame) {
                _rotateTut.SetActive(false);
                _done = true;
            }
        }
    }


}
