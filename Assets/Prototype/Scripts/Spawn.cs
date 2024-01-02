using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform[] tranSpawn;

    float time = 1.5f;

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 1.5f;
            int ran = Random.Range(0, tranSpawn.Length);
            Instantiate(enemyPrefab, tranSpawn[ran]);
        }
    }
}
