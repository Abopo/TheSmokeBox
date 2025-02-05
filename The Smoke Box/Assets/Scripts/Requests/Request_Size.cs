using UnityEngine;

public class Request_Size : Request
{
    int _requestedSize;
    Bounds _subBounds;
    float _largestAxis = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        _text.text = "Make it about " + _requestInfo.RequestDetails + "cm big: 0cm";
        _requestedSize = int.Parse(_requestInfo.RequestDetails);
    }
    // Update is called once per frame
    void Update()
    {
        DrawBounds();
    }

    protected override void CheckRequestStatus() {
        // We need to get the largest axis length of the bounds of all the pieces of the submission
        _subBounds = new Bounds(_submission.transform.position, Vector3.zero);

        // This shooouuuld create a bounds that encapsulates the entire submission
        Renderer tempRenderer;
        foreach (WoodPiece piece in _submission.WoodPieces) {
            tempRenderer = piece.GetComponent<Renderer>();
            _subBounds.Encapsulate(tempRenderer.bounds);
        }

        _largestAxis = _subBounds.size.x;
        if (_subBounds.size.y > _largestAxis) {
            _largestAxis = _subBounds.size.y;
        }
        if (_subBounds.size.z > _largestAxis) {
            _largestAxis = _subBounds.size.z;
        }

        _text.text = "Make it about " + _requestInfo.RequestDetails + "cm big: " + (int)_largestAxis*100 + "cm";
    }

    protected override void DetermineScore() {
        base.DetermineScore();

        // Similar to PieceCount, score should be determined by how close to the size the submission is
        // Probly need some sort of algorithm to award points based on difference
        int dif = Mathf.Abs(_submission.WoodPieces.Length - _requestedSize);
        // So, 0 should be maximum points, and should fall off as it get's larger
        if (dif <= 10) {
            // As long as they get close, they should get full points
            _score = 100;
        } else {
            // idk what a good number for max reward is, but let's just set it at 100 for now
            _score = (int)(100 - (dif * 0.5f));
        }
    }

    void DrawBounds() {
        Vector3 extents = _subBounds.extents;
        Debug.DrawLine(_subBounds.center + extents, _subBounds.center + new Vector3(-extents.x, extents.y, extents.z));
        Debug.DrawLine(_subBounds.center + extents, _subBounds.center + new Vector3(extents.x, -extents.y, extents.z));
        Debug.DrawLine(_subBounds.center + extents, _subBounds.center + new Vector3(extents.x, extents.y, -extents.z));
        Debug.DrawLine(_subBounds.center - extents, _subBounds.center + new Vector3(extents.x, -extents.y, -extents.z));
        Debug.DrawLine(_subBounds.center - extents, _subBounds.center + new Vector3(-extents.x, extents.y, -extents.z));
        Debug.DrawLine(_subBounds.center - extents, _subBounds.center + new Vector3(-extents.x, -extents.y, extents.z));
    }
}
