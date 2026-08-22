using UnityEngine;
using System.Collections;



public class Missile : MonoBehaviour
{
    
    Weapon weapon;
    
    [SerializeField] int missileSpeed;
    [SerializeField] int defaultSpeed;
    [SerializeField] int missileRotationSpeed;
    [SerializeField] int missileDamage;
    [SerializeField] float missileTimer = 5f;
    private ObjectPooler destroyEffectPool;


    // this is for testing 
    private Transform asteroidTarget;

    private Transform enemyTarget;

    private Rigidbody2D rb;

    void Start()
    {
        weapon = Weapon.instance;
        rb = GetComponent<Rigidbody2D>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
        
    }

    void OnEnable()
    {
        StartCoroutine(LookForTarget());
    }

    void Update()
    {
       transform.position += new Vector3(defaultSpeed * Time.deltaTime, 0f);
       missileTimer -= Time.deltaTime;
        if(transform.position.x > 19 || missileTimer <= 0)
        {
            Explode();
            // Destroy(gameObject);
            //gameObject.SetActive(false);
            // reset missiletimer if setactive false
            missileTimer = 5f;
        }    
    }

    void Explode()
    {
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = this.transform.position;
            destroyEffect.transform.rotation = this.transform.rotation;
            destroyEffect.SetActive(true);
            gameObject.SetActive(false);        
    }


    void FixedUpdate()
    {
        if(enemyTarget == null)
        {
            //transform.position += new Vector3(defaultSpeed * Time.deltaTime, 0f);
            StartCoroutine(LookForTarget());
        }
        if(enemyTarget != null)
        {
            Vector2 direction = (enemyTarget.position - transform.position).normalized;
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            // assigning rigidbody's angularvelocity to this value
            rb.angularVelocity = -rotateAmount * missileRotationSpeed;
            // assigning rigidbody's linearvelocity to this value
            rb.linearVelocity = transform.up * missileSpeed;
        }
    }

    IEnumerator LookForTarget()
    {
        yield return new WaitForSeconds(0.5f);
        enemyTarget = GameObject.FindWithTag("Enemy").transform;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Obstacles"))
        {
            Asteroid asteroid = col.gameObject.GetComponent<Asteroid>();
            Meteor meteor = col.gameObject.GetComponent<Meteor>();
            if(asteroid) asteroid.TakeDamage(missileDamage);
            if(meteor) meteor.TakeDamage(missileDamage);
            // GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            // destroyEffect.transform.position = transform.position;
            // destroyEffect.transform.rotation = transform.rotation;
            // destroyEffect.SetActive(true);            
            gameObject.SetActive(false); 
        }
        else if(col.gameObject.CompareTag("Enemy"))
        {
            // GetComponent of the actual gameobject name and not the tag or layer of it
            EnemyShipWave enemyShipWave = col.gameObject.GetComponent<EnemyShipWave>();
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            // GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            // destroyEffect.transform.position = transform.position;
            // destroyEffect.transform.rotation = transform.rotation;
            // destroyEffect.SetActive(true);      
            if(enemyShipWave) enemyShipWave.TakeDamage(missileDamage);
            if(enemy)enemy.TakeDamage(missileDamage);
            gameObject.SetActive(false);
            Debug.Log("Enemy ship is taking damage");            
        }
    }

}
