using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int direction;
    public List<GameObject> spawnerObjects;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0.2f, Random.Range(5f, 8f));
    }

    private void Spawn()
    {
        var index = Random.Range(0, spawnerObjects.Count);
        var target = Instantiate(spawnerObjects[index], transform.position, Quaternion.identity, transform);

        target.GetComponent<MoveForward>().dir = direction;
    }
}
