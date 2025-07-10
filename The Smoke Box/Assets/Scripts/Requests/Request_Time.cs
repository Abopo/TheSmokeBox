using UnityEngine;

public class Request_Time : Request
{
    float _curTime = 0;
    int _endTime;

    protected override void Awake() {
        base.Awake();

        _isActive = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();

        _text.text = "Make it in " + _requestInfo.RequestDetails + " seconds: 0";
        _endTime = int.Parse(_requestInfo.RequestDetails);
    }
    // Update is called once per frame
    void Update()
    {
        if (_isActive) {
            _curTime += Time.deltaTime;
            _text.text = GetRequestText();
        }
    }

    protected override void DetermineScore() {
        base.DetermineScore();

        // Score starts at 100
        _score = 100;
        // Reduce score as time goes beyond limit
        if (_curTime > _endTime) {
            _score -= ((int)_curTime - _endTime) * 2;
            if (_score < 0) _score = 0;
        }
    }

    public override string GetRequestText() {
        return "Make it in " + _requestInfo.RequestDetails + " seconds: " + (int)_curTime;
    }
}
