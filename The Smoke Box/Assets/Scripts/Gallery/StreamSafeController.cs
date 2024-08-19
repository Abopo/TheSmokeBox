using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StreamSafeController : MonoBehaviour
{
    private Toggle _streamSafeToggle;

    // Start is called before the first frame update
    void Start()
    {
        _streamSafeToggle = GetComponent<Toggle>();

        if (PlayerPrefs.HasKey("StreamSafe") == false)
        {
            PlayerPrefs.SetInt("StreamSafe", 1);
        }

        _streamSafeToggle.isOn = PlayerPrefs.GetInt("StreamSafe") == 1;
    }

    public void SetStreamSafeMode(bool isOn)
    {
        PlayerPrefs.SetInt("StreamSafe", isOn ? 1 : 0);
    }
}
