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
    GameObject _model;
    MeshRenderer _renderer;
    MeshFilter _filter;

    [SerializeField]
    GameObject _backer;

    [SerializeField]
    AudioClip _hoverClip;
    [SerializeField]
    AudioClip _selectClip;
    AudioSource _audioSource;

    ShopItemData _data;

    ShopItemData _nextData;

    BoxCollider _boxCollider;

    Animator _animator;

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
        _animator = GetComponent<Animator>();

        _renderer = _model.GetComponent<MeshRenderer>();
        _filter = _model.GetComponent<MeshFilter>();

        _audioSource = GetComponent<AudioSource>();
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

        _audioSource.clip = _hoverClip;
        _audioSource.Play();
    }

    private void OnMouseExit() {
        _backer.SetActive(false);
    }

    private void OnMouseDown() {
        _shop.PurchaseFromSlot(this);

        _audioSource.clip = _selectClip;
        _audioSource.Play();
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
    public void ChangeItem(ShopItemData nextData) {
        _nextData = nextData;

        _animator.Play("WS_ChangeItem");
    }

    public void SetData(ShopItemData inData) {
        _data = inData;
        ItemName = inData.itemName;
        Price = inData.price;
        Mesh = inData.mesh;
        _filter.transform.localRotation = Quaternion.Euler(inData.rotation);

        // Set wood material
        switch (_data.type) {
            case WOOD_TYPE.ACACIA:
                _renderer.material = Resources.Load<Material>("Materials/Wood/Wood_Acacia");
                break;
            case WOOD_TYPE.ASH:
                _renderer.material = Resources.Load<Material>("Materials/Wood/Wood_Ash");
                break;
            case WOOD_TYPE.BEECH:
                _renderer.material = Resources.Load<Material>("Materials/Wood/Wood_Beech");
                break;
            case WOOD_TYPE.OAK:
                _renderer.material = Resources.Load<Material>("Materials/Wood/Wood_Oak");
                break;
            case WOOD_TYPE.SPRUCE:
                _renderer.material = Resources.Load<Material>("Materials/Wood/Wood_Spruce");
                break;
            case WOOD_TYPE.WALNUT:
                _renderer.material = Resources.Load<Material>("Materials/Wood/Wood_Walnut");
                break;
        }
    }

    public void SwapModel() {
        if (_nextData != null) {
            SetData(_nextData);
            EnableSlot();
        } else {
            DisableSlot();
        }
    }
}
