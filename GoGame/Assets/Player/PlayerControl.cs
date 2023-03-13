using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public Transform playerPos;
    public Player player;
    public Animator animator;
    public bool canMove;
    public float speed;
    private int score;
    public Text scoreText;
    public Text coinText;
    private float maxDistance;

    public bool firstMove;

    private void Start()
    {
        coinText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        firstMove = false;
        maxDistance = float.MinValue;
        canMove = true;
        SwipeManager.instance.MoveEvent += MovePlayer;
        SwipeManager.instance.ClickEvent += Click;
    }

    void MovePlayer(bool[] swipes)
    {
        firstMove = true;
        if (swipes[(int)SwipeManager.Direction.Left] && player.canMoveLeft)
            StartCoroutine(MoveCoroutine(playerPos.position, Vector3.left));
        if (swipes[(int)SwipeManager.Direction.Right] && player.canMoveRight)
            StartCoroutine(MoveCoroutine(playerPos.position, Vector3.right));
        if (swipes[(int)SwipeManager.Direction.Down] && player.canMoveBack)
            StartCoroutine(MoveCoroutine(playerPos.position, Vector3.back));
        if (canMove)
            animator.Play("Move");
    }

    void Click(Vector2 touchPosition)
    {
        firstMove = true;
        if (player.canMoveForward)
        {
            StartCoroutine(MoveCoroutine(playerPos.position, Vector3.forward));
        }
        if (canMove)
            animator.Play("Move");
    }

    private void Update()
    {
        if (player.environment.Count <= 0 && firstMove)
        {
            StartCoroutine(MoveCoroutine(playerPos.position, Vector3.down));
        }

        if (transform.position.z > maxDistance)
            maxDistance = transform.position.z;

        if (canMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.Play("PrepareToMove");
            }
        }

        score = maxDistance > 0 ? (int)(maxDistance + 0.5f) : 0;
        PlayerPrefs.SetInt("score", score);
        if (PlayerPrefs.HasKey("maxScore"))
        {
            if (PlayerPrefs.GetInt("score") > PlayerPrefs.GetInt("maxScore"))
            {
                PlayerPrefs.SetInt("maxScore", score);
            }
        }
        else
            PlayerPrefs.SetInt("maxScore", 0);
        PlayerPrefs.Save();


        if (firstMove)
        {
            scoreText.text = score.ToString();
            coinText.text = player.coinCount.ToString();
        }
    }

    IEnumerator MoveCoroutine(Vector3 startPosition, Vector3 offset)
    {
        if (canMove)
            while (playerPos.position != startPosition + offset)
            {
                yield return new WaitForFixedUpdate();
                playerPos.transform.position = Vector3.MoveTowards(playerPos.position, startPosition + offset, speed * Time.deltaTime);
            }
    }
}
