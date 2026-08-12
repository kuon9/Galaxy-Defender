using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyShipWave : MonoBehaviour
{
    
    protected float shootTimer;
    protected float shootInterval;

    protected int dmg = 1;
    
    protected ObjectPooler projectilePool;
    protected ObjectPooler destroyEffectPool;
    protected Animator anim;
    
    public Transform bulletSpawn;

    private FlashWhite flashWhite;

    [SerializeField] protected float lives;
    [SerializeField] protected float maxLives;
    [SerializeField] protected int expToGive;
    protected AudioSource destroySound;
    protected float timeBeforeShooting = 1.5f;  
    protected bool canShoot;
    //[SerializeField] float timerBeforeHpIncrease = 10f;
    [SerializeField] protected int livesMultipler = 1;

    
    public virtual void OnEnable()
    {
        HpScaling();
    }
    
    public virtual void Awake()
    {
        // moving this to OnEnable or Awake solved the shooting bug.
        // putting this under Update made enemy shoot nonstop
        shootInterval = Random.Range(1.5f,2f);         
    }


    public virtual void Start()
    {
        projectilePool = GameObject.Find("EnemyBulletTwoPool").GetComponent<ObjectPooler>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
        flashWhite = GetComponent<FlashWhite>();
        lives = maxLives;
        destroySound = AudioManager.instance.shipExplosion;
    }     


    public virtual void Update()
    {
        // timerBeforeHpIncrease -= Time.deltaTime;
        // if(timerBeforeHpIncrease <= 0)
        // {
        //     maxLives += 1;
        //     maxLives = lives;
        //     timerBeforeHpIncrease = 10f;        
        // }
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

    public virtual void Shoot()
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

    public virtual IEnumerator ResetShoot()
    {
        // this means to wait one frame before executing again
        yield return null;
    }

    
    public virtual void TakeDamage(float damage)
    {
        lives -= damage;
        if(lives > 0)
        {
            flashWhite.Flash();
        }
        else
        {
            AudioManager.instance.PlayModifiedSound(destroySound);
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
    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            //Player player = col.gameObject.GetComponent<Player>();
            Debug.Log("Taking Damage");
            PlayerMovement.instance.TakeDamage(dmg);
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            destroyEffect.SetActive(true);
            gameObject.SetActive(false);    
        }
    }

    public virtual void HpScaling()
    {
        maxLives = PlayerMovement.instance.currentLevel * livesMultipler;
        lives = maxLives;              
    }
}
