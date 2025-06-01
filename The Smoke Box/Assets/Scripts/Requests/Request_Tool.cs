using UnityEngine;

public class Request_Tool : Request
{
    TOOLS _tool;
    int _requestCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();

        _requestCount = Random.Range(5, 10);
        _text.text = "Use the " + _requestInfo.RequestDetails + " tool a lot: 0/" + _requestCount.ToString();

        System.Enum.TryParse(_requestInfo.RequestDetails, out _tool);

        switch (_tool) {
            case TOOLS.JOINT:
                FindFirstObjectByType<JointTool>(FindObjectsInactive.Include).JointToolUsed.AddListener(OnToolUsed);
                break;
            case TOOLS.SAW:
                FindFirstObjectByType<SawTool>(FindObjectsInactive.Include).SawToolUsed.AddListener(OnToolUsed);
                break;
            case TOOLS.PAINT:
                FindFirstObjectByType<PaintTool>(FindObjectsInactive.Include).PaintToolUsed.AddListener(OnToolUsed);
                break;
            case TOOLS.AUGER:
                FindFirstObjectByType<AugurTool>(FindObjectsInactive.Include).AugerToolUsed.AddListener(OnToolUsed);
                break;
            case TOOLS.WEDGE:
                FindFirstObjectByType<WedgeTool>(FindObjectsInactive.Include).WedgeToolUsed.AddListener(OnToolUsed);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnToolUsed() {
        _score++;
        _text.text = "Use the " + _requestInfo.RequestDetails + " tool a lot: " + _score.ToString() + "/" + _requestCount.ToString();
    }
}
