using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyShip : MonoBehaviour
{
    
    private float shootTimer;
    private float shootInterval;

    private int dmg = 1;
    
    private ObjectPooler projectilePool;
    private ObjectPooler destroyEffectPool;
    private Animator anim;
    
    public Transform bulletSpawn;

    private FlashWhite flashWhite;

    [SerializeField] private int lives;
    [SerializeField] private int maxLives;
    private float timeBeforeShooting = 1.5f;  
    private bool canShoot;

    
    // void OnEnable()
    // {
    //     // moving this to OnEnable solved the shooting bug.
    //     // putting this under Update made enemy shoot nonstop
    //     shootInterval = Random.Range(1f,2f); 
    // }
    
    void Awake()
    {
        // moving this to OnEnable or Awake solved the shooting bug.
        // putting this under Update made enemy shoot nonstop
        shootInterval = Random.Range(1.5f,2f);         
    }


    void Start()
    {
        projectilePool = GameObject.Find("EnemyBulletTwoPool").GetComponent<ObjectPooler>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
        flashWhite = GetComponent<FlashWhite>();
        lives = maxLives;
    }     


    void Update()
    {
        timeBeforeShooting -= Time.deltaTime;
        if(timeBeforeShooting <= 0)
        {
            canShoot = true;    
        }
        else
        {
            canShoot = false;
        }
        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0 && canShoot == true)
        {
            shootTimer += shootInterval;
            Shoot();
        }
    }

    private void Shoot()
    {
        // // we'll use this for multiple gun transform 
        // // for(int i = 0; i < bulletSpawn.Length; i++)
        // // {
        // //     GameObject projectile = projectilePool.GetPooledObject();
        // //     projectile.transform.position = bulletSpawn[i].position;
        // //     projectile.transform.rotation = bulletSpawn[i].rotation;
        // //     projectile.SetActive(true);
        // //     StartCoroutine(ResetShoot());
        // // }
        GameObject projectile = projectilePool.GetPooledObject();
        projectile.transform.position = bulletSpawn.position;
        projectile.transform.rotation = bulletSpawn.rotation;
        projectile.SetActive(true);
        StartCoroutine(ResetShoot());
    }

    IEnumerator ResetShoot()
    {
        // this means to wait one frame before executing again
        yield return null;
    }

    
    public void TakeDamage(int damage)
    {
        lives -= damage;
        if(lives > 0)
        {
            flashWhite.Flash();
        }
        else
        {
            flashWhite.Reset();
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            GameManager.instance.enemyCounter++;
            destroyEffect.SetActive(true);
            gameObject.SetActive(false);
        }   
    }    
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            //Player player = col.gameObject.GetComponent<Player>();
            Debug.Log("Taking Damage");
            PlayerMovement.instance.TakeDamage(dmg);
            gameObject.SetActive(false);    
        }
    }
}
