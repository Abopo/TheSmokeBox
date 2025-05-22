using System.Collections;
using UnityEngine;
using Parabox.CSG;

public class AugurTool : Tool {
    
    [SerializeField]
    GameObject _bitObject;

    [SerializeField]
    GameObject _bitVisualizer;

    [SerializeField]
    Mesh[] _bitMeshes;

    [SerializeField]
    private Canvas _postCutCanvas;

    GameObject _composite;
    WoodPiece _originalPiece;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public override void ActivateTool() {
        base.ActivateTool();

        _toolUI.SetActive(true);

        _originalPiece = _editManager.curPiece;

        _bitVisualizer.SetActive(true);
        // Make sure the augur bit has the same material as the piece
        _bitObject.GetComponent<MeshRenderer>().material = _originalPiece.Material;
    }

    public override void DeactivateTool() {
        base.DeactivateTool();

        _toolUI.SetActive(false);
    }

    public void ChangeBit(int bitIndex) {
        _bitObject.GetComponent<MeshFilter>().sharedMesh = _bitMeshes[bitIndex];
        _bitVisualizer.GetComponent<MeshFilter>().sharedMesh = _bitMeshes[bitIndex];
    }

    public override void UseTool() {
        base.UseTool();

        // Full disable all edit controls
        _editManager.DisableRotation();

        Model result = CSG.Perform(CSG.BooleanOp.Subtraction, _originalPiece.gameObject, _bitObject);

        BuildComposite(result);

        StartCoroutine(AdjustMeshPivotPoints(_composite));
    }

    void BuildComposite(Model boolResult) {
        _composite = new GameObject();
        _composite.name = "WoodPiece(Augured)";
        _composite.transform.position = _originalPiece.transform.position;
        _composite.AddComponent<MeshFilter>().sharedMesh = boolResult.mesh;
        _composite.AddComponent<MeshRenderer>().sharedMaterials = boolResult.materials.ToArray();
        _composite.AddComponent<MeshCollider>();
        Rigidbody rBody = _composite.AddComponent<Rigidbody>();
        rBody.isKinematic = true;
        WoodPiece wPiece = _composite.AddComponent<WoodPiece>();
        wPiece.CopyWoodData(_originalPiece);
    }

    private IEnumerator AdjustMeshPivotPoints(GameObject meshObject) {
        // This doesn't work if the rotation isn't identity.
        // So, we should save the current rotation, set to identity, then reapply the rotation after.
        Quaternion curRotation = meshObject.transform.rotation;
        meshObject.transform.rotation = Quaternion.identity;
        // Maybe the huge scale is also messing it up?
        Vector3 curScale = meshObject.transform.localScale;
        meshObject.transform.localScale = new Vector3(1f, 1f, 1f);

        yield return new WaitForSecondsRealtime(0.1f);

        MeshCollider meshCollider = meshObject.GetComponent<MeshCollider>();
        Bounds meshBounds = meshCollider.bounds;
        Mesh mesh = meshObject.GetComponent<MeshFilter>().sharedMesh;
        //Determine the offset
        Vector3 offset = meshObject.transform.position - meshBounds.center;

        //Get the vertices from the gameObject
        Vector3[] objectVerts = mesh.vertices;
        //Loop through our vertices and add the offset
        for (int i = 0; i < objectVerts.Length; i++) {

            objectVerts[i] += offset;
        }
        //Assign the modified vertices to our gameObject
        mesh.vertices = objectVerts;

        yield return null;

        // Assign the updated mesh back
        meshObject.GetComponent<MeshFilter>().sharedMesh = null;
        meshObject.GetComponent<MeshFilter>().sharedMesh = mesh;

        // Update the mesh collider
        meshCollider.sharedMesh = null;
        yield return null;
        meshCollider.sharedMesh = mesh;
        yield return null;

        // Reapply rotation
        meshObject.transform.rotation = curRotation;
        // and scale
        meshObject.transform.localScale = curScale;

        // Recalculate stuff
        meshObject.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
        meshObject.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();

        ShowResult();
    }

    public void ShowResult() {
        // Hide original piece
        _originalPiece.gameObject.SetActive(false);

        // Hide the visualizer
        _bitVisualizer.gameObject.SetActive(false);

        // Show post cut canvas
        _postCutCanvas.gameObject.SetActive(true);

        // Set curPiece as the result
        _editManager.curPiece = _composite.GetComponent<WoodPiece>();
        _editManager.EnableRotation();
    }

    public void UndoDrill() {
        // Hide composite
        _composite.SetActive(false);

        // Show original piece
        _editManager.curPiece = _originalPiece;
        _editManager.curPiece.gameObject.SetActive(true);

        // Show the visualizer
        _bitVisualizer.gameObject.SetActive(true);

        _postCutCanvas.gameObject.SetActive(false);
    }

    public void ConfirmDrill() {
        DeactivateTool();

        _postCutCanvas.gameObject.SetActive(false);
        _editManager.Activate();
    }
}
