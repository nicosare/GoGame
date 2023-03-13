using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public GameObject[] models;

    private void Start()
    {
        var model = Instantiate(models[Random.Range(0, models.Length)], transform);
    }
}
