using UnityEngine;

public class Request : MonoBehaviour
{
    protected RequestInfo _requestInfo;
    protected SuperTextMesh _text;
    protected Submission _submission;
    protected int _score = 0;

    protected virtual void Awake() {
        _text = GetComponent<SuperTextMesh>();
        _submission = FindFirstObjectByType<Submission>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start() {
        Submission.OnChanged.AddListener(CheckRequestStatus);
    }

    public virtual void Initialize(RequestInfo rInfo) {
        Request tempRequest = null;

        // Add the appropriate Request script based on the type
        switch (rInfo.RequestType) {
            case REQUESTTYPE.OBJECT:
                tempRequest = gameObject.AddComponent<Request_Object>();
                break;
            case REQUESTTYPE.PIECE_COUNT:
                tempRequest = gameObject.AddComponent<Request_PieceCount>();
                break;
            case REQUESTTYPE.COLOR:
                tempRequest = gameObject.AddComponent<Request_Color>();
                break;
            case REQUESTTYPE.CATEGORY:
                tempRequest = gameObject.AddComponent<Request_Category>();
                break;
            case REQUESTTYPE.WOOD:
                tempRequest = gameObject.AddComponent<Request_Wood>();
                break;
            case REQUESTTYPE.TOOL:
                tempRequest = gameObject.AddComponent<Request_Tool>();
                break;
            case REQUESTTYPE.TIME:
                tempRequest = gameObject.AddComponent<Request_Time>();
                break;
            case REQUESTTYPE.SIZE:
                tempRequest = gameObject.AddComponent<Request_Size>();
                break;
        }

        if (tempRequest != null) {
            // Set the request info
            tempRequest._requestInfo = rInfo;
        }

        // Delete self (only 1 request component is needed)
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void CheckRequestStatus() {

    }

    protected virtual void DetermineScore() {

    }
}
