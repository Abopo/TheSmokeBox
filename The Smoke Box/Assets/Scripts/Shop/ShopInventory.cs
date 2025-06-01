using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopInventory", menuName = "ScriptableObjects/ShopInventory", order = 2)]
public class ShopInventory : ScriptableObject {

    public ShopItemData[] inventory;
    int _size;

    public void Initialize(int size) {
        _size = size;
        inventory = new ShopItemData[_size];
    }

    public void ClearInventory() {
        inventory = new ShopItemData[_size];
    }
}
