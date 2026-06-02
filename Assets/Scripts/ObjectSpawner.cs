using UnityEngine;
using System.Collections.Generic;
public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] Transform minPos;
    [SerializeField] Transform maxPos;
    [SerializeField] int waveNumber;
    [SerializeField] List<Wave> waves;

    
    
    // Update is called once per frame
    void Update()
    {
        // // 0 + 1 = 1
        waves[waveNumber].spawnTimer += Time.deltaTime * GameManager.instance.mapSpeed;
        // // if spawntimer = 1, its higher than spawnInterval. Reset back to 0
        if(waves[waveNumber].spawnTimer >= waves[waveNumber].spawnInterval)
        {
            waves[waveNumber].spawnTimer = 0;
            SpawnObject();
        }    
        // if spawnedObjectCount is higher than objectsPerWave. wavenumber index goes up by 1
        // therefore starting the next wave
        if(waves[waveNumber].spawnedObjectCount >= waves[waveNumber].objectsPerWave)
        {
            // reset spawnObjectCount to 0 when restarting previous waves
            waves[waveNumber].spawnedObjectCount = 0;
            waveNumber++;
            // if waveNumber index is higher than total amount of waves, restart from first wave again.
            if(waveNumber >= waves.Count)
            {
                waveNumber = 0;
            }
        }
    }

    [System.Serializable]
    public class Wave
    {
        // we're switching over to objectpooling vs prefabs
        //public GameObject prefab;
        public ObjectPooler pool;
        public float spawnTimer;
        public float spawnInterval;
        public int objectsPerWave;
        public int spawnedObjectCount;        
    }


    void SpawnObject()
    {
        // picks the correct object pooler based on what wave number instead of using one for all waves
        GameObject spawnObject = waves[waveNumber].pool.GetPooledObject();
        spawnObject.transform.position = RandomSpawnPoint();
        // deactivate spawnObject.transform.rotation because it overwrites every object spawning in
        // now every gameobject spawns in based on their rotation depicted in their scripts;
        //spawnObject.transform.rotation = transform.rotation;
        spawnObject.SetActive(true);
        //Instantiate(waves[waveNumber].prefab, RandomSpawnPoint(), transform.rotation, transform);
        // for every object spawn, the spawnedobjectcount goes up by 1
        waves[waveNumber].spawnedObjectCount++;
    }


    Vector2 RandomSpawnPoint()
    {
        Vector2 spawnPoint;
        spawnPoint.x = minPos.position.x;
        spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);

        return spawnPoint;   
    }
}
