using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class FanShip : Enemy
{
private float initialPositionX;
private float moveSpeed;
private float shootTimer;
private float shootInterval;
private ObjectPooler projectilePool;

public int bulletCount = 4;

[Tooltip("The total arc angle of the fan spread (in degrees).")]
public float totalSpreadAngle = 50f;
private Animator anim;
private float timeBeforeShooting = 2f;
private bool canShoot;
public Transform bulletSpawn;
    
public override void OnEnable()
    {
        base.OnEnable();
        //transform.rotation = Quaternion.Euler(0,0,90);
        // this makes enemys charge in and spawn on right side of the map
        initialPositionX = 15f + Random.Range(-1f,1f);
        moveSpeed = Random.Range(2f, 3f);
        // if random value is less than 0.5 , 50% it eithers moves up or down Two float
        speedY = Random.value < 0.5 ? -2f : 2f;
        shootInterval = Random.Range(2f, 3.5f); 
        HpScaling();       
    }    
    
    
    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();    
        projectilePool = GameObject.Find("FanBulletPool").GetComponent<ObjectPooler>();        
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
            shootTimer -= Time.deltaTime;    
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
        if(shootTimer <=  0 && canShoot == true)
        {
            shootTimer += shootInterval;
            Shoot();
        }    
    }
    private void Shoot()
    {
        // 1. Calculate the step angle between each individual bullet
        float angleStep = 0f;
        if (bulletCount > 1)
        {
            angleStep = totalSpreadAngle / (bulletCount - 1);
        }

        // 2. Find the leftmost starting angle relative to the center direction
        // transform.rotation.eulerAngles.z handles any direction your shooter is facing
        float centerAngle = transform.rotation.eulerAngles.z;
        float startAngle = centerAngle - (totalSpreadAngle / 2f);
        for (int i = 0; i < bulletCount; i++)
        {
            FanBullet.dmg = 3;
            FanBullet.bulletSpeed = 5;
            float currentBulletAngle = startAngle + (angleStep * i);
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, currentBulletAngle);        
            GameObject projectile = projectilePool.GetPooledObject();
            projectile.transform.position = bulletSpawn.position;
            projectile.transform.rotation = bulletRotation;
            projectile.SetActive(true);
            //anim.SetBool("shooting", true);
            //AudioManager.instance.PlaySound(AudioManager.instance.squidShoot);
            //StartCoroutine(ResetShoot());               
        }           
    }    
    // IEnumerator ResetShoot()
    // {
    //     // waits one frame
    //     yield return null;
    //     //anim.SetBool("shooting", false); 
    // }
    void HpScaling()
    {
        // makes enemy hp scale based on current player's hp
        maxLives = PlayerMovement.instance.currentLevel * livesMultipler;
        lives = maxLives;        
    }
}
