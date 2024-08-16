using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public List<ShopItemData> playerInventory = new List<ShopItemData>();

    public int stage; // 1, 2, or 3 depending on which stage of the contest we're in.

    // Judges topic lists
    public List<TOPIC> chippTopics = new List<TOPIC>();
    public List<TOPIC> jambonTopics = new List<TOPIC>();
    public List<TOPIC> pitmasterTopics = new List<TOPIC>();

    public static GameManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            DestroyImmediate(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void IncreaseStage() {
        stage += 1;
        if(stage > 3) {
            stage = 3;
        }

        // Since we're moving to another stage, clear our inventory
        playerInventory.Clear();
    }
}
