using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodShop : MonoBehaviour {

    [SerializeField]
    ShopInventory _inventory;

    WoodSlot[] _slots;

    ReceiptWindow _receiptWindow;

    private void Awake() {
        _slots = GetComponentsInChildren<WoodSlot>();
        _receiptWindow = FindObjectOfType<ReceiptWindow>();
    }
    // Start is called before the first frame update
    void Start() {
        FillSlotsWithInventory();
    }

    void FillSlotsWithInventory() {
        for (int i = 0; i < _slots.Length; i++) {
            if(i < _inventory.inventory.Length) {
                _slots[i].ItemName = _inventory.inventory[i].name;
                _slots[i].Price = _inventory.inventory[i].price;
                _slots[i].Mesh = _inventory.inventory[i].mesh;

                _slots[i].EnableSlot();
            } else {
                _slots[i].DisableSlot();
            }
        }
    }

    public void PurchaseFromSlot(WoodSlot slot) {
        _receiptWindow.AddItemToWindow(slot);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
