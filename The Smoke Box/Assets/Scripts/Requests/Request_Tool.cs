using UnityEngine;

public class Request_Tool : Request
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        _text.text = "Use the " + _requestInfo.RequestDetails + " tool a lot.";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
