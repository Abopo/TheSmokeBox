using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemData", menuName = "ScriptableObjects/ShopItemData", order = 1)]
public class ShopItemData : ScriptableObject {

    public string itemName;
    public int price;
    public Mesh mesh;

}
