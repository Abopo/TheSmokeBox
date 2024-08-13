using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodShop : MonoBehaviour {

    [SerializeField]
    ShopInventory _inventory;

    WoodSlot[] _slots;

    ReceiptWindow _receiptWindow;

    int _playerMoney = 10;

    public bool _lastPage;

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
                _slots[i].SetData(_inventory.inventory[i]);
                //_slots[i].ItemName = _inventory.inventory[i].itemName;
                //_slots[i].Price = _inventory.inventory[i].price;
                //_slots[i].Mesh = _inventory.inventory[i].mesh;

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

    public IEnumerator ChangePage() {
        int _check = 0;
        for (int i = 0; i < 4; i++)
        {
            for(int j = i; j <= i + 8; j += 4)
            {
                if (j + (_lastPage ? 0 : 9) < _inventory.inventory.Length) {
                    _slots[j].ChangeItem(_inventory.inventory[j + (_lastPage ? 0 : 9)]);
                    _check++;
                }
                else if(j < _slots.Length) {
                    _slots[j].ChangeItem(null);
                }
            }

            yield return new WaitForSeconds(0.1f);
        }

        // TODO: this fails if the last page actually fills in all the slots
        if(_check == 12) {
            _lastPage = false;
        } else {
            _lastPage = true;
        }
    }
}
