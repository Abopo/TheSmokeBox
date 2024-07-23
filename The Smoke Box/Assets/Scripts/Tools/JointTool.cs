using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JointTool : Tool {

    [SerializeField]
    JointNode _newJointNode;

    [SerializeField]
    JointNode _baseJointNode;

    [SerializeField]
    GameObject _confirmationUI;

    JointNode _ghostJointNode;

    bool _isJoining;
    bool _confirming;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !_isJoining) {
            StartCoroutine(JoinPieces());
        }

        if (_ghostJointNode != null) {
            GhostNodeFollow();
            if (_confirming) { 
                // Allow rotation of the ghost node
                if(Keyboard.current.aKey.isPressed) {
                    _ghostJointNode.transform.Rotate(new Vector3(0f, 0f, 20f * Time.deltaTime));
                }
                if(Keyboard.current.dKey.isPressed) {
                    _ghostJointNode.transform.Rotate(new Vector3(0f, 0f, -20f * Time.deltaTime));
                }
            }
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
        if (_ghostJointNode != null) {
            Destroy(_ghostJointNode.gameObject);
        }

        _editManager.Activate();
    }

    public void ActivateNextJoint() {
        if (_newJointNode.isActive) {
            // Parent the piece and new joint node
            _newJointNode.ParentPiece();

            // Instantiate the ghost node (that now has the piece as a child)
            InitializeGhostNode();

            // Activate the base joint node
            _baseJointNode.Activate();
            // This is the base node, so it should be able to connect with ANY wood piece on the submission
            _baseJointNode.curPiece = _editManager.curPiece;
            _editManager.LookAtSubmission();
        } else if (_baseJointNode.isActive) {
            // Parent the ghost node to the submission so the player can rotate and see how their placement looks
            _ghostJointNode.transform.parent = _baseJointNode.curPiece.transform.parent;

            _baseJointNode.Deactivate();

            _confirming = true;

            // Ask for confirmation of join
            _confirmationUI.SetActive(true);
        }
    }

    void InitializeGhostNode() {
        _ghostJointNode = Instantiate(_newJointNode.gameObject, transform).GetComponent<JointNode>();
        // Change the material of the child piece to be transparent
        string mat = _ghostJointNode.curPiece.GetComponent<MeshRenderer>().sharedMaterial.name;
        // Get rid of the " (Instance)" part of the string that shows up in material names sometimes
        mat = mat.Replace(" (Instance)", "");
        _ghostJointNode.curPiece.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/" + mat + "_Trans");
        // Also change the piece's layer so it doesn't mess with the node tracking
        _ghostJointNode.curPiece.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    void GhostNodeFollow() {
        if (_baseJointNode.isActive) {
            _ghostJointNode.transform.position = _baseJointNode.transform.position;
            _ghostJointNode.transform.rotation = Quaternion.LookRotation(-_baseJointNode.transform.forward);
        }
    }

    public void ConfirmJoin() {
        _confirmationUI.SetActive(false);
        StartCoroutine(JoinPieces());
    }

    public void CancelJoin() {
        // De-parent the ghost node back to us
        _ghostJointNode.transform.parent = transform;

        // Reactivate the base node
        _baseJointNode.Activate();

        _confirmationUI.SetActive(false);
    }

    public IEnumerator JoinPieces() {
        _isJoining = true;

        // Parent the piece to the joint node

        // Rotate the new joint node to face the base joint node
        Quaternion startRot = _newJointNode.transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(_ghostJointNode.transform.forward);
        // We need to further adjust the rotation in case the player rotated the ghost node
        endRot.eulerAngles = new Vector3(endRot.eulerAngles.x, endRot.eulerAngles.y, _ghostJointNode.transform.rotation.eulerAngles.z);

        Vector3 startPos = _newJointNode.transform.position;
        Vector3 endPos = _ghostJointNode.transform.position;
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
