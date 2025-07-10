using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField]
    Stage _stage1;
    [SerializeField]
    Stage _stage2;

    [SerializeField]
    GameObject _stageView;

    WoodShop _shop;

    private void Awake() {
        _shop = FindFirstObjectByType<WoodShop>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _stage1.Initialize();
        _stage2.Initialize();
    }

    // Update is called once per frame
    void Update() {
    }

    public void ToggleStageView() {
        _stageView.SetActive(!_stageView.activeSelf);

        if (_stageView.activeSelf) {
            _shop.DisableMenu();
        } else {
            _shop.EnableMenu();
        }
    }

    public void OpenStageSelect() {
        _stageView.SetActive(true);
        _shop.DisableMenu();

        // Turn on buttons for stages
        _stage1.GetComponent<Button>().enabled = true;
        _stage2.GetComponent<Button>().enabled = true;
    }

    public void ChooseStage1() {
        AddStageToGameManager(_stage1);

        GameManager.Instance.LoadScene("Workshop");
    }

    public void ChooseStage2() {
        AddStageToGameManager(_stage2);

        GameManager.Instance.LoadScene("Workshop");
    }

    void AddStageToGameManager(Stage selectedStage) {
        var stage = GameManager.Instance.gameObject.AddComponent(selectedStage.GetType());
        ((Stage)stage).Requests = selectedStage.Requests;
    }
}
