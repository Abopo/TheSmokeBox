using UnityEngine;

public class Request_Color : Request
{
    PAINTCOLOR _requestedColor;
    float _correctColorPercent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        _text.text = "Use a lot of " + _requestInfo.RequestDetails + " paint: 0%";

        System.Enum.TryParse(_requestInfo.RequestDetails, out _requestedColor);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void CheckRequestStatus() {
        // We need to check what percentage of the submission is using the correct color
        // Is just percentage okay? Can someone just use a single piece and get full points for coloring it?
        // They probably wouldn't get points for any other request so maybe it's okay?

        int numPiecesColored = -1;
        foreach(var wPiece in _submission.WoodPieces) {
            if (wPiece.paintColor == _requestedColor) {
                numPiecesColored++;
            }
        }

        if (numPiecesColored > 0) {
            _correctColorPercent = (float)numPiecesColored / (float)(_submission.WoodPieces.Length - 1);
        } else {
            _correctColorPercent = 0;
        }
        _text.text = GetRequestText();
    }

    protected override void DetermineScore() {
        base.DetermineScore();

        // Just do the straight percentage for now
        _score = (int)(100 * _correctColorPercent);
    }

    public override string GetRequestText() {
        return "Use a lot of " + _requestInfo.RequestDetails.ToString() + " paint: " + (int)(_correctColorPercent * 100) + "%";
    }
}
