using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReceiptItem : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI _nameText;

    [SerializeField]
    TextMeshProUGUI _priceText;

    public ShopItemData itemData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitializeItem(ShopItemData data) {
        itemData = data;

        _nameText.text = data.itemName;
        _priceText.text = data.price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
