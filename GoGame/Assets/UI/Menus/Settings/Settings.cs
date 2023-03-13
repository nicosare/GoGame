using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public Slider renderDistance;
    public Slider cameraRange;
    public ChunkPlacer chunkPlacer;

    private void Start()
    {
        renderDistance.value = PlayerPrefs.HasKey("renderDistance") ? PlayerPrefs.GetInt("renderDistance") : 100;
        cameraRange.value = PlayerPrefs.HasKey("distanceToPlayer") ? PlayerPrefs.GetInt("distanceToPlayer") : 0;
    }
}
