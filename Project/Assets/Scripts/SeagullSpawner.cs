using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullSpawner : MonoBehaviour
{
    public GameObject seagullPrefab;
    

    [SerializeField] private float spawnFrequency = 5;
    private float spawnTimer = 0;
    [SerializeField] private float difficultyIncreaseFrequency = 5;
    private float difficultyIncreaseTimer = 0;
    private float numberOfSeagullsEachSpawn = 1;

    [SerializeField] private float spawnHeight_min = 2;
    [SerializeField] private float spawnHeight_max = 5;
    [SerializeField] private float spawnDist = 20;
    
    void Awake()
    {
        difficultyIncreaseTimer = difficultyIncreaseFrequency;

    }

    void Update()
    {
        // UpdateTimers
        difficultyIncreaseTimer -= Time.deltaTime;
        spawnTimer -= Time.deltaTime;

        // Spawn new Seagulls, if there are no birds, spawn in some. 
        if(spawnTimer < 0 || Seagull.static_numberOfSeagulls == 0){
            spawnTimer = spawnFrequency;
            Spawn(numberOfSeagullsEachSpawn);
        }
        // Increase difficulty level
        if(difficultyIncreaseTimer < 0){
            difficultyIncreaseTimer = difficultyIncreaseFrequency;
            numberOfSeagullsEachSpawn++;
        }
    }

    void Spawn(float numberOfSpawns){
        for(int i= 0; i< numberOfSpawns; i++){
            Vector2 unitCircle = Random.insideUnitCircle;
            Vector3 randDir = new Vector3(unitCircle.x, 0, unitCircle.y);
            Vector3 spawnPos = transform.position + randDir * spawnDist;
            // Add some hight to spawn location
            spawnPos.y += Random.Range(spawnHeight_min,spawnHeight_max);

            GameObject go = Instantiate(seagullPrefab, spawnPos, Quaternion.identity);
            go.GetComponent<Seagull>().SetTarget(transform);
        }
    }
}
