using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public PlayerControl playerControl;
    public GameObject loseGameMenu;
    public GameObject score;
    public GameObject coinText;

    public List<GameObject> models;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("playerModel"))
            Instantiate(models[PlayerPrefs.GetInt("playerModel")], transform.position, Quaternion.identity.normalized, transform);
        else
            Instantiate(models[0], transform.position, Quaternion.identity.normalized, transform);
    }

    public void SetCanMoveFalse()
    {
        playerControl.canMove = false;
    }
    public void SetCanMoveTrue()
    {
        playerControl.canMove = true;
    }

    public void Die()
    {
        Time.timeScale = 0f;
        Destroy(this);
        loseGameMenu.SetActive(true);
        score.SetActive(false);
        coinText.SetActive(false);
    }
}
