using UnityEngine;
using System;

public class Request_Category : Request
{
    PIECE_CATEGORY _requestCategory;
    int _numCatPieces;
    float _catPercent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        _text.text = "Use a lot of " + _requestInfo.RequestDetails + " pieces: 0%";
        Enum.TryParse(_requestInfo.RequestDetails, out _requestCategory);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void CheckRequestStatus() {
        base.CheckRequestStatus();

        _numCatPieces = CountCategoryPieces();
        _catPercent = (float)_numCatPieces / (float)(_submission.WoodPieces.Length-1);
        _text.text = "Use a lot of " + _requestInfo.RequestDetails + " pieces: " + (int)(_catPercent * 100) + "%";
    }

    int CountCategoryPieces() {
        int numPieces = 0;

        foreach (WoodPiece piece in _submission.WoodPieces) {
            if(piece.Data != null && piece.Data.category == _requestCategory) {
                numPieces++;
            }
        }

        return numPieces;
    }

    protected override void DetermineScore() {
        base.DetermineScore();

        // Just do the straight percentage for now
        _score = (int)(100 * _catPercent);
    }
}
