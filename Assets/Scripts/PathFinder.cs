using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    
    //[SerializeField] WaveConfig waveConfig;
    WaveConfiguration waveConfiguration;
    EnemySpawner enemySpawner;
    // will come from our waveconfig scriptable object
    List<Transform> waypoints;
    int waypointIndex = 0;
    
    
    void Awake()
    {
        // looks for the script enemySpawner on whatever gameobject that its attached to
        // this is obsolete
        //enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner = Object.FindFirstObjectByType<EnemySpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        waveConfiguration = enemySpawner.GetCurrentWave();
        waypoints = waveConfiguration.GetWayPoints();
        // current position will be whatever index of the waypoint transform. 
        transform.position = waypoints[waypointIndex].position;    
    }

    // Update is called once per frame
    void Update()
    {
        FollowPath();    
    }

    void FollowPath()
    {
        if(waypointIndex < waypoints.Count)
        {
            Vector3 targetPosition = waypoints[waypointIndex].position;
            // returns float from scriptable object script.
            float speed = waveConfiguration.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
            if(transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Vector2.MoveTowards changes the z-position to 0f while Vector3.MoveTowards does not override the z-value with 0f.
    // We check if transform.position == targetPosition. This expression gets evaluated to false if the targetPosition.position.z is not 0f.
    // ^ this is the reason why Vector2.MoveTowards wasn't working 
}
