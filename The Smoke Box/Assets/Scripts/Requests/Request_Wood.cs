using UnityEngine;

public class Request_Wood : Request
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        _text.text = "Make it out of " + _requestInfo.RequestDetails + " wood.";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
