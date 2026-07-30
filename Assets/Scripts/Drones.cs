using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Drones : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject bulletPrefab;
    private ObjectPooler projectilePool;
    public Transform projectileSpawn;
    public float fireRate = 1f;
    public float detectionRange = 5f;

    private Quaternion targetRotation;
    
    [Header("Targeting")]
    //public LayerMask enemyLayer;
    //private Transform currentTarget;
    private bool isShooting = false;


    // this fixes the issue of drones of not firing after being setactive true
    // when returning to normal form after shooting form ends
    // because disabling an gameobject stops all coroutine
    // so when drones are setactive false, we make also set the boolean isShooting false
    // when drone gets reactivated, the update method will run because isShooting is false
    void OnDisable()
    {
        isShooting = false;
    }

    void Start()
    {
        projectilePool = GameObject.Find("BulletPool").GetComponent<ObjectPooler>();
    }

    void Update()
    {
        //currentTarget != null && 
        //FindClosestEnemy();

        if (!isShooting)
        {
            isShooting = true;
            Shoot();
            //StartCoroutine(ShootRoutine());
        }
    }

    // void FindClosestEnemy()
    // {
    //     Collider2D [] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
    //     float closestDistance = Mathf.Infinity;
    //     Transform closestEnemy = null;

    //     foreach (Collider2D enemy in enemies)
    //     {
    //         // calculates direction from drone's current transform subtracted from enemy's position
    //         Vector2 direction = transform.position - enemy.transform.position;
    //         targetRotation = Quaternion.LookRotation(Vector3.forward,direction);
    //         transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 90 * Time.deltaTime);
    //     }
    // }
    
    public void Shoot()
    {
        GameObject projectile = projectilePool.GetPooledObject();
        projectile.transform.position = projectileSpawn.position;
        projectile.transform.rotation = projectileSpawn.rotation;
        projectile.SetActive(true);
        StartCoroutine(ShootCD());
    }
    
    IEnumerator ShootCD()    
    {
        Debug.Log("DRONE FIRING");
        //Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }


    // Visualize the detection range in the Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
   