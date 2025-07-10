using UnityEngine;

public class Request_Wood : Request
{
    WOOD_TYPE _requestedWood;
    int _numTypePieces;
    float _typePercent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        _text.text = "Make it out of " + _requestInfo.RequestDetails + " wood: 0%";

        SetWoodType();
    }

    void SetWoodType() {
        System.Enum.TryParse(_requestInfo.RequestDetails, out _requestedWood);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void CheckRequestStatus() {
        base.CheckRequestStatus();

        _numTypePieces = CountWoodPieces();
        _typePercent = (float)_numTypePieces / (float)(_submission.WoodPieces.Length - 1);
        _text.text = GetRequestText();
    }

    int CountWoodPieces() {
        int numPieces = 0;

        foreach (WoodPiece wPiece in _submission.WoodPieces) {
            if (wPiece.Data.type == _requestedWood) {
                numPieces++;
            }
        }

        return numPieces;
    }

    protected override void DetermineScore() {
        base.DetermineScore();

        // Just do the straight percentage for now
        _score = (int)(100 * _typePercent);
    }

    public override string GetRequestText() {
        return "Make it out of " + _requestInfo.RequestDetails + " wood: " + (int)(_typePercent * 100) + "%";
    }
}
