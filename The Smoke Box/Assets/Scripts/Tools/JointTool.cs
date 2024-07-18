using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointTool : Tool {

    [SerializeField]
    JointNode _newJointNode;

    [SerializeField]
    JointNode _baseJointNode;

    bool _isJoining;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !_isJoining) {
            StartCoroutine(JoinPieces());
        }
    }

    public override void ActivateTool() {
        base.ActivateTool();

        _baseJointNode.gameObject.SetActive(false);
        _newJointNode.Activate();
        _newJointNode.curPiece = _editManager.curPiece;
    }

    public override void DeactivateTool() {
        base.DeactivateTool();

        _isJoining = false;
        _newJointNode.Deactivate();
        _baseJointNode.Deactivate();
        _editManager.Activate();
    }

    public void ActivateNextJoint() {
        if(_newJointNode.isActive) {
            _baseJointNode.Activate();
            // This is the base node, so it should be able to connect with ANY wood piece on the submission
            _baseJointNode.curPiece = _editManager.curPiece;
            _editManager.LookAtSubmission();
        } else if(_baseJointNode.isActive) {
            // Ask for confirmation of join
        }
    }

    public IEnumerator JoinPieces() {
        _isJoining = true;

        // Parent the piece to the joint node
        _newJointNode.ParentPiece();

        // Rotate the new joint node to face the base joint node
        Quaternion startRot = _newJointNode.transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(-_baseJointNode.transform.forward);
        Vector3 startPos = _newJointNode.transform.position;
        Vector3 endPos = _baseJointNode.transform.position;
        float interpolation = 0;
        float lerpTime = 1;
        float _startTime = Time.time;
        float timePassed = 0;

        while(interpolation < 1.0f) {
            timePassed = Time.time - _startTime;
            interpolation = timePassed / lerpTime;
            
            // Rotate lerp
            _newJointNode.transform.rotation = Quaternion.Lerp(startRot, endRot, interpolation);
            // Position lerp
            _newJointNode.transform.position = Vector3.Lerp(startPos, endPos, interpolation);

            yield return null;
        }

        // TODO: wait to do this until confirmation
        // Reparent new piece to the base object
        _newJointNode.curPiece.transform.parent = _baseJointNode.curPiece.transform.parent;
        _newJointNode.curPiece.isLocked = true;

        // Deactivate the tool
        DeactivateTool();
    }
}
