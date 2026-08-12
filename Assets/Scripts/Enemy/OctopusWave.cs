using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class OctopusWave : MonoBehaviour
{
    
    private float shootTimer;
    private float shootInterval;
    private ObjectPooler projectilePool;
    private ObjectPooler destroyEffectPool;
    private AudioSource destroySound;
    private float timeBeforeShooting = 1.5f;
    private bool canShoot;
    public Transform bulletSpawn;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private float lives;
    [SerializeField] private float maxLives;
    [SerializeField] int expToGive;
    private FlashWhite flashWhite;
    private float dmg = 1;
    private SpriteRenderer spriteRenderer;
    [SerializeField] float timerBeforeHpIncrease = 10f;
    [SerializeField] int livesMultipler = 1;
    
    
    void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0,0,90);
        HpScaling();
    }

    void Awake()
    {
        shootInterval = Random.Range(1.5f,2f);         
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projectilePool = GameObject.Find("EnemyBulletPool").GetComponent<ObjectPooler>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
        flashWhite = GetComponent<FlashWhite>();
        lives = maxLives;
        destroySound = AudioManager.instance.monsterDeath;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];        
    }

    // Update is called once per frame
    void Update()
    {
        timerBeforeHpIncrease -= Time.deltaTime;
        if(timerBeforeHpIncrease <= 0)
        {
            maxLives += 1;
            maxLives = lives;
            timerBeforeHpIncrease = 10f;           
        }
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

    public void TakeDamage(float damage)
    {
        lives -= damage;
        if(lives > 0)
        {
            flashWhite.Flash();
        }
        else
        {
            //AudioManager.instance.PlayModifiedSound(destroySound);
            flashWhite.Reset();
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            GameManager.instance.enemyCounter++;
            destroyEffect.SetActive(true);
            gameObject.SetActive(false);
            PlayerMovement.instance.GetExperience(expToGive);
        }   
    }  

    void HpScaling()
    {
        maxLives = PlayerMovement.instance.currentLevel * livesMultipler;
        lives = maxLives;           
    }

    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     if(col.gameObject.CompareTag("Player"))
    //     {
    //         Player player = col.gameObject.GetComponent<Player>();
    //         Debug.Log("Taking Damage");
    //         PlayerMovement.instance.TakeDamage(dmg);
    //         gameObject.SetActive(false);    
    //     }
    // }
}
