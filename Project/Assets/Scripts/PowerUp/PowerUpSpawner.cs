using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] powerUpPrefabs;
    [SerializeField] GameObject bulletsPrefab;

    [SerializeField] private float spawnsPowerUpEvery = 5;
    private float powerUpTimer = 0;

    [SerializeField] private float spawnsBulletsEvery = 4;
    private float bulletsTimer = 0;


    [SerializeField] private float spawnDist_min;
    [SerializeField] private float spawnDist_max;


    //asd
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bulletsTimer -= Time.deltaTime;
        powerUpTimer -= Time.deltaTime;

        if(bulletsTimer < 0){
            Spawn(bulletsPrefab, 1);
            bulletsTimer = spawnsBulletsEvery;
        }

        
        if(powerUpTimer < 0){
            int index = Random.Range(0,powerUpPrefabs.Length);
            Spawn(powerUpPrefabs[index], 1);
            powerUpTimer = spawnsPowerUpEvery;
        }
        
    }
    
    void Spawn(GameObject prefab, float numberOfSpawns){
        for(int i= 0; i< numberOfSpawns; i++){
            Vector2 unitCircle = Random.insideUnitCircle;
            Vector3 randDir = new Vector3(unitCircle.x, 0, unitCircle.y);
            Vector3 spawnPos = new Vector3(transform.position.x, 0, transform.position.z) + randDir * Random.Range(spawnDist_min,spawnDist_max);
            
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }
}
