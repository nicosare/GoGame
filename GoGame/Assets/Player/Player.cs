using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canMoveForward = true;
    public bool canMoveLeft = true;
    public bool canMoveRight = true;
    public bool canMoveBack = true;
    public Animator animator;
    public List<GameObject> environment;
    public int coinCount;

    void Start()
    {
        if (!PlayerPrefs.HasKey("coinsTotal"))
            PlayerPrefs.SetInt("coinsTotal", 0);
        coinCount = 0;
        Time.timeScale = 1f;
        transform.position = new Vector3(0.5f, 1f, -0.5f);
    }


    private void FixedUpdate()
    {
        CheckEnvironment();
        if (transform.position.y < 1)
        {
            Die();
        }
    }

    private void CheckEnvironment()
    {
        foreach (var obj in environment)
        {
            var delta = obj.transform.position - transform.position;
            if (obj.tag == "Obstacle" || obj.tag == "Wall")
            {
                if (delta == Vector3.left)
                    canMoveLeft = false;
                if (delta == Vector3.right)
                    canMoveRight = false;
                if (delta == Vector3.forward)
                    canMoveForward = false;
                if (delta == Vector3.back)
                    canMoveBack = false;
            }
            if (obj.tag == "Coin")
            {
                environment.Remove(obj);
                obj.SetActive(false);
                coinCount++;
                PlayerPrefs.SetInt("coinsTotal", PlayerPrefs.GetInt("coinsTotal") + 1);

            }
            PlayerPrefs.Save();
            if (obj.tag == "DeathObstacle" || obj.tag == "Car")
            {
                Die();
            }

        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        environment.Add(collision.gameObject);
    }

    public void OnCollisionExit(Collision collision)
    {
        environment.Remove(collision.gameObject);
        canMoveForward = true;
        canMoveLeft = true;
        canMoveRight = true;
        canMoveBack = true;
    }

    public void Die()
    {
        animator.Play("Die");
    }
}
