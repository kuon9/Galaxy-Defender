using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Drones : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float detectionRange = 5f;

    private Quaternion targetRotation;
    
    [Header("Targeting")]
    //public LayerMask enemyLayer;
    //private Transform currentTarget;
    private bool isShooting = false;

    void Update()
    {
        //currentTarget != null && 
        //FindClosestEnemy();

        if (!isShooting)
        {
            isShooting = true;
            StartCoroutine(ShootRoutine());
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
    IEnumerator ShootRoutine()
    {
        Debug.Log("DRONE FIRING");
        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
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
   