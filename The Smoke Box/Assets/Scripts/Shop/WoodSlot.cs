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
    TextMeshPro _woodText;
    [SerializeField]
    TextMeshPro _catText;
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
    int _price;

    BoxCollider _boxCollider;

    Animator _animator;

    WoodShop _shop;

    public bool isActive = true;

    public string ItemName {
        get => _data.itemName;
        set {
            _data.itemName = value;
            _nameText.text = _data.itemName;
        }
    }
    public int Price {
        get => _price;
        set {
            _price = value;
            _priceText.text = _price.ToString();
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
        if(!isActive) {
            return;
        }

        _backer.SetActive(true);

        _audioSource.clip = _hoverClip;
        _audioSource.Play();
    }

    private void OnMouseExit() {
        if (!isActive) {
            return;
        }
        
        _backer.SetActive(false);
    }

    private void OnMouseDown() {
        if (!isActive) {
            return;
        }
        
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

        _catText.text = inData.category.ToString();

        // Set wood material
        switch (_data.type) {
            case WOOD_TYPE.ACACIA:
                _woodText.text = "ACACIA";
                _renderer.material = Resources.Load<Material>("Materials/Wood/Wood_Acacia");
                Price += 1;
                break;
            case WOOD_TYPE.ASH:
                _woodText.text = "ASH";
                _renderer.material = Resources.Load<Material>("Materials/Wood/Wood_Ash");
                break;
            case WOOD_TYPE.BEECH:
                _woodText.text = "BEECH";
                _renderer.material = Resources.Load<Material>("Materials/Wood/Wood_Beech");
                Price += 1;
                break;
            case WOOD_TYPE.OAK:
                _woodText.text = "OAK";
                _renderer.material = Resources.Load<Material>("Materials/Wood/Wood_Oak");
                break;
            case WOOD_TYPE.SPRUCE:
                _woodText.text = "SPRUCE";
                _renderer.material = Resources.Load<Material>("Materials/Wood/Wood_Spruce");
                Price += 2;
                break;
            case WOOD_TYPE.WALNUT:
                _woodText.text = "WALNUT";
                _renderer.material = Resources.Load<Material>("Materials/Wood/Wood_Walnut");
                Price += 2;
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
