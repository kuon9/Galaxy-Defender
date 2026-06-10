using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class OctopusEnemy : Enemy
{

private float initialPositionX;
private float moveSpeed;

private float shootTimer;
private float shootInterval;
private ObjectPooler projectilePool;
private Animator anim;
private float timeBeforeShooting = 2f;
private bool canShoot;

public Transform bulletSpawn;

[SerializeField] private Sprite[] sprites;


public override void OnEnable()
    {
        base.OnEnable();
        transform.rotation = Quaternion.Euler(0,0,90);
        // this makes enemys charge in and spawn on right side of the map
        initialPositionX = 15f + Random.Range(-1f,1f);
        moveSpeed = Random.Range(1.5f, 2.5f);
        // if random value is less than 0.5 , 50% it eithers moves up or down one float
        speedY = Random.value < 0.5 ? -1f : 1f;
        shootInterval = Random.Range(2f, 3.5f);        
    }

    public override void Start()
    {
        base.Start();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        //anim = GetComponent<Animator>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();    
        projectilePool = GameObject.Find("EnemyBulletPool").GetComponent<ObjectPooler>();        
        // hitSound = AudioManager.instance.squidHit2;
        // destroySound = AudioManager.instance.squidDestroy2;        
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
        GameObject projectile = projectilePool.GetPooledObject();
        projectile.transform.position = bulletSpawn.position;
        projectile.transform.rotation = bulletSpawn.rotation;
        projectile.SetActive(true);
        //anim.SetBool("shooting", true);
        //AudioManager.instance.PlaySound(AudioManager.instance.squidShoot);
        StartCoroutine(ResetShoot());            
    }    
    IEnumerator ResetShoot()
    {
        // waits one frame
        yield return null;
        //anim.SetBool("shooting", false); 
    }
}
