using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class Submission : MonoBehaviour {
    
    public Transform baseTransform;

    public bool hasBase;

    public UnityEvent OnAddedPiece = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPieceAsBase(WoodPiece wPiece) {
        wPiece.transform.parent = baseTransform;
        wPiece.GoTo(transform.position);
        wPiece.isLocked = true;

        hasBase = true;
        OnAddedPiece.Invoke();
    }
}
