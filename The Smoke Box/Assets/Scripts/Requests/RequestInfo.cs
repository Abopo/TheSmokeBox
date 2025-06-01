using UnityEngine;
using System.IO;

public enum REQUESTTYPE { OBJECT, COLOR, PIECE_COUNT, CATEGORY, WOOD, TIME, TOOL, SIZE, NUM_REQUESTS };
public class RequestInfo {

    REQUESTTYPE _requestType;

    string _requestDetails;

    public REQUESTTYPE RequestType { get => _requestType; }
    public string RequestDetails { get => _requestDetails; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRequestType(REQUESTTYPE rType) {
        // When the type is set, determine the actual values of the request
        _requestType = rType;

        switch (_requestType) {
            case REQUESTTYPE.OBJECT:
                DetermineObjectDetails();
                break;
            case REQUESTTYPE.COLOR:
                DetermineColorDetails();
                break;
            case REQUESTTYPE.PIECE_COUNT:
                DeterminePieceCountDetails();
                break;
            case REQUESTTYPE.CATEGORY:
                DeterminePieceCategoryDetails();
                break;
            case REQUESTTYPE.WOOD:
                DeterminePieceWoodDetails();
                break;
            case REQUESTTYPE.TIME:
                DetermineTimeDetails();
                break;
            case REQUESTTYPE.TOOL:
                DetermineToolDetails();
                break;
            case REQUESTTYPE.SIZE:
                DetermineSizeDetails();
                break;
        }
    }

    void DetermineObjectDetails() {
        // Choose a random object from the available prefabs

        // First, get all the prefabs from the ShapeOutlines folder
        // TODO: this is obviously horrible, find some way to just load the file names of the prefabs.
        Object[] objects = Resources.LoadAll("Prefabs/Requests/ShapeOutlines");
        int rand = Random.Range(0, objects.Length);
        _requestDetails = objects[rand].name;       
    }

    void DetermineColorDetails() {
        // Choose a random color from available paints

        // For now, just use 0-9
        int randColor = Random.Range(0, 10);

        switch(randColor) {
            case 0:
                _requestDetails = "White";
                break;
            case 1:
                _requestDetails = "Red";
                break;
            case 2:
                _requestDetails = "Green";
                break;
            case 3:
                _requestDetails = "Blue";
                break;
            case 4:
                _requestDetails = "Yellow";
                break;
            case 5:
                _requestDetails = "Pink";
                break;
            case 6:
                _requestDetails = "Purple";
                break;
            case 7:
                _requestDetails = "Orange";
                break;
            case 8:
                _requestDetails = "Cyan";
                break;
            case 9:
                _requestDetails = "Black";
                break;
        }
    }

    void DeterminePieceCountDetails() {
        // Choose a random, reasonable piece count

        _requestDetails = Random.Range(5, 20).ToString();
    }

    void DeterminePieceCategoryDetails() {
        // Choose a random piece category
        PIECE_CATEGORY chosenCat = (PIECE_CATEGORY)Random.Range(0, (int)PIECE_CATEGORY.NUM_CATS);

        _requestDetails = chosenCat.ToString();
    }

    void DeterminePieceWoodDetails() {
        // Select wood type randomly for now
        _requestDetails = ((WOOD_TYPE)Random.Range(0, (int)WOOD_TYPE.NUM_TYPES)).ToString();
    }

    void DetermineTimeDetails() {
        // Choose a random, reasonable amount of time

        // In seconds:
        _requestDetails = Random.Range(120, 300).ToString();
    }

    void DetermineToolDetails() {
        // Choose a random, available tool
        _requestDetails = ((TOOLS)Random.Range(0, (int)TOOLS.NUM_TOOLS)).ToString();
    }

    void DetermineSizeDetails() {
        // Choose a random, reasonable size
        _requestDetails = Random.Range(300, 800).ToString();
    }
}
