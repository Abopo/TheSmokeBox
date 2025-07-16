using UnityEngine;

public class WoodInfoMenu : MonoBehaviour
{
    [SerializeField]
    SuperTextMesh nameText;
    [SerializeField]
    SuperTextMesh typeText;
    [SerializeField]
    SuperTextMesh catText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMenu() {
        gameObject.SetActive(true);
    }

    public void HideMenu() {
        gameObject.SetActive(false);
    }

    public void SetInfo(ShopItemData data) {
        nameText.text = data.name;
        typeText.text = data.type.ToString();
        catText.text = data.category.ToString();
    }
}
