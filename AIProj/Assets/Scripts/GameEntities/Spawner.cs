using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns enemies

public class Spawner : MonoBehaviour
{ 
    public GameObject prefab;
    public float spawnTime;
    public int maxSpawns;

    float elapsedTime;
    int spawns;
    List<Index> path;
    Vector3 spawnPos;

    void Start()
    {
        spawnPos = transform.position;
        spawnPos.y = 1; 
        elapsedTime = Random.Range(0f, 1f);
        spawns = 0;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(spawns < maxSpawns && elapsedTime >= spawnTime) { SpawnEnemy(); }
    }

    public void SetValues(List<Index> _path)
    {
        path = _path;
    }

    void SpawnEnemy()
    {
        elapsedTime = 0f;
        spawns++;

        if (null == path) { return; }      

        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
        Enemy e = obj.GetComponent<Enemy>();
        Enemy.enemies.Add(obj.transform);
        
        e.Set(path);
    }

        
}

