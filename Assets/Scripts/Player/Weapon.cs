using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : Weapons
{
    
    public static Weapon instance; 
    
    
    [SerializeField] private ObjectPooler bulletPool;
    [SerializeField] private ObjectPooler redbulletPool;
    [SerializeField] private ObjectPooler missilePool;
    [SerializeField] private ObjectPooler offsetMisslePool;
    [SerializeField] private ObjectPooler spreadBulletPool;
    //[SerializeField] private ObjectPooler blossomBulletPool;
    private AudioSource missileShoot;

    [SerializeField] Transform missleSpawn;
    [SerializeField] Transform offsetMissleSpawn;

    [SerializeField] Transform [] spreadBulletSpawn;

    private bool missileAvailable = true;
    [SerializeField] int missileCD;

    PowerUpTracker powerUpTracker;
    
    [SerializeField] GameObject laser;

    // [Header ("Blossom Bullet")]
    // public GameObject blossomBulletPrefab;
    // [SerializeField] int armCount = 4;
    // [SerializeField] int pelletsPerArm = 3;
    // [SerializeField] float spreadAngle = 15f;
    // [SerializeField] float fireRate = 0.2f;
    // [SerializeField] float rotationSpeed = 90f;


 
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        missileShoot = AudioManager.instance.missileShoot;
        powerUpTracker = GetComponentInParent<PowerUpTracker>();
    }

    public void Shoot()
    {
        for (int i = 0; i < stats[weaponLevel].amount; i++)
        {
            GameObject bullet = bulletPool.GetPooledObject();
            GameObject redbullet = redbulletPool.GetPooledObject();
            // this spacing makes the bullet spread apart
            float yPos = transform.position.y;
            
            if(stats[weaponLevel].amount > 1)
            {
                float spacing = stats[weaponLevel].range / (stats[weaponLevel].amount - 1);
                yPos = transform.position.y - (stats[weaponLevel].range/2 ) + i * spacing;
            }
            if(!powerUpTracker.isRedMode)
            {
            bullet.transform.position = new Vector2(transform.position.x, yPos);
            //bullet.transform.position = transform.position;
            bullet.transform.localScale = new Vector2(stats[weaponLevel].size, stats[weaponLevel].size);
            bullet.SetActive(true);                
            }
            // old red mode powerup
            // if(powerUpTracker.isShooting)
            // {
            // redbullet.transform.position = new Vector2(transform.position.x, yPos);
            // //bullet.transform.position = transform.position;
            // redbullet.transform.localScale = new Vector2(stats[weaponLevel].size, stats[weaponLevel].size);
            // redbullet.SetActive(true);                
            // }
        }
    }
    public void Laser()
    {
        laser.SetActive(true);     
    }

    public void StopLaser()
    {
        laser.SetActive(false);
    }

    public void ShootMissile()
    {   
        if(missileAvailable)
        {
            missileAvailable = false;
            // AudioManager.instance.PlayModifiedSound(AudioManager.instance.shoot);
            GameObject missle = missilePool.GetPooledObject();
            missle.transform.position = missleSpawn.position;
            missle.transform.rotation = missleSpawn.rotation;
            missle.SetActive(true);
            StartCoroutine(MissileCD());             
        }       
    }
    public void ShootOffsetMissile()
    {
        if(missileAvailable)
        {
            missileAvailable = false;
            // AudioManager.instance.PlayModifiedSound(AudioManager.instance.shoot);
            GameObject offsetmissle = offsetMisslePool.GetPooledObject();
            offsetmissle.transform.position = offsetMissleSpawn.position;
            offsetmissle.transform.rotation = offsetMissleSpawn.rotation;
            offsetmissle.SetActive(true);
            StartCoroutine(MissileCD());             
        }           
    }
    IEnumerator MissileCD()
    {
        // putting audio source in here prevents me from spamming it whenever i press it
        // sound only plays when missile is off CD
        AudioManager.instance.PlayModifiedSound(missileShoot);
        yield return new WaitForSeconds(missileCD);
        missileAvailable = true;    
    }


    public void SpreadPattern()
    {
        for(int i = 0; i < spreadBulletSpawn.Length; i++)
        {
            GameObject spreadProjectile = spreadBulletPool.GetPooledObject();
            spreadProjectile.transform.position = spreadBulletSpawn[i].position;
            spreadProjectile.transform.rotation = spreadBulletSpawn[i].rotation;
            spreadProjectile.SetActive(true);       
        }
    }

    // public void BlossomPattern()
    // {
    //     for (int i = 0; i < armCount; i++)
    //     {
    //         // Base angle for each symmetric arm around the circle
    //         float armBaseAngle = currentAngle + (i * (360f / armCount));

    //         // Shotgun spread logic per arm
    //         for (int j = 0; j < pelletsPerArm; j++)
    //         {
    //             float pelletOffset = (j - (pelletsPerArm - 1) / 2f) * spreadAngle;
    //             float finalAngle = armBaseAngle + pelletOffset;

    //             Quaternion rotation = Quaternion.Euler(0, 0, finalAngle);
    //             GameObject bullet = Instantiate(blossomBulletPrefab, transform.position, rotation);
    //         }        
    //     }
    // }    
    public void LevelUp()
    {
        if(weaponLevel < stats.Count - 1)
        {
            weaponLevel++;
        }
    }
}
 
