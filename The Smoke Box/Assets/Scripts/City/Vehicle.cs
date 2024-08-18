using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {

    public float moveSpeed;

    public float lifetime;
    float _lifeTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        _lifeTimer += Time.deltaTime;
        if(_lifeTimer >= lifetime) {
            Destroy(gameObject);
        }
    }
}
