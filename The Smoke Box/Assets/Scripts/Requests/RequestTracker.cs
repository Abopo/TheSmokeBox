using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestTracker : MonoBehaviour {

    [SerializeField]
    List<SuperTextMesh> requirementTexts = new List<SuperTextMesh>();

    Submission _submission;

    Transform _requestsWindow;

    public bool allRequirementsClear;

    private void Awake() {
        _submission = FindFirstObjectByType<Submission>();
        _requestsWindow = transform.GetChild(0);
    }
    // Start is called before the first frame update
    void Start() {
        Submission.OnChanged.AddListener(CheckSubmission);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void GenerateRequestItems(List<RequestInfo> requestInfos) {
        Object requestObj = Resources.Load("Prefabs/Requests/RequestItem");

        GameObject tempRequest;
        foreach (RequestInfo rInfo in requestInfos) {
            tempRequest = Instantiate(requestObj, _requestsWindow) as GameObject;
            tempRequest.GetComponent<Request>().Initialize(rInfo);
        }
    }

    public void CheckSubmission() {

    }
}
