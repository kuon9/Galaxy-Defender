using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class HomingCircleBullet : MonoBehaviour
{
    
    private enum State { Circling, Chasing}
    private State currentState;
    public float circleRadius = 2f;
    public float circleSpeed = 5f;
    private Vector2 bulletSpawn;
    private float angle;

    public float bulletSpeed = 10f;
    public float timeToChase = 1f;
    public int dmg;

    private Transform player;
    private Rigidbody2D rb;    
    public float homingProjectileTimer;
    public GameObject childProjectilePrefab;

    //private ObjectPooler  projectilePool; // The bullet to spawn on split
    public int splitCount = 3; // Number of new projectiles to spawn
    public float spreadAngle = 45f; // Total angle spread in degrees

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletSpawn = transform.position;
        currentState = State.Circling;
        GameObject playerShip = GameObject.FindWithTag("Player");
        if(playerShip != null)
        {
            player = playerShip.transform;
        }        
    
        currentState = State.Circling;
        Invoke(nameof(StartChasing), timeToChase);
        StartCoroutine(Timer());        
    }
    
    void Start()
    {
        //projectilePool = GameObject.Find("BossBulletFrPool").GetComponent<ObjectPooler>();        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentState == State.Circling)
        {
            angle += circleSpeed * Time.fixedDeltaTime;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * circleRadius;
            rb.MovePosition(bulletSpawn + offset);
        }
        else if (currentState == State.Chasing)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, bulletSpeed * Time.deltaTime);

            // Rotate towards the player
            Vector2 direction = (Vector2)(player.position - transform.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }    
    }

     void OnCollisionEnter2D(Collision2D col)
    {
        // Handle impact logic here (e.g., damage player, spawn explosion VFX)
        if(col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Taking Damage");
            PlayerMovement.instance.TakeDamage(dmg);
            gameObject.SetActive(false);    
        }
    }
    void StartChasing()
    {
        currentState = State.Chasing;
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(homingProjectileTimer);
        SplitProjectile();  
        gameObject.SetActive(false);  
    }
    void SplitProjectile()
    {
        float angleStep = 45f; 
        float startAngle = -15f;
        for (int i = 0; i < splitCount; i++)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            {
                //Rotate the direction slightly for the spread
                float currentAngle = startAngle + (angleStep * i);
                Quaternion spreadRotation = Quaternion.AngleAxis(currentAngle, Vector3.forward); // Use Vector3.up for 3D
                Vector3 finalDirection = spreadRotation * directionToPlayer;

                // Spawn the new projectile
                GameObject newProjectile = Instantiate(childProjectilePrefab, transform.position, Quaternion.identity);    
            }            
        }
    }
}