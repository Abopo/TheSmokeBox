using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Spawns in the wood pieces when entering the Workshop scene
/// </summary>
public class WoodSpawner : MonoBehaviour {

    // For testing
    [SerializeField]
    ShopItemData[] _spawnTestList;

    [SerializeField]
    GameObject _woodPieceObj;

    List<GameObject> _spawnList = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {
        if (GameManager.Instance.playerInventory.Count > 0) {
            StartCoroutine(SpawnWoodPieces());
        } else {
            StartCoroutine(SpawnWoodPiecesTest());
        }
    }

    // Update is called once per frame
    void Update() {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            //StartCoroutine(SpawnWoodPiecesTest());
        }
    }

    IEnumerator SpawnWoodPiecesTest() {
        GameObject tempPiece;
        Rigidbody tempRigidbody;

        foreach (ShopItemData piece in _spawnTestList) {
            if(piece == null) continue;

            tempPiece = Instantiate(_woodPieceObj);

            _spawnList.Add(tempPiece);

            tempPiece.GetComponent<WoodPiece>().SetData(piece);
            PositionPiece(tempPiece.gameObject);

            tempRigidbody = tempPiece.GetComponent<Rigidbody>();
            tempRigidbody.isKinematic = false;

            //tempRigidbody.AddForce(new Vector3(Random.Range(-5f, 5f), -1f, Random.Range(-5f, 5f)), ForceMode.Impulse);

            //yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.1f);

        PUllPiecesIn();

        yield return new WaitForSeconds(1f);

        DropPieces();
    }


    /// <summary>
    /// This will spawn all the wood pieces from the list of pieces purchased from the shop.
    /// It should first instantiate all the pieces and position them spread apart along a plane over the table
    /// Then, all the pieces should drop onto the table and quickly come to rest
    /// </summary>
    IEnumerator SpawnWoodPieces() {
        GameObject tempPiece;
        Rigidbody tempRigidbody;

        foreach (ShopItemData piece in GameManager.Instance.playerInventory) {
            tempPiece = Instantiate(_woodPieceObj);

            _spawnList.Add(tempPiece);

            tempPiece.GetComponent<WoodPiece>().SetData(piece);
            PositionPiece(tempPiece.gameObject);

            tempRigidbody = tempPiece.GetComponent<Rigidbody>();
            tempRigidbody.isKinematic = false;
        }

        yield return new WaitForSeconds(0.1f);

        PUllPiecesIn();

        yield return new WaitForSeconds(1f);

        DropPieces();
    }

    void PositionPiece(GameObject piece) {
        // Will just putting all the pieces in the same spot work? Will the physics push them apart?
        piece.transform.position = new Vector3(transform.position.x + Random.Range(-5f, 5f), transform.position.y, transform.position.z + Random.Range(-3f, 3f));
    }

    void PUllPiecesIn() {
        // Pull the rigidbodies toward us
        foreach (GameObject piece in _spawnList) {
            piece.GetComponent<Rigidbody>().AddExplosionForce(-500f, transform.position, 50f);
        }
    }

    void DropPieces() {
        Rigidbody tempRigidbody;

        foreach (GameObject piece in _spawnList) {
            tempRigidbody = piece.GetComponent<Rigidbody>();
            // Turn on gravity
            tempRigidbody.useGravity = true;
            // Clear the constraints
            tempRigidbody.constraints = RigidbodyConstraints.None;
            // Give a little push to separate the pieces
            tempRigidbody.AddExplosionForce(100f, transform.position, 50f);

            // Also, turn on shadows for the mesh renderer
            piece.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
}
