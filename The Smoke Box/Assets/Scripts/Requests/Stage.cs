using UnityEngine;
using System.Collections.Generic;

public class Stage : MonoBehaviour {

    string stageName;
    List<RequestInfo> _requests = new List<RequestInfo>();

    public List<RequestInfo> Requests { get => _requests; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        GenerateRandomRequests(5);
        FindFirstObjectByType<RequestTracker>().GenerateRequestItems(_requests);
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
        while (_requests.Count < count || availableTypes.Count == 0) {
            tempRequest = GenerateRandomRequest(availableTypes);
            _requests.Add(tempRequest);
            availableTypes.Remove(tempRequest.RequestType);
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
}
