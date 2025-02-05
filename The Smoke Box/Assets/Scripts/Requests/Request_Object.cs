using UnityEngine;

public class Request_Object : Request
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        _text.text = "Make a " + _requestInfo.RequestDetails;
    }
}
