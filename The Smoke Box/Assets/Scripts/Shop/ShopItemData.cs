using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PIECE_CATEGORY { BASIC, ANIMAL, COOL, NUM_CATS };
public enum WOOD_TYPE { ACACIA, ASH, BEECH, OAK, SPRUCE, WALNUT, NUM_TYPES };

[CreateAssetMenu(fileName = "ShopItemData", menuName = "ScriptableObjects/ShopItemData", order = 1)]
public class ShopItemData : ScriptableObject {

    public string itemName;
    public int price;
    public Mesh mesh;
    public Vector3 rotation;
    public PIECE_CATEGORY category;
    public WOOD_TYPE type;

    public void CopyData(ShopItemData inData) {
        itemName = inData.itemName;
        price = inData.price;
        mesh = inData.mesh;
        rotation = inData.rotation;
        category = inData.category;
        type = inData.type;
    }
}
