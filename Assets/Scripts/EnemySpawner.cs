using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfiguration> waveConfigurations;
    [SerializeField] float TimeBetweenWaves = 0f;
    [SerializeField] bool isLooping;
    //[SerializeField] GameObject bossPrefab;
    // ScoreKeeper scoreKeeper;
    // AudioPlayer audioPlayer;
    //Boss boss;
    //[SerializeField] WaveConfig currentWave;
    WaveConfiguration currentWave;
    public bool isSpawning;
    // Start is called before the first frame update
    
    void Awake()
    {
        // scoreKeeper = FindObjectOfType<ScoreKeeper>();
        // audioPlayer = FindObjectOfType<AudioPlayer>();
        //boss = FindObjectOfType<Boss>();
    }

    void Start()
    {
        StartCoroutine(SpawnEnemyWaves());   
    }
    void Update()
    {
        // StopWaveSpawn();
        // if(!isSpawning)
        // {
        //     StopAllCoroutines();
        //     SpawnBoss();
        // }    
    }

    public WaveConfiguration GetCurrentWave()
    {
        return currentWave;    
    }

    // void SpawnBoss()
    // {
    //    //boss.isMoving = true;
    //    //audioPlayer.BossMusic();
    //    bossPrefab.SetActive(true);          
    // }

    // void StopWaveSpawn()
    // {
    //     if(scoreKeeper.score >= 3000)
    //     {
    //         isSpawning = false;
    //     }
    // }

    IEnumerator SpawnEnemyWaves()
    {
        do
        {
            foreach(WaveConfiguration wave in waveConfigurations)
            {
                currentWave = wave;
                // GetEnemyCount is int that returns enemyprefabs.Count from WaveConfig
                for(int i = 0; i < currentWave.GetEnemyCount(); i++)
                {
                    Instantiate(currentWave.GetEnemyPrefab(i),
                            // get transform of first waypoint in the path
                            currentWave.GetStartingWayPoint().position,
                            // Changing z value of enemy results in them facing downwards 
                            Quaternion.Euler(0,0,0),
                            //Quaternion.identity,
                            // this makes every enemy instanitated a child of the enemyspawner.
                            transform);
                    yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());        
                }
                yield return new WaitForSeconds(TimeBetweenWaves);
            }
        }
        while(isLooping);
    }

    // this function below is for one wave
    // IEnumerator SpawnEnemies()
    // {
    //     // GetEnemyCount is int that returns enemyprefabs.Count from WaveConfig
    //     for(int i = 0; i < currentWave.GetEnemyCount(); i++)
    //     {
    //         Instantiate(currentWave.GetEnemyPrefab(i),
    //                 // get transform of first waypoint in the path
    //                 currentWave.GetStartingWaypoint().position,
    //                 Quaternion.identity,
    //                 // this makes every enemy instanitated a child of the enemyspawner.
    //                 transform);
    //         yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());        
    //     }

    // }    



}