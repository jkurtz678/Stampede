using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject buffaloid;
    public Transform[] spawnSpots;
    private float timeBtwSpawns;
    public float startTimeBtwSpawns;

    // Start is called before the first frame update
    void Start()
    {
        timeBtwSpawns = startTimeBtwSpawns;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwSpawns <= 0)
        {
            Instantiate(buffaloid, spawnSpots[0].position, Quaternion.identity);
            timeBtwSpawns = startTimeBtwSpawns;
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }
}
