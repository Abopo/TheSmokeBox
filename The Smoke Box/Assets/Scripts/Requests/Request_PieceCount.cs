using UnityEngine;

public class Request_PieceCount : Request
{
    int _requestedPieceCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        _text.text = "Use around " + _requestInfo.RequestDetails + " pieces: " + "0/" + _requestInfo.RequestDetails;
        _requestedPieceCount = int.Parse(_requestInfo.RequestDetails);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void CheckRequestStatus() {
        base.CheckRequestStatus();


        _text.text = GetRequestText();
    }

    protected override void DetermineScore() {
        base.DetermineScore();

        // The results of the request should be how close to the requested number of pieces are used.
        // I think this could kinda be scummed by using very tiny pieces? Maybe limited uses of the saw fixes that.
        
        // Probly need some sort of algorithm to award points based on difference
        int dif = Mathf.Abs(_submission.WoodPieces.Length - _requestedPieceCount - 1);
        // So, 0 should be maximum points, and should fall off as it get's larger
        // idk what a good number for max reward is, but let's just set it at 100 for now
        _score = (int)(100 - (dif * 1.5) * 10);
    }

    public override string GetRequestText() {
        return "Use around " + _requestInfo.RequestDetails + " pieces: " + (_submission.WoodPieces.Length - 1) + "/" + _requestInfo.RequestDetails;
    }
}
