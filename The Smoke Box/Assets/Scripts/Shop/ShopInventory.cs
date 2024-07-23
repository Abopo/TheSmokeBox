using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopInventory", menuName = "ScriptableObjects/ShopInventory", order = 2)]
public class ShopInventory : ScriptableObject {

    public ShopItemData[] inventory;
    
}
