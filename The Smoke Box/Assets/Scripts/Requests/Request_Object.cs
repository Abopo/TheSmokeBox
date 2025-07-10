using UnityEngine;

public class Request_Object : Request
{
    ShapeChecker _checker;
    int _satisfiedColliders;
    float _percent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        _text.text = "Make a " + _requestInfo.RequestDetails + ": 0%";

        // Spawn the prefab
        _checker = FindFirstObjectByType<ShapeChecker>();
        if (_checker != null) {
            GameObject shapeObj = Resources.Load("Prefabs/Requests/ShapeOutlines/" + _requestInfo.RequestDetails) as GameObject;
            _checker.AddShape(shapeObj);
        }
    }

    protected override void CheckRequestStatus() {
        base.CheckRequestStatus();

        _satisfiedColliders = CountColliders();
        _percent = (float)_satisfiedColliders / (float)(_checker.Colliders.Length - 1);
        _text.text = GetRequestText();
    }

    protected override void DetermineScore() {
        base.DetermineScore();

        _score = (int)((float)_satisfiedColliders / _checker.Colliders.Length * 100);
    }

    int CountColliders() {
        int count = 0;

        GuideCollider gCollider;
        foreach (Collider collider in _checker.Colliders) {
            gCollider = collider.GetComponent<GuideCollider>();
            if (gCollider != null && gCollider.isSatisfied) {
                count++;
            }
        }

        return count;
    }

    public override string GetRequestText() {
        return "Make a " + _requestInfo.RequestDetails + " " + (int)(_percent * 100) + "%";
    }
}
