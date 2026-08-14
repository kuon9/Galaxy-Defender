using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AlienBoss : Enemy
{
    private float initialPositionX;
    private float moveSpeed;
    private float shootTimer;
    private float spreadTimer;
    private float spreadShootInterval;
    private float spiralTimer = 10f;
    private float spiralCD = 5f;
    private float shootInterval; 
    private ObjectPooler projectilePool;
    private ObjectPooler secondProjectilePool;
    private float timeBeforeShooting = 2f;
    private bool canShoot;
    private bool canSpiral;
    private bool canSpread;
    private Animator anim;
    public Transform spiralBulletSpawn;
    public Transform [] phaseTwoSpiralSpawn;
    public Transform [] phaseTwoBulletSpawn;
    private float angle = 0f;
    [SerializeField] float rotationSpeed = 10f; 

    
    public override void OnEnable()
    {
        base.OnEnable();
        transform.rotation = Quaternion.Euler(0,0,0);
        // this makes enemys charge in and spawn on right side of the map
        //initialPositionX = 10f + Random.Range(-1f,1f);
        initialPositionX = 12f;
        moveSpeed = Random.Range(1f, 2f);
        // if random value is less than 0.5 , 50% it eithers moves up or down Two float
        speedY = Random.value < 0.5 ? -2f : 2f;
        shootInterval = Random.Range(0.1f, 0.2f);
        spreadShootInterval = Random.Range(1.5f,2.5f);
        canSpiral = true; 
        //HpScaling();       
    }    
    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();    
        projectilePool = GameObject.Find("SpiralBulletOnePool").GetComponent<ObjectPooler>();
        secondProjectilePool = GameObject.Find("SpreadBulletPool").GetComponent<ObjectPooler>();
        // second variation of the alienboss's bullet
        //projectilePool = GameObject.Find("SpiralBulletTwoPool").GetComponent<ObjectPooler>();         
        hitSound = AudioManager.instance.hitImpact;
        destroySound = AudioManager.instance.monsterDeath;        
    }    
    public override void Update()
    {
        base.Update();
        if(lives >= 200)
        {
            canSpread = false;    
        }
        else
        {
            canSpread = true;
        }
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
        if(lives >= 200 && shootTimer <=  0 && canShoot == true && canSpiral == true)
        {
            shootTimer += shootInterval;
            PhaseOneSpiral();
            // initialPositionX = 12f + Random.Range(-1f,1f);
        }   
        if(lives <= 200 && shootTimer <= 0 &&  canShoot == true && canSpiral == true)
        {
            shootTimer += shootInterval;
            spiralTimer = 15f;
            PhaseTwoSpiral();
            // initialPositionX = 12f + Random.Range(-1f,1f);
        }
        spreadTimer -= Time.deltaTime;
        // spreadTimer dictates the fire rate
        if(canSpread && spreadTimer <= 0)
        {
            spreadTimer += spreadShootInterval;
            SpreadFire();    
        }
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
            projectile.transform.position = spiralBulletSpawn.position;
            projectile.transform.rotation = spiralBulletSpawn.rotation;
            projectile.SetActive(true);
            projectile.GetComponent<SpiralBullet>().SetBulletDirection(bulDir);
            angle += 20f;
            StartCoroutine(SpiralCD());                     
            //anim.SetBool("shooting", true);
            //AudioManager.instance.PlaySound(AudioManager.instance.squidShoot);              
        }           
    }
    private void PhaseTwoSpiral()
    {
        for (int i = 0; i < phaseTwoSpiralSpawn.Length; i++)
        {
            float bulletDirectionX = transform.position.x + Mathf.Sin(((angle + 180f * i) * Mathf.PI) / 180f);
            float bulletDirectionY = transform.position.y + Mathf.Cos(((angle + 180f * i) * Mathf.PI) / 180f);
            Vector3 bulVector = new Vector3(bulletDirectionX , bulletDirectionY, 0f);
            Vector2 bulDir = (bulVector - transform.position).normalized;
            GameObject projectile = projectilePool.GetPooledObject();
            projectile.transform.position = phaseTwoSpiralSpawn[i].position;
            projectile.transform.rotation = phaseTwoSpiralSpawn[i].rotation;
            projectile.SetActive(true);
            projectile.GetComponent<SpiralBullet>().SetBulletDirection(bulDir);
            StartCoroutine(SpiralCD());                                   
        }           
        angle += 15f;
        if(angle >= 360f)
        {
            angle = 0f;
        }
    }
    private void SpreadFire()
    {
        for (int v = 0; v < phaseTwoBulletSpawn.Length; v++)
        {
            GameObject spreadProjectile = secondProjectilePool.GetPooledObject();
            spreadProjectile.transform.position = phaseTwoBulletSpawn[v].position;
            spreadProjectile.transform.rotation = phaseTwoBulletSpawn[v].rotation;
            spreadProjectile.SetActive(true);
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
}
