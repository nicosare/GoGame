using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Transform begin;
    public Transform end;
    public void Generate(GameObject objectToSpawn, int chance, int evenOdd)
    {
        System.Random rnd = new System.Random();
        var spawnPositions = new List<Vector3>();


        foreach (Transform obj in transform)
            if (obj.name == "Ground")
                spawnPositions.Add(obj.position);

        var strokes = spawnPositions.OrderBy(position => position.z)
                        .ThenBy(position => position.x)
                        .GroupBy(position => position.z);

        foreach (var stroke in strokes)
        {
            var positions = new Dictionary<Vector3, int>();

            foreach (var pos in stroke)
                positions.Add(pos, rnd.Next(100 / chance));

            if (positions.All(pos => pos.Value == 0))
            {
                positions[new Vector3(stroke.ElementAt(rnd.Next(stroke.Count())).x, -5, stroke.Key)] = 1;
            }

            foreach (var position in positions)
                if ((position.Key.z + 0.5f) % 2 == evenOdd && position.Value == 0)
                {
                    Instantiate(objectToSpawn, position.Key + Vector3.up, Quaternion.identity.normalized, transform);
                }
            positions.Clear();
        }
        spawnPositions.Clear();

    }

    public void GenerateMoving(GameObject objectToMove, int direction)
    {
        System.Random rnd = new System.Random();
        StartCoroutine(MovingCoroutine(objectToMove, direction, (float)rnd.Next(5, 20)));
    }

    private IEnumerator MovingCoroutine(GameObject objectToMove, int direction, float speed)
    {
        System.Random rnd = new System.Random();
        var dir = direction == 1 ? Vector3.left : Vector3.right;

        var startPos = direction == 1 ? 6 : -5;
        var spawnPos = new Vector3(startPos, 1, 0.5f);

        while (true)
        {
            var obj = Instantiate(objectToMove, spawnPos + transform.position, Quaternion.identity.normalized, transform);
            if (direction == 0)
            {
                obj.transform.GetChild(0).transform.Rotate(0, 180, 0);
                while (obj.transform.localPosition.x <= 6)
                {
                    yield return new WaitForFixedUpdate();
                    obj.transform.Translate(dir * speed / 75);

                    if (obj.transform.localPosition.x >= 6)
                    {
                        Destroy(obj.gameObject);
                    }
                }
            }
            else
                while (obj.transform.localPosition.x >= -5)
                {
                    yield return new WaitForFixedUpdate();
                    obj.transform.Translate(dir * speed / 75);

                    if (obj.transform.localPosition.x <= -5)
                    {
                        Destroy(obj.gameObject);
                    }
                }
            yield return new WaitForSeconds(rnd.Next(1, 30) / 10);
        }
    }
}

