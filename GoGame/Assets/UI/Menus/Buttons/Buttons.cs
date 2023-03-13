using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public GameObject loseGameMenu;
    public SceneLoader sceneLoader;
    public Settings settings;

    public void RestartButton()
    {
        Time.timeScale = 1f;
        sceneLoader.FadeToLevel(SceneManager.GetActiveScene().name);
        StartCoroutine(SetFalse(loseGameMenu));
    }

    private IEnumerator SetFalse(GameObject objectToFalse)
    {
        yield return new WaitForSeconds(0.5f);
        objectToFalse.SetActive(false);
    }

    public void MenuButton()
    {
        Time.timeScale = 1f;
        sceneLoader.FadeToLevel("StartMenu");
    }

    public void StartGameButton()
    {
        Time.timeScale = 1f;
        sceneLoader.FadeToLevel("Level");
    }
    public void ShopButton()
    {
        Time.timeScale = 1f;
        sceneLoader.FadeToLevel("Shop");
    }

    public void SettingsButton()
    {
        sceneLoader.FadeToLevel("SettingsMenu");
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public void ResetButton()
    {
        PlayerPrefs.DeleteKey("renderDistance");
        PlayerPrefs.DeleteKey("distanceToPlayer");
        sceneLoader.FadeToLevel(SceneManager.GetActiveScene().name);
    }

    public void ApplyButton()
    {
        PlayerPrefs.SetInt("renderDistance", (int)settings.renderDistance.value);
        PlayerPrefs.SetInt("distanceToPlayer", (int)settings.cameraRange.value);
        sceneLoader.FadeToLevel(SceneManager.GetActiveScene().name);
    }

}
