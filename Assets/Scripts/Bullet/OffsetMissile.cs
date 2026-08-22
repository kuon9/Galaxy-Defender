using UnityEngine;

public class OffsetMissile : MonoBehaviour
{
    Weapon weapon;
    public float speed = 5f;
    public float rotateSpeed = 200f;
    public float offsetDuration = 1f; // Time spent flying straight/offset
    public float curveDirection;
    //public Vector3 initialOffsetDir = Vector3.up;
    [SerializeField] int missileDamage;
    private ObjectPooler destroyEffectPool;
    

    private Transform target;
    private float timer;
    private Rigidbody2D rb; // Use Rigidbody for 3D if needed

    void OnEnable()
    {
        FindClosestEnemy();
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = offsetDuration;
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
    }

    void FixedUpdate()
    {
        if (timer > 0)
        {
            // Phase 1: Move along the initial offset direction
            timer -= Time.fixedDeltaTime;
            //rb.linearVelocity = initialOffsetDir * speed;
            rb.linearVelocity = transform.up * speed;
            rb.angularVelocity = curveDirection * rotateSpeed;
        }
        else
        {
            // // Phase 2: Find target if null
            // if (target == null)
            // {
            //     FindClosestEnemy();
            // }

            if (target != null)
            {
                // Steer towards enemy
                Vector2 direction = (Vector2)target.position - rb.position;
                direction.Normalize();
                float rotateAmount = Vector3.Cross(direction, transform.up).z;
                rb.angularVelocity = -rotateAmount * rotateSpeed;
                rb.linearVelocity = transform.up * speed;
            }
            else
            {
                // Keep going straight if no enemies exist
                rb.linearVelocity = transform.up * speed;
            }
        }
    }
    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Obstacles"))
        {
            Asteroid asteroid = col.gameObject.GetComponent<Asteroid>();
            Meteor meteor = col.gameObject.GetComponent<Meteor>();
            if(asteroid) asteroid.TakeDamage(missileDamage);
            if(meteor) meteor.TakeDamage(missileDamage);
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            destroyEffect.SetActive(true);            
            gameObject.SetActive(false); 
        }
        else if(col.gameObject.CompareTag("Enemy"))
        {
            // GetComponent of the actual gameobject name and not the tag or layer of it
            EnemyShipWave enemyShipWave = col.gameObject.GetComponent<EnemyShipWave>();
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            destroyEffect.SetActive(true);      
            if(enemyShipWave) enemyShipWave.TakeDamage(missileDamage);
            if(enemy)enemy.TakeDamage(missileDamage);
            gameObject.SetActive(false);
            Debug.Log("Enemy ship is taking damage");            
        }
    }
}
