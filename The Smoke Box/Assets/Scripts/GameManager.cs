using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int playerMoney = 10;
    public List<ShopItemData> playerInventory = new List<ShopItemData>();

    public int stage; // 1, 2, or 3 depending on which stage of the contest we're in.
    public Stage curStage;

    // Judges topic lists
    public List<TOPIC> chippTopics = new List<TOPIC>();
    public List<TOPIC> jambonTopics = new List<TOPIC>();
    public List<TOPIC> pitmasterTopics = new List<TOPIC>();

    public static GameManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            DestroyImmediate(gameObject);
        }
    }

    private void Start() {
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        curStage = GetComponent<Stage>();
        if (curStage != null) {
            curStage.Initialize();
        }
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void IncreaseStage() {
        stage += 1;

        // Since we're moving to another stage, clear our inventory
        playerInventory.Clear();
    }

    public void AddMoney(int amount) {
        playerMoney += amount;
    }
    public void IncurCost(int cost) {
        playerMoney -= cost;
        if (playerMoney < 0) {
            playerMoney = 0;
        }
    }
}
