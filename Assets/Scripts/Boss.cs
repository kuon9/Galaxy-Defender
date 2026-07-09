using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Boss : Enemy
{
    private float initialPositionX;
    private float moveSpeed;
    private float shootTimer;
    private float shootInterval;

    private int lives = 0;
    private int maxLives = 1000;
    private ObjectPooler projectileEnemyPool;
    private ObjectPooler projectilePool;
    private float timeBeforeShooting = 2f;
    
    private bool canShoot;
    
    public Transform projectileSpawn;



    public override void OnEnable()
    {
        base.OnEnable();
        initialPositionX = 20f + Random.Range(-1f,1f);
        transform.rotation = Quaternion.Euler(0,0,-90);
        moveSpeed = Random.Range(1.5f, 2.5f);
        speedY = Random.value < 0.5 ? -1f : 1f;
        shootInterval = Random.Range(2f,3.5f);
    }

    public override void Start()
    {
        base.Start();
        //destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
        projectileEnemyPool = GameObject.Find("FloatingHeadEnemyPool").GetComponent<ObjectPooler>();
        projectilePool = GameObject.Find("BossBulletPool").GetComponent<ObjectPooler>();
        hitSound = AudioManager.instance.hitImpact;
        destroySound = AudioManager.instance.bossDeath;    
    }


    public override void Update()
    {
        base.Update();
        // enemy can't shoot after spawning in for 2 seconds.
        // this fixes enemy shooting as they spawn in
        timeBeforeShooting -= Time.deltaTime;
        if(timeBeforeShooting <= 0)
        {
            canShoot = true;    
        }
        else
        {
            canShoot = false;
        }
        //movement x
        float currentX = transform.position.x;
        if(Mathf.Abs(currentX - initialPositionX) > 0.1f)
        {
            float posX = Mathf.Lerp(currentX, initialPositionX, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(posX, transform.position.y);
        }

        //movement y 
        if(transform.position.y > 4 || transform.position.y < -4)
        {
            speedY *= -1;
        }
        //shooting
        shootTimer -= Time.deltaTime;
        if(shootTimer <=  0 && canShoot == true)
        {
            shootTimer += shootInterval;
            ChooseRandomAttack();
            shootTimer = 2f;
            //Shoot();
        }    
    }
    
    void ChooseRandomAttack()
    {
        int randomAttack = Random.Range(0,2);
        if(randomAttack == 0)
        {
            SpawnEnemy();
        }
        else
        {
            Shoot();
        }
    }
    
    private void SpawnEnemy()
    {
        GameObject projectileEnemy = projectileEnemyPool.GetPooledObject();
        projectileEnemy.transform.position = projectileSpawn.position;
        projectileEnemy.transform.rotation = projectileSpawn.rotation;
        projectileEnemy.SetActive(true);
        //anim.SetBool("shooting", true);
        //AudioManager.instance.PlaySound(AudioManager.instance.squidShoot);
        //StartCoroutine(ResetShoot());          
    }

    private void Shoot()
    {
        GameObject projectile = projectilePool.GetPooledObject();
        projectile.transform.position = projectileSpawn.position;
        projectile.transform.rotation = projectileSpawn.rotation;
        projectile.SetActive(true);
        //StartCoroutine(ResetShoot());    
    }

    // IEnumerator ResetShoot()
    // {
    //     // waits one frame
    //     yield return null;
    //     //anim.SetBool("shooting", false); 
    // }
    // this overrides the inheritance method from enemy script
    public override void TakeDamage(int damage)
    {
        //AudioManager.instance.PlayModifiedSound(hitSound);
        base.TakeDamage(damage);
        lives -= damage;
        if(lives <= 0 )
        {
            AudioManager.instance.PlayModifiedSound(destroySound);
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            destroyEffect.SetActive(true);
            // UiController.instance.ModifyScore(scoreToGive);
            // Player.instance.GetExperience(experienceToGive);
            gameObject.SetActive(false);
            GameManager.instance.ActivateLevelCompletedUI();
        }
    }

    // we need to override this method or else the boss will have same hp as a regular enemy
    // because this boss is inheriting from the enemy script
    public override void HpScaling()
    {
        lives = maxLives;
    }

}
