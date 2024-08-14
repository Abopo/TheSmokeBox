using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReceiptWindow : MonoBehaviour {

    [SerializeField]
    GameObject _receiptItem;

    [SerializeField]
    Transform _window;

    [SerializeField]
    TextMeshProUGUI _totalText;

    [SerializeField]
    GameObject _checkoutButton;
    [SerializeField]
    TextMeshProUGUI _countText;

    ScrollRect _scrollRect;

    int _playerMoney;
    int _totalPrice;

    List<ReceiptItem> _receiptItems = new List<ReceiptItem>();

    private void Awake() {
        _scrollRect = GetComponentInChildren<ScrollRect>();
    }
    // Start is called before the first frame update
    void Start() {
        _playerMoney = GameManager.Instance.stage * 10;
        _totalPrice = 0;
        _totalText.text = _playerMoney.ToString();
        CheckCount();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void AddItemToWindow(WoodSlot item) {
        if (_playerMoney - _totalPrice >= item.Price) {
            ReceiptItem newItem = Instantiate(_receiptItem, _window).GetComponent<ReceiptItem>();
            newItem.InitializeItem(item.Data);
            newItem.receiptWindow = this;

            _receiptItems.Add(newItem);

            IncreaseTotal(item.Price);
            CheckCount();

            // Force scroll view to follow down
            Canvas.ForceUpdateCanvases();
            _scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
            _scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            _scrollRect.verticalNormalizedPosition = 0;
        } else {
            // Play error sound?

        }
    }

    public void RemoveItemFromWindow(ReceiptItem item) {
        _receiptItems.Remove(item);

        IncreaseTotal(-item.price);
        CheckCount();

        Destroy(item.gameObject);
    }

    void IncreaseTotal(int price) {
        _totalPrice += price;
        _totalText.text = (_playerMoney - _totalPrice).ToString();
    }

    void CheckCount() {
        _countText.text = _receiptItems.Count.ToString() + " / 5";
        if (_receiptItems.Count >= 5) {
            _countText.color = Color.green;
            _checkoutButton.SetActive(true);
        } else {
            _countText.color = Color.red;
            _checkoutButton.SetActive(false);
        }
    }

    public void Checkout() {
        SaveItems();

        // Load next scene
        GameManager.Instance.LoadScene("Workshop");
    }

    void SaveItems() {
        foreach (ReceiptItem item in _receiptItems) {
           GameManager.Instance.playerInventory.Add(item.itemData);
        }
    }
}
