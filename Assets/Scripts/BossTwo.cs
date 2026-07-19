using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class BossTwo : Enemy
{

private float initialPositionX;
private float moveSpeed;
private float shootTimer;
private float shootInterval;
private ObjectPooler projectilePool;
private ObjectPooler homingCircleProjectilePool;
private Animator anim;
private float timeBeforeShooting = 2f;
private bool canShoot;
// using arrays means we can't edit it 
public Transform [] phaseOneBulletSpawn;
public Transform [] phaseTwoBulletSpawn;

// [SerializeField] private List<Transform> phaseOneGuns;
// [SerializeField] private List<Transform> phaseTwoGuns;
    
public override void OnEnable()
    {
        base.OnEnable();
        transform.rotation = Quaternion.Euler(0,0,0);
        // this makes enemys charge in and spawn on right side of the map
        initialPositionX = 17f + Random.Range(-1f,1f);
        moveSpeed = Random.Range(1f, 2f);
        // if random value is less than 0.5 , 50% it eithers moves up or down Two float
        speedY = Random.value < 0.5 ? -2f : 2f;
        shootInterval = Random.Range(2f, 3.5f); 
        //HpScaling();       
    }    
    
    
    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();    
        projectilePool = GameObject.Find("BossBulletFrPool").GetComponent<ObjectPooler>();
        homingCircleProjectilePool= GameObject.Find("BossBulletCirclePool").GetComponent<ObjectPooler>();        
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
        if(maxLives >= 200 && shootTimer <=  0 && canShoot == true)
        {
            shootTimer += shootInterval;
            PhaseOne();
        }    
        if(maxLives <= 200 && shootTimer <= 0 &&  canShoot == true)
        {
            shootTimer += shootInterval;
            PhaseTwo();
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

    }
    private void PhaseOne()
    {
        for (int i = 0; i < phaseOneBulletSpawn.Length; i++)
        {
            BossTwoBullet.bulletSpeed = 8;
            GameObject projectile = projectilePool.GetPooledObject();
            GameObject homingProjectile = homingCircleProjectilePool.GetPooledObject();
            // projectile.transform.position = phaseOneBulletSpawn[i].position;
            // projectile.transform.rotation = phaseOneBulletSpawn[i].rotation;
            // projectile.SetActive(true);
            homingProjectile.transform.position = phaseOneBulletSpawn[i].position;
            homingProjectile.transform.rotation = phaseOneBulletSpawn[i].rotation;
            homingProjectile.SetActive(true);            
            //anim.SetBool("shooting", true);
            //AudioManager.instance.PlaySound(AudioManager.instance.squidShoot);
            StartCoroutine(ResetShoot());               
        }           
    }
    private void PhaseTwo()
    {
        for (int i = 0; i < phaseTwoBulletSpawn.Length; i++)
        {
            // position boss closer to player
            // means less time for player to react and dodge to boss's projectiles
            initialPositionX = 14f + Random.Range(-1f,1f);
            BossTwoBullet.bulletSpeed = 10;
            GameObject projectile = projectilePool.GetPooledObject();
            projectile.transform.position = phaseTwoBulletSpawn[i].position;
            projectile.transform.rotation = phaseTwoBulletSpawn[i].rotation;
            projectile.SetActive(true);
            //anim.SetBool("shooting", true);
            //AudioManager.instance.PlaySound(AudioManager.instance.squidShoot);
            StartCoroutine(ResetShoot());               
        }           
    }
    // private void PhaseOne()
    // {
                            // we use .Count when using Lists, only use.Length with Arrays    
    // for (int i = 0; i < phaseOneGuns.Count; i++)
    //     {
    //     GameObject projectile = projectilePool.GetPooledObject();
    //     projectile.transform.position = phaseOneGuns[i].position;
    //     projectile.transform.rotation = phaseOneGuns[i].rotation;
    //     projectile.SetActive(true);
    //     //anim.SetBool("shooting", true);
    //     //AudioManager.instance.PlaySound(AudioManager.instance.squidShoot);
    //     StartCoroutine(ResetShoot());               
    //     }           
    // }
    // private void PhaseTwo()
    // {
    //     for (int i = 0; i < phaseTwoGuns.Count; i++)
    //     {
    //     GameObject projectile = projectilePool.GetPooledObject();
    //     projectile.transform.position = phaseTwoGuns[i].position;
    //     projectile.transform.rotation = phaseTwoGuns[i].rotation;
    //     projectile.SetActive(true);
    //     //anim.SetBool("shooting", true);
    //     //AudioManager.instance.PlaySound(AudioManager.instance.squidShoot);
    //     StartCoroutine(ResetShoot());               
    //     }           
    // }
    IEnumerator ResetShoot()
    {
        // waits one frame
        yield return null;
        //anim.SetBool("shooting", false); 
    }
    // void HpScaling()
    // {
    //     // makes enemy hp scale based on current player's hp
    //     maxLives = PlayerMovement.instance.currentLevel * livesMultipler;
    //     lives = maxLives;        
    // }
}
