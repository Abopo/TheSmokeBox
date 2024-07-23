using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// A slot for a wood piece in the Wood Shop.
/// </summary>
public class WoodSlot : MonoBehaviour {

    [SerializeField]
    TextMeshPro _nameText;
    [SerializeField]
    TextMeshPro _priceText;
    [SerializeField]
    MeshFilter _filter;
    [SerializeField]
    GameObject _backer;

    ShopItemData _data;

    BoxCollider _boxCollider;

    WoodShop _shop;

    public string ItemName { 
        get => _data.itemName;
        set {
            _data.itemName = value;
            _nameText.text = _data.itemName;
        }
    }
    public int Price {
        get => _data.price;
        set {
            _data.price = value;
            _priceText.text = _data.price.ToString();
        }
    }
    public Mesh Mesh {
        get => _data.mesh;
        set {
            _data.mesh = value;
            _filter.mesh = _data.mesh;
        }
    }

    public ShopItemData Data { get => _data; set => _data = value; }

    private void Awake() {
        _boxCollider = GetComponent<BoxCollider>();
        _shop = GetComponentInParent<WoodShop>();
        _data = ScriptableObject.CreateInstance("ShopItemData") as ShopItemData;
    }
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter() {
        _backer.SetActive(true);
    }

    private void OnMouseExit() {
        _backer.SetActive(false);
    }

    private void OnMouseDown() {
        _shop.PurchaseFromSlot(this);
    }

    public void EnableSlot() {
        _boxCollider.enabled = true;
    }
    public void DisableSlot() {
        ItemName = "";
        Price = 0;
        Mesh = null;

        _boxCollider.enabled = false;
    }
}
