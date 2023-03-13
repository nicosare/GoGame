using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPlacer : MonoBehaviour
{
    public Transform cam;
    public Transform player;
    public Chunk[] chunks;
    public Chunk firstChunk;
    private int renderDistance;
    private List<Chunk> spawnedChunks = new List<Chunk>();

    public GameObject coin;
    public GameObject deathObstacle;
    public GameObject obstacle;
    public GameObject car;

    void Start()
    {
        renderDistance = PlayerPrefs.HasKey("renderDistance") ? PlayerPrefs.GetInt("renderDistance") : 100;
        spawnedChunks.Add(firstChunk);
    }

    void Update()
    {
        if (player.position.z > spawnedChunks[spawnedChunks.Count - 1].end.position.z - renderDistance)
        {
            SpawnChunk();
        }

        if (cam.position.z - spawnedChunks[0].end.position.z > 0)
        {
            Destroy(spawnedChunks[0].gameObject);
            spawnedChunks.RemoveAt(0);
        }
    }

    private void SpawnChunk()
    {
        Chunk newChunk = Instantiate(chunks[Random.Range(0, chunks.Length)]);
        var endPos = spawnedChunks[spawnedChunks.Count - 1].end.position - newChunk.begin.localPosition;
        newChunk.transform.position = new Vector3(endPos.x, -5, endPos.z);
        StartCoroutine(MoveCoroutine(newChunk, newChunk.transform.position, 5 * Vector3.up));
        SetLogic(newChunk);
    }
    IEnumerator MoveCoroutine(Chunk chunk, Vector3 startPosition, Vector3 offset)
    {
        while (chunk.transform.position != startPosition + offset)
        {
            yield return new WaitForFixedUpdate();
            chunk.transform.position = Vector3.MoveTowards(chunk.transform.position, startPosition + offset, 10 * Time.deltaTime);
        }
    }

    private void SetLogic(Chunk newChunk)
    {
        System.Random rnd = new System.Random();

        newChunk.Generate(coin, 2, 1);

        if (newChunk.tag != "CarsChunk" && newChunk.tag != "LabyrinthChunk" && newChunk.tag != "DeathObstacleChunk")
            newChunk.Generate(obstacle, 10, 0);
        
        if (newChunk.tag == "LabyrinthChunk")
            newChunk.Generate(deathObstacle, 100, 0);
        
        if (newChunk.tag == "DeathObstacleChunk")
            newChunk.Generate(deathObstacle, 20, 0);
        
        if (newChunk.tag == "CarsChunk")
            newChunk.GenerateMoving(car, rnd.Next(2));
        
        spawnedChunks.Add(newChunk);
    }
}
