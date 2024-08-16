using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBlock : MonoBehaviour
{
    WoodSFX _woodSFX;

    // Start is called before the first frame update
    void Start()
    {
        _woodSFX = FindObjectOfType<WoodSFX>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        _woodSFX.PlayWoodCollisionSFX();
    }

}
