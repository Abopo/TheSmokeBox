using UnityEngine;

public class RequestResult : MonoBehaviour {

    [SerializeField]
    SuperTextMesh _requestText;
    [SerializeField]
    SuperTextMesh _resultText;
    [SerializeField]
    SuperTextMesh _rewardText;

    Request _request;

    int _rewardMoney;

    public int RewardMoney { get => _rewardMoney; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SetRequest(Request request) {
        _request = request;
        SetRequestText(_request.GetRequestText());
        DetermineReward();
    }

    void SetRequestText(string text) {
        _requestText.text = text;
    }

    void SetResultText(string text) {
        _resultText.text = text;
    }

    void DetermineReward() {
        _rewardMoney = (int)Mathf.Ceil(_request.Score/10);
        _rewardText.text = "$" + _rewardMoney.ToString();
    }
}
