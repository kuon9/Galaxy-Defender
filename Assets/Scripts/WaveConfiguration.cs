using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Wave Configuration", fileName = "New Wave Configuration")]
public class WaveConfiguration : ScriptableObject
{
    // making a list for enemy prefabs
    [SerializeField] List<GameObject> enemyPrefabs;
    // prefab of the path for enemies to move in
    [SerializeField] Transform pathPrefab;

    // speed of enemies on the path
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float TimeBetweenEnemySpawns = 1f;
    [SerializeField] float spawnTimeVariance = 0f;
    [SerializeField] float minimumSpawnTime = 0.2f;



    public Transform GetStartingWayPoint()
    {
        return pathPrefab.GetChild(0);
    }


    public List <Transform> GetWayPoints()
    {
        List<Transform> waypoints = new List<Transform>();
        foreach(Transform child in pathPrefab)
        {
            waypoints.Add(child);
        }
        return waypoints;
    }

    public int GetEnemyCount()
    {
        return enemyPrefabs.Count;
    }

    public GameObject GetEnemyPrefab(int index)
    {
        return enemyPrefabs[index];
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;    
    }

    public float GetRandomSpawnTime()
    {
        float spawnTime = Random.Range(TimeBetweenEnemySpawns - spawnTimeVariance,
                                        TimeBetweenEnemySpawns + spawnTimeVariance);
        return Mathf.Clamp(spawnTime, minimumSpawnTime, float.MaxValue);
    }
}
