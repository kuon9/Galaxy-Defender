using UnityEngine;
using System.Collections;


public class EnemyMissile : MonoBehaviour
{
    [SerializeField] int missileSpeed;
    [SerializeField] int defaultSpeed;
    [SerializeField] int missileRotationSpeed;
    [SerializeField] int missileDamage;
    [SerializeField] float missileTimer = 5f;
    [SerializeField] float dmg;
    private ObjectPooler destroyEffectPool;    
    private Transform enemyTarget;

    private Rigidbody2D rb;    

    
    void OnEnable()
    {
        StartCoroutine(LookForTarget());        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();        
    }

    // Update is called once per frame
    void Update()
    {
       transform.position += new Vector3(-defaultSpeed * Time.deltaTime, 0f);
       missileTimer -= Time.deltaTime;
        if(transform.position.x < -10 || missileTimer <= 0)
        {
            Explode();
            // Destroy(gameObject);
            //gameObject.SetActive(false);
            // reset missiletimer if setactive false
            missileTimer = 5f;
        }          
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
    void Explode()
    {
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = this.transform.position;
            destroyEffect.transform.rotation = this.transform.rotation;
            destroyEffect.SetActive(true);
            gameObject.SetActive(false);        
    }    
    IEnumerator LookForTarget()
    {
        yield return new WaitForSeconds(0.5f);
        enemyTarget = GameObject.FindWithTag("Player").transform;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            //Player player = col.gameObject.GetComponent<Player>();
            Debug.Log("Taking Damage");
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = this.transform.position;
            destroyEffect.transform.rotation = this.transform.rotation;
            destroyEffect.SetActive(true);
            gameObject.SetActive(false);
            PlayerMovement.instance.TakeDamage(dmg);    
        }
    }    
}
