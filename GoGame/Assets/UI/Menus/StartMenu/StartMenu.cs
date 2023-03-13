using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Text coinText;
    public Text scoreText;

    void Start()
    {
        coinText.text = PlayerPrefs.GetInt("coinsTotal").ToString();
        scoreText.text = PlayerPrefs.GetInt("maxScore").ToString();
    }
}
