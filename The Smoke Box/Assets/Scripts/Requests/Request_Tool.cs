using UnityEngine;

public class Request_Tool : Request
{
    TOOLS _tool;
    int _requestCount;
    int _useCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();

        _requestCount = Random.Range(5, 10);
        _text.text = "Use the " + _requestInfo.RequestDetails + " tool a lot: 0/" + _requestCount.ToString();

        System.Enum.TryParse(_requestInfo.RequestDetails, out _tool);

        switch (_tool) {
            case TOOLS.JOINT:
                JointTool jointTool = FindFirstObjectByType<JointTool>(FindObjectsInactive.Include);
                if (jointTool != null) {
                    jointTool.JointToolUsed.AddListener(OnToolUsed);
                }
                break;
            case TOOLS.SAW:
                SawTool sawTool = FindFirstObjectByType<SawTool>(FindObjectsInactive.Include);
                if (sawTool != null) {
                    sawTool.SawToolUsed.AddListener(OnToolUsed);
                }
                break;
            case TOOLS.PAINT:
                PaintTool paintTool = FindFirstObjectByType<PaintTool>(FindObjectsInactive.Include);
                if (paintTool != null) {
                    paintTool.PaintToolUsed.AddListener(OnToolUsed);
                }
                break;
            case TOOLS.AUGER:
                AugurTool augurTool = FindFirstObjectByType<AugurTool>(FindObjectsInactive.Include);
                if (augurTool != null) {
                    augurTool.AugerToolUsed.AddListener(OnToolUsed);
                }
                break;
            case TOOLS.WEDGE:
                WedgeTool wedgeTool = FindFirstObjectByType<WedgeTool>(FindObjectsInactive.Include);
                if (wedgeTool != null) {
                    wedgeTool.WedgeToolUsed.AddListener(OnToolUsed);
                }
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnToolUsed() {
        _useCount++;
        _text.text = GetRequestText();
    }

    protected override void DetermineScore() {
        base.DetermineScore();

        _score = 100;
        _score -= Mathf.Abs(_requestCount - _useCount) * 20;
        if (_score < 0) _score = 0;
    }

    public override string GetRequestText() {
        return "Use the " + _requestInfo.RequestDetails + " tool a lot: " + _useCount.ToString() + "/" + _requestCount.ToString();
    }
}
