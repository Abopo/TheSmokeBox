using UnityEngine;
using System.Collections.Generic;

public class Stage : MonoBehaviour {

    string stageName;
    List<RequestInfo> _requests = new List<RequestInfo>();

    public REQUESTTYPE[] testRequests;

    public List<RequestInfo> Requests { get => _requests; set => _requests = value; }

    [SerializeField]
    RequestTracker _requestTracker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
    }

    public void Initialize() {
        if (_requests.Count == 0) {
            GenerateRandomRequests(5);
        }

        if (_requestTracker == null) {
            _requestTracker = FindFirstObjectByType<RequestTracker>();
        }
        if (_requestTracker != null) {
            _requestTracker.GenerateRequestItems(_requests);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    /// <summary>
    /// Generates the given count of random requests
    /// </summary>
    /// <param name="count"></param>
    public void GenerateRandomRequests(int count) {
        // Each request type should only be generated once, so we'll make a list to keep track of what's still available
        List<REQUESTTYPE> availableTypes = new List<REQUESTTYPE> ();
        PopulateRequestTypeList (availableTypes);

        // Clear the requests list
        _requests.Clear ();

        RequestInfo tempRequest;
        int i = 0;
        while (_requests.Count < count || availableTypes.Count == 0) {
            if (testRequests.Length > i && testRequests[i] != REQUESTTYPE.NUM_REQUESTS) {
                tempRequest = GenerateRandomRequest(testRequests[i]);
            } else {
                tempRequest = GenerateRandomRequest(availableTypes);
            }
            _requests.Add(tempRequest);
            availableTypes.Remove(tempRequest.RequestType);

            i++;
        }
    }

    void PopulateRequestTypeList(List<REQUESTTYPE> typeList) {
        for (int i = 0; i < (int)REQUESTTYPE.NUM_REQUESTS; i++) {
            typeList.Add((REQUESTTYPE)i);
        }
    }

    RequestInfo GenerateRandomRequest(List<REQUESTTYPE> typeList) {
        // Get a random type from the type list
        int type = Random.Range(0, typeList.Count);
        REQUESTTYPE requestType = typeList[type];

        // Make a new request with the chosen type
        RequestInfo newRequest = new RequestInfo();
        newRequest.SetRequestType(requestType);

        return newRequest;
    }
    RequestInfo GenerateRandomRequest(REQUESTTYPE type) {
        // Make a new request with the chosen type
        RequestInfo newRequest = new RequestInfo();
        newRequest.SetRequestType(type);

        return newRequest;
    }

}
