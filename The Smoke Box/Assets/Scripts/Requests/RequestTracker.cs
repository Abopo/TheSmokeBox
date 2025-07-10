using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestTracker : MonoBehaviour {

    [SerializeField]
    List<Request> _allRequests = new List<Request>();

    Submission _submission;

    Transform _requestsWindow;

    public List<Request> AllRequests {
        get { return _allRequests; }
    }

    public bool allRequirementsClear;

    private void Awake() {
        _submission = FindFirstObjectByType<Submission>();
    }
    // Start is called before the first frame update
    void Start() {
        Submission.OnChanged.AddListener(CheckSubmission);
    }

    // Update is called once per frame
    void Update() {
    }

    public void GenerateRequestItems(List<RequestInfo> requestInfos) {
        _requestsWindow = transform.GetChild(0);
        Object requestObj = Resources.Load("Prefabs/Requests/RequestItem");

        GameObject tempRequest;
        foreach (RequestInfo rInfo in requestInfos) {
            tempRequest = Instantiate(requestObj, _requestsWindow) as GameObject;
            tempRequest.GetComponent<Request>().Initialize(rInfo);
            _allRequests.Add(tempRequest.GetComponent<Request>());
        }
    }

    public void CheckSubmission() {

    }
}
