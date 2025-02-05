using UnityEngine;

public class Request_Time : Request
{
    float _curTime = 0;
    int _endTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        _text.text = "Make it in " + _requestInfo.RequestDetails + " seconds: 0";
        _endTime = int.Parse(_requestInfo.RequestDetails);
    }
    // Update is called once per frame
    void Update()
    {
        _curTime += Time.deltaTime;
        _text.text = "Make it in " + _requestInfo.RequestDetails + " seconds: " + (int)_curTime;
    }

    protected override void DetermineScore() {
        base.DetermineScore();

        // Score starts at 100
        _score = 100;
        // Reduce score as time goes beyond limit
        if (_curTime > _endTime) {
            _score -= (_endTime - (int)_curTime) * 2;
            if (_score < 0) _score = 0;
        }
    }
}
