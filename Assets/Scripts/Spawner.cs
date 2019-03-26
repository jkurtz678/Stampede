using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject buffaloid;
    public Transform[] spawnSpots;
    public float startTimeBtwSpawns;
    public int maxSpawnCount;
    private float timeBtwSpawns;
    private int spawnCount;

    // Start is called before the first frame update
    void Start()
    {
        timeBtwSpawns = startTimeBtwSpawns;
        spawnCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwSpawns <= 0 && spawnCount < maxSpawnCount)
        {
            Instantiate(buffaloid, spawnSpots[0].position, Quaternion.identity);
            timeBtwSpawns = startTimeBtwSpawns;
            spawnCount++;
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }
}
