using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MissileShip : Enemy
{
    private ObjectPooler projectilePool;
    private float shootTimer;
    private float shootInterval;
    private float timeBeforeShooting = 5f;
    private bool canShoot;
    [SerializeField] Transform [] missleSpawn;

    private float shipLivesMultipler = 3f;        
    
    
    public override void OnEnable()
    {
        base.OnEnable();
        shootInterval = 3f; 
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();    
        projectilePool = GameObject.Find("EnemyMissilePool").GetComponent<ObjectPooler>();        
        hitSound = AudioManager.instance.hitImpact;
        destroySound = AudioManager.instance.monsterDeath;                
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
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
        if(shootTimer <=  0 && canShoot == true)
        {
            shootTimer += shootInterval;
            LaunchMissile();
        }       
    }
    void LaunchMissile()
    {
        for(int m = 0; m < missleSpawn.Length; m++)
        {
            GameObject missile = projectilePool.GetPooledObject();
            missile.transform.position = missleSpawn[m].position;
            missile.transform.rotation = missleSpawn[m].rotation;
            missile.SetActive(true);
        }        
    }
    void HpScaling()
    {
        // makes enemy hp scale based on current player's hp
        maxLives = PlayerMovement.instance.currentLevel * shipLivesMultipler;
        lives = maxLives;        
    }
}
