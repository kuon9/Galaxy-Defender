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
    private Vector2 direction;
    private float angle;
    private ObjectPooler explosionVFX;

    public float bulletSpeed = 10f;
    public float timeToChase = 1f;
    public int dmg;
    private Rigidbody2D rb;    
    // public float homingProjectileTimer;

    //private ObjectPooler  projectilePool; // The bullet to spawn on split
    // public int splitCount = 3; // Number of new projectiles to spawn
    // public float spreadAngle = 45f; // Total angle spread in degrees
    
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletSpawn = transform.position;
        currentState = State.Circling;
        GameObject player = GameObject.FindWithTag("Player");
        if(player != null)
        {
            direction = (player.transform.position - transform.position).normalized;
        }        
        currentState = State.Circling;
        Invoke(nameof(StartChasing), timeToChase);    
    }
    void Start()
    {
        explosionVFX = GameObject.Find("RedHomingVFXPool").GetComponent<ObjectPooler>();
    }

    void Update()
    {
        // Make them setactive(false) and recycle them back into objectpool
        if(transform.position.x < -5)
        {
            gameObject.SetActive(false);
        }    
    }
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
            rb.linearVelocity = direction * bulletSpeed;

            // // Rotate towards the player
            // Vector2 direction = (Vector2)(player.position - transform.position);
            // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }    
    }
     void OnCollisionEnter2D(Collision2D col)
    {
        // Handle impact logic here (e.g., damage player, spawn explosion VFX)
        if(col.gameObject.CompareTag("Player"))
        {
            GameObject explosion = explosionVFX.GetPooledObject();
            explosion.transform.position = transform.position;
            explosion.transform.rotation = transform.rotation;
            explosion.SetActive(true);
            PlayerMovement.instance.TakeDamage(dmg);
            gameObject.SetActive(false);    
        }
    }
    void StartChasing()
    {
        currentState = State.Chasing;
    }
}