using System.Collections.Generic;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    RequestTracker _rTracker;

    Object _resultsObj;

    [SerializeField]
    GameObject _container;

    [SerializeField]
    GameObject _continueButton;

    List<RequestResult> _results = new List<RequestResult>();

    private void Awake() {
        _rTracker = FindFirstObjectByType<RequestTracker>();
        _resultsObj = Resources.Load("Prefabs/Requests/RequestResult");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenResults() {
        transform.GetChild(0).gameObject.SetActive(true);
        FillResultsData();
        _continueButton.transform.SetAsLastSibling();
    }

    void FillResultsData() {
        GameObject tempObj;
        RequestResult tempResult;

        // Get request data from RequestTracker
        foreach (Request request in _rTracker.AllRequests) {
            // Create a result for each request
            tempObj = Instantiate(_resultsObj, _container.transform) as GameObject;
            tempResult = tempObj.GetComponent<RequestResult>();
            tempResult.SetRequest(request);
            _results.Add(tempResult);
        }
    }

    public void Continue() {
        // Add the money to the player
        int totalReward = 0;
        foreach (RequestResult result in _results) {
            totalReward += result.RewardMoney;
        }

        GameManager.Instance.AddMoney(totalReward);

        // Load the next scene
        GameManager.Instance.LoadScene("ShopR");
    }
}
