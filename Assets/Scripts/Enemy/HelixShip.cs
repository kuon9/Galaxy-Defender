using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class HelixShip : Enemy
{
private float initialPositionX;
private float moveSpeed;
private float shootTimer;
private float shootInterval;
private ObjectPooler firstHelixProjectilePool;
private ObjectPooler secondHelixProjectilePool;
public GameObject bulletPrefab;
private Animator anim;
private float timeBeforeShooting = 2f;
private bool canShoot;
public Transform bulletSpawn;
public float frequency = 5f;
public float amplitude = 2f;

public float speed = 2f;    
public override void OnEnable()
    {
        base.OnEnable();
        //transform.rotation = Quaternion.Euler(0,0,90);
        // this makes enemys charge in and spawn on right side of the map
        initialPositionX = 17f + Random.Range(-1f,1f);
        moveSpeed = Random.Range(2f, 3f);
        // if random value is less than 0.5 , 50% it eithers moves up or down Two float
        speedY = Random.value < 0.5 ? -2f : 2f;
        shootInterval = Random.Range(1f, 2f); 
        HpScaling();       
    }    
    
    
    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();    
        firstHelixProjectilePool = GameObject.Find("HelixBulletPool").GetComponent<ObjectPooler>();
        secondHelixProjectilePool = GameObject.Find("HelixBulletTwoPool").GetComponent<ObjectPooler>();        
        //hitSound = AudioManager.instance.hitImpact;
        //destroySound = AudioManager.instance.monsterDeath;        
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
            Shoot();
        }    
    }
    private void Shoot()
    {
        // negative transform.right means shooting projectile <
        Vector2 shootDir = -bulletSpawn.right;
        // GameObject projectileOne = firstHelixProjectilePool.GetPooledObject();
        // projectileOne.transform.position = bulletSpawn.position;
        // projectileOne.transform.rotation = bulletSpawn.rotation;
        // projectileOne.SetActive(true);
        GameObject bullet1 = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        
        HelixBullet helix1 = bullet1.GetComponent<HelixBullet>();
        if (helix1 != null)
        {
            helix1.offset = 1f;
            helix1.speed = speed;
            helix1.frequency = frequency;
            helix1.amplitude = amplitude;
            helix1.Initialize(shootDir);
        }

        // Spawn Bullet 2 (180 degree / PI phase offset to complete the helix)
        // GameObject projectileTwo = secondHelixProjectilePool.GetPooledObject();
        // projectileTwo.transform.position = bulletSpawn.position;
        // projectileTwo.transform.rotation = bulletSpawn.rotation;
        // projectileTwo.SetActive(true);
        GameObject bullet2 = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        HelixBullet helix2 = bullet2.GetComponent<HelixBullet>();
        if (helix2 != null)
        {
            helix2.offset = Mathf.PI;
            // we're using gameobject's frequency
            helix2.speed = speed;
            helix2.frequency = frequency;
            helix2.amplitude = amplitude;
            helix2.Initialize(shootDir);
        }        
        //AudioManager.instance.PlaySound(AudioManager.instance.squidShoot);
        //StartCoroutine(ResetShoot());                        
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
