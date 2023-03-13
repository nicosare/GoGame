using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public List<PlayerModel> skins;
    public SnapScrolling snapScrolling;
    public GameObject buySkinButton;
    public GameObject applySkinButton;

    private void Awake()
    {
        foreach (var skin in skins)
        {
            if (!PlayerPrefs.HasKey(skin.modelName + "isOpen") && !PlayerPrefs.HasKey(skin.modelName + "isSelected"))
            {
                PlayerPrefs.SetInt(skin.modelName + "isOpen", 0);
                PlayerPrefs.SetInt(skin.modelName + "isSelected", 0);

                if (skin.modelName == "GREEN CUBE")
                {
                    PlayerPrefs.SetInt(skin.modelName + "isOpen", 1);
                    PlayerPrefs.SetInt(skin.modelName + "isSelected", 1);
                }
            }
        }
    }

    private void FixedUpdate()
    {

        if (PlayerPrefs.GetInt(skins[snapScrolling.selectedPanID].modelName + "isOpen") == 0)
        {
            applySkinButton.SetActive(false);
            buySkinButton.SetActive(true);
        }

        if (PlayerPrefs.GetInt(skins[snapScrolling.selectedPanID].modelName + "isOpen") == 1
            && PlayerPrefs.GetInt(skins[snapScrolling.selectedPanID].modelName + "isSelected") == 0)
        {
            applySkinButton.GetComponent<Button>().interactable = true;
            applySkinButton.SetActive(true);
            buySkinButton.SetActive(false);
            applySkinButton.transform.GetChild(0).GetComponent<Text>().text = "APPLY SKIN";

        }
        if (PlayerPrefs.GetInt(skins[snapScrolling.selectedPanID].modelName + "isSelected") == 1)
        {
            applySkinButton.SetActive(true);
            buySkinButton.SetActive(false);
            applySkinButton.GetComponent<Button>().interactable = false;
            applySkinButton.transform.GetChild(0).GetComponent<Text>().text = "SELECTED";
        }
    }

    public void ApplySkinButton()
    {
        PlayerPrefs.SetInt(skins[snapScrolling.selectedPanID].modelName + "isSelected", 1);
        PlayerPrefs.SetInt("playerModel", snapScrolling.selectedPanID);
        foreach (var skin in skins)
            if (PlayerPrefs.GetInt(skin.modelName + "isSelected") == 1 && skin.modelName != skins[snapScrolling.selectedPanID].modelName)
                PlayerPrefs.SetInt(skin.modelName + "isSelected", 0);
    }

    public void BuySkinButton()
    {
        var coinsTotal = PlayerPrefs.GetInt("coinsTotal");
        if (coinsTotal >= skins[snapScrolling.selectedPanID].price)
        {
            coinsTotal -= skins[snapScrolling.selectedPanID].price;
            PlayerPrefs.SetInt("coinsTotal", coinsTotal);
            buySkinButton.SetActive(false);
            applySkinButton.SetActive(true);
            PlayerPrefs.SetInt(skins[snapScrolling.selectedPanID].modelName + "isOpen", 1);
        }
        else
        {
            return;
        }
    }
}
