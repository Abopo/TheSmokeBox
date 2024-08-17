using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Move the game object down until it collides with something
public class GoToFloor : MonoBehaviour
{
    bool _movingDown;

    // Start is called before the first frame update
    void Start()
    {
        _movingDown = true;
        StartCoroutine(MovingToFloor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MovingToFloor() {
        while (_movingDown) {
            transform.Translate(-Vector3.up * 0.5f, Space.World);

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        // Stop
        _movingDown = false;
    }
}
