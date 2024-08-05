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

    ScrollRect _scrollRect;

    int _totalPrice;

    List<ReceiptItem> _receiptItems = new List<ReceiptItem>();

    private void Awake() {
        _scrollRect = GetComponentInChildren<ScrollRect>();
    }
    // Start is called before the first frame update
    void Start() {
        _totalPrice = 0;
        _totalText.text = "0";
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void AddItemToWindow(WoodSlot item) {
        ReceiptItem newItem = Instantiate(_receiptItem, _window).GetComponent<ReceiptItem>();
        newItem.InitializeItem(item.Data);
        newItem.receiptWindow = this;

        _receiptItems.Add(newItem);

        IncreaseTotal(item.Price);

        // Force scroll view to follow down
        Canvas.ForceUpdateCanvases();
        _scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
        _scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        _scrollRect.verticalNormalizedPosition = 0;
    }

    public void RemoveItemFromWindow(ReceiptItem item) {
        _receiptItems.Remove(item);

        IncreaseTotal(-item.price);

        Destroy(item.gameObject);
    }

    void IncreaseTotal(int price) {
        _totalPrice += price;
        _totalText.text = _totalPrice.ToString();
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
