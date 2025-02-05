using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PIECE_CATEGORY { BASIC, ANIMAL, COOL, NUM_CATS };

[CreateAssetMenu(fileName = "ShopItemData", menuName = "ScriptableObjects/ShopItemData", order = 1)]
public class ShopItemData : ScriptableObject {

    public string itemName;
    public int price;
    public Mesh mesh;
    public Vector3 rotation;
    public PIECE_CATEGORY category;

}
