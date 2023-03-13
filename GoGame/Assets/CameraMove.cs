using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
    public Animator animator;
    public Transform playerPos;
    public Player player;
    public PlayerControl playerControl;

    private float deltaZ;

    private float speed;
    public bool isEndAnimating;
    private int minMaxOffsetX;
    private int countMove;
    public int distanceToPlayer;
    public bool isCanMove;

    void Start()
    {
        countMove = 0;
        distanceToPlayer = PlayerPrefs.HasKey("distanceToPlayer") ? PlayerPrefs.GetInt("distanceToPlayer") : 0;
        minMaxOffsetX = distanceToPlayer <= 0 ? 1 : distanceToPlayer;
        isCanMove = true;
        speed = 0.025f;
        isEndAnimating = true;
        transform.position = new Vector3(0.5f, 10 - distanceToPlayer, -15.5f + 2 * distanceToPlayer);
    }

    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            transform.Translate(Vector3.forward * 0.015f);
        }
        else if (SceneManager.GetActiveScene().name == "SettingsMenu")
        {
            transform.Translate(Vector3.zero);
        }
        else if (playerControl.firstMove)
        {
            CheckDistance();

            transform.Translate(Vector3.forward * speed);

            if (transform.position.x > playerPos.position.x && isCanMove && countMove > -minMaxOffsetX)
            {
                countMove--;
                isCanMove = false;
                StartCoroutine(MoveCoroutineLeft(transform.position.x));
            }

            if (transform.position.x < playerPos.position.x && isCanMove && countMove < minMaxOffsetX)
            {
                countMove++;
                isCanMove = false;
                StartCoroutine(MoveCoroutineRight(transform.position.x));
            }
        }
    }

    IEnumerator MoveCoroutineLeft(float startX)
    {
        while (transform.position.x > startX - 1)
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(Vector3.left * 0.2f);
        }
        isCanMove = true;
        transform.position = new Vector3(startX - 1, transform.position.y, transform.position.z);
    }

    IEnumerator MoveCoroutineRight(float startX)
    {
        while (transform.position.x < startX + 1)
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(Vector3.right * 0.2f);
        }
        isCanMove = true;
        transform.position = new Vector3(startX + 1, transform.position.y, transform.position.z);
    }

    private void CheckDistance()
    {
        deltaZ = Mathf.Abs(transform.position.z - playerPos.position.z);

        if (deltaZ >= 11 - distanceToPlayer && deltaZ <= 15 - distanceToPlayer && isEndAnimating)
        {
            speed = 0.025f;
            animator.Play("Idle");
        }

        if (deltaZ > 15 - distanceToPlayer * 1.2)
        {
            speed = deltaZ / 180;
        }

        if (deltaZ < 11 - distanceToPlayer * 1.2)
        {
            speed = 0.015f;
            animator.Play("Signal");
            isEndAnimating = false;
        }

        if (deltaZ < 8 - distanceToPlayer)
        {
            player.Die();
        }
    }


}
