using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodShop : MonoBehaviour {

    [SerializeField]
    ShopInventory _inventory;

    WoodSlot[] _slots;

    ReceiptWindow _receiptWindow;

    public bool turningPage;
    public bool lastPage;

    private void Awake() {
        _slots = GetComponentsInChildren<WoodSlot>();
        _receiptWindow = FindFirstObjectByType<ReceiptWindow>();
    }
    // Start is called before the first frame update
    void Start() {
        if (_inventory != null) {
            FillSlotsWithInventory();
        } else {
            _inventory = new ShopInventory();
            _inventory.Initialize(_slots.Length);
            FillSlotsRandomly();
        }
    }

    void FillSlotsWithInventory() {
        for (int i = 0; i < _slots.Length; i++) {
            if(i < _inventory.inventory.Length) {
                _slots[i].SetData(_inventory.inventory[i]);

                _slots[i].EnableSlot();
            } else {
                _slots[i].DisableSlot();
            }
        }
    }

    void FillSlotsRandomly() {
        CreateRandomInventory();

        for (int i = 0; i < _slots.Length; ++i) {
            _slots[i].SetData(_inventory.inventory[i]);
        }
    }

    void CreateRandomInventory() {
        ShopItemData tempItemData;

        ShopItemData[] allItems = Resources.LoadAll<ShopItemData>("SOs/WoodPieces");
        int rand;

        _inventory.ClearInventory();
        
        for (int i = 0; i < _inventory.inventory.Length; ++i) {
            rand = Random.Range(0, allItems.Length);
            tempItemData = allItems[rand];

            tempItemData.type = (WOOD_TYPE)Random.Range(0, (int)WOOD_TYPE.NUM_TYPES);

            _inventory.inventory[i] = tempItemData;
        }
    }

    public void PurchaseFromSlot(WoodSlot slot) {
        _receiptWindow.AddItemToWindow(slot);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void RerollShop() {
        // Cost money
        GameManager.Instance.IncurCost(5);

        CreateRandomInventory();
        StartCoroutine(ChangePage(_inventory.inventory));
    }

    public IEnumerator ChangePage(ShopItemData[] newItems) {
        turningPage = true;

        for (int i = 0; i < _slots.Length; ++i) {
            _slots[i].ChangeItem(newItems[i]);

            yield return new WaitForSeconds(0.1f);
        }

        turningPage = false;
    }

    public IEnumerator ChangePage() {
        turningPage = true;
        int _check = 0;
        for (int i = 0; i < 4; i++)
        {
            for(int j = i; j <= i + 8; j += 4)
            {
                if (j + (lastPage ? 0 : 12) < _inventory.inventory.Length) {
                    _slots[j].ChangeItem(_inventory.inventory[j + (lastPage ? 0 : 12)]);
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
            lastPage = false;
        } else {
            lastPage = true;
        }

        turningPage = false;
    }

    public void DisableMenu() {
        for (int i = 0; i < _slots.Length; ++i) {
            _slots[i].isActive = false;
        }
    }

    public void EnableMenu() {
        for (int i = 0; i < _slots.Length; ++i) {
            _slots[i].isActive = true;
        }
    }
}
