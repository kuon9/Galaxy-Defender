using UnityEngine;
using System.Collections;



public class Missile : MonoBehaviour
{
    
    Weapon weapon;
    
    [SerializeField] int missileSpeed;
    [SerializeField] int defaultSpeed;
    [SerializeField] int missileRotationSpeed;
    [SerializeField] int missileDamage;

    // this is for testing 
    private Transform asteroidTarget;

    private Transform enemyTarget;

    private Rigidbody2D rb;

    void Start()
    {
        weapon = Weapon.instance;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       transform.position += new Vector3(defaultSpeed * Time.deltaTime, 0f);
        if(transform.position.x > 19)
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }    
    }

    void FixedUpdate()
    {
        if(asteroidTarget == null)
        {
            //transform.position += new Vector3(defaultSpeed * Time.deltaTime, 0f);
            StartCoroutine(LookForTarget());
        }
        if(asteroidTarget != null)
        {
            Vector2 direction = (asteroidTarget.position - transform.position).normalized;
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
        asteroidTarget = GameObject.FindWithTag("Obstacles").transform;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Obstacles"))
        {
            Asteroid asteroid = col.gameObject.GetComponent<Asteroid>();
            if(asteroid) asteroid.TakeDamage(missileDamage);
            gameObject.SetActive(false); 
        }
    }

}
