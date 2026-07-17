using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Boss : Enemy
{
    private float initialPositionX;
    private float moveSpeed;
    private float shootTimer;
    private float shootInterval;
    private ObjectPooler projectileEnemyPool;
    private ObjectPooler projectilePool;
    private ObjectPooler homingProjectilePool;
    private float timeBeforeShooting = 2f;
    
    private bool canShoot;
    
    public Transform projectileSpawn;

    public Transform [] phaseTwoProjectileSpawn;

    [SerializeField] GameObject laser;
    public bool isLaser;
    [SerializeField] float laserDuration;

    public override void OnEnable()
    {
        base.OnEnable();
        initialPositionX = 18f + Random.Range(-1f,1f);
        transform.rotation = Quaternion.Euler(0,0,-90);
        moveSpeed = Random.Range(1.5f, 2.5f);
        speedY = Random.value < 0.5 ? -2f : 2f;
        shootInterval = Random.Range(1f,2f);
        isLaser = false;
    }

    public override void Start()
    {
        base.Start();
        //destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
        projectileEnemyPool = GameObject.Find("FloatingHeadEnemyPool").GetComponent<ObjectPooler>();
        projectilePool = GameObject.Find("BossBulletPool").GetComponent<ObjectPooler>();
        homingProjectilePool = GameObject.Find("BossHomingBulletPool").GetComponent<ObjectPooler>();
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
        if(transform.position.y > 3 || transform.position.y < -3)
        {
            speedY *= -1;
        }
        //shooting
        shootTimer -= Time.deltaTime;
        if(maxLives >= 300 && shootTimer <=  0 && canShoot == true)
        {
            shootTimer += shootInterval;
            PhaseOneAttacks();
            shootTimer = 1f;
        }    
        // isLaser bool prevents boss from shooting projectile while also shooting laser
        // maybe we'll remove later and add other attacks
        if(maxLives <= 300 && shootTimer <= 0 && canShoot == true && !isLaser)
        {
            shootTimer += shootInterval;
            PhaseTwoAttacks();
            shootTimer = 2f;
        }
    }
    
    void PhaseOneAttacks()
    {
        int randomAttack = Random.Range(0,3);
        if(randomAttack == 0 || randomAttack == 1)
        {
            PhaseOneProjectile();
        }
        else
        {
            SpawnEnemy();
        }
    }

    void PhaseTwoAttacks()
    {
        // increase odds of regular attacks vs laser
        int randomAttack = Random.Range(0,3);
        if(randomAttack == 0 || randomAttack == 1)
        {
            PhaseTwoProjectile();            
        }
        else
        {
            StartCoroutine(Laser());
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

    private void PhaseOneProjectile()
    {
        
        BossBullet.bulletSpeed = 7;
        BossBullet.dmg = 3;
        GameObject projectile = projectilePool.GetPooledObject();
        projectile.transform.position = projectileSpawn.position;
        projectile.transform.rotation = projectileSpawn.rotation;
        projectile.SetActive(true);
        //StartCoroutine(ResetShoot());    
    }

    private void PhaseTwoProjectile()
    {
        for(int i = 0; i < phaseTwoProjectileSpawn.Length; i++)
        {

            GameObject homingProjectile = homingProjectilePool.GetPooledObject();
            homingProjectile.transform.position = phaseTwoProjectileSpawn[i].position;
            homingProjectile.transform.rotation = phaseTwoProjectileSpawn[i].rotation;
            homingProjectile.SetActive(true);
            //StartCoroutine(ResetShoot());               
        } 
    }
    IEnumerator Laser()
    {
        laser.SetActive(true);
        isLaser = true;
        yield return new WaitForSeconds(laserDuration);
        laser.SetActive(false);
        isLaser = false;    
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
            // this makes us switch to level 2
            GameManager.instance.isSwitchingLevel = true;
            gameObject.SetActive(false);
            //GameManager.instance.ActivateLevelCompletedUI();
        }
    }
}
