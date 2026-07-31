using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AlienBoss : Enemy
{
    private float initialPositionX;
    private float moveSpeed;
    private float shootTimer;
    private float spiralTimer = 10f;
    private float spiralCD = 5f;
    private float shootInterval;    
    private ObjectPooler projectilePool;
    private float timeBeforeShooting = 2f;
    private bool canShoot;
    private bool canSpiral;
    private Animator anim;
    public Transform phaseOneBulletSpawn;
    private float angle = 0f;

    
    public override void OnEnable()
    {
        base.OnEnable();
        transform.rotation = Quaternion.Euler(0,0,0);
        // this makes enemys charge in and spawn on right side of the map
        initialPositionX = 15f + Random.Range(-1f,1f);
        moveSpeed = Random.Range(1f, 2f);
        // if random value is less than 0.5 , 50% it eithers moves up or down Two float
        speedY = Random.value < 0.5 ? -2f : 2f;
        shootInterval = Random.Range(0.1f, 0.2f);
        canSpiral = true; 
        //HpScaling();       
    }    
    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();    
        projectilePool = GameObject.Find("SpiralBulletOnePool").GetComponent<ObjectPooler>();
        // second variation of the alienboss's bullet
        //projectilePool = GameObject.Find("SpiralBulletTwoPool").GetComponent<ObjectPooler>();         
        hitSound = AudioManager.instance.hitImpact;
        destroySound = AudioManager.instance.monsterDeath;        
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
        //shooting
        shootTimer -= Time.deltaTime;
        if(maxLives >= 200 && shootTimer <=  0 && canShoot == true && canSpiral == true)
        {
            shootTimer += shootInterval;
            PhaseOneSpiral();
            initialPositionX = 12f + Random.Range(-1f,1f);
        }   
        // if(maxLives <= 200 && shootTimer <= 0 &&  canShoot == true)
        // {
        //     shootTimer += shootInterval;
        //     PhaseTwoSpiral();
        // }

        //movement x
        float currentX = transform.position.x;
        if(Mathf.Abs(currentX - initialPositionX) > 0.1f)
        {
            float posX = Mathf.Lerp(currentX, initialPositionX, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(posX, transform.position.y);
        }

        if(transform.position.y > 3 || transform.position.y < -3)
        {
            speedY *= -1;
        }
    }    
    private void PhaseOneSpiral()
    {
        //for (int i = 0; i < phaseTwoBulletSpawn.Length; i++)
        {
            // makes boss move closer to player when doing spiral bullet pattern
            float bulletDirectionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulletDirectionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);
            Vector3 bulVector = new Vector3(bulletDirectionX , bulletDirectionY, 0f);
            Vector2 bulDir = (bulVector - transform.position).normalized;
            GameObject projectile = projectilePool.GetPooledObject();
            projectile.transform.position = phaseOneBulletSpawn.position;
            projectile.transform.rotation = phaseOneBulletSpawn.rotation;
            projectile.SetActive(true);
            projectile.GetComponent<SpiralBullet>().SetBulletDirection(bulDir);
            angle += 20f;
            StartCoroutine(SpiralCD());                     
            //anim.SetBool("shooting", true);
            //AudioManager.instance.PlaySound(AudioManager.instance.squidShoot);              
        }           
    }

    IEnumerator SpiralCD()
    {
        yield return new WaitForSeconds(spiralTimer);
        canSpiral = false;
        shootTimer = 0;
        yield return new WaitForSeconds(spiralCD);
        canSpiral = true;
    }
    // private void PhaseTwoSpiral()
    // {
    //     for (int i = 0; i < phaseTwoBulletSpawn.Length; i++)
    //     {
    //         // position boss closer to player
    //         // means less time for player to react and dodge to boss's projectiles
    //         initialPositionX = 14f + Random.Range(-1f,1f);
    //         BossTwoBullet.bulletSpeed = 10;
    //         GameObject projectile = projectilePool.GetPooledObject();
    //         GameObject homingProjectile = homingCircleProjectilePool.GetPooledObject();
    //         projectile.transform.position = phaseTwoBulletSpawn[i].position;
    //         projectile.transform.rotation = phaseTwoBulletSpawn[i].rotation;
    //         projectile.SetActive(true);
    //         homingProjectile.transform.position = phaseOneHomingBulletSpawn[v].position;
    //         homingProjectile.transform.rotation = phaseOneHomingBulletSpawn[v].rotation;
    //         homingProjectile.SetActive(true);                        
    //         //anim.SetBool("shooting", true);
    //         //AudioManager.instance.PlaySound(AudioManager.instance.squidShoot);
    //         //StartCoroutine(ResetShoot());               
    //     }           
    // }    
}
