using UnityEngine;
using System.Collections;

public class BeetleBoss : Enemy
{
    private Animator anim;
    //private ObjectPooler destroyEffectPool;
    // private float speedX;
    // private float speedY;
    private bool charging;
    private float switchInterval;
    private float switchTimer;
    // private int lives;
    // private int maxlives = 100;
    // private int dmg = 5;
    // private int experienceToGive = 20;
    public bool CanSpawn;
    //[SerializeField] GameObject WinConditonPreFab;

    

    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        // this prevents boss's spawn sound from playing even when boss hasn't spawned yet
        //gameObject.SetActive(false);   
        // Awake > OnEnable > Start <---- this is the initialization priority when unity plays 
    }
    
     /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    public override void OnEnable()
    {
        base.OnEnable();
        //lives = maxLives;
        ChargingState();
        AudioManager.instance.PlaySound(AudioManager.instance.bossSpawn);           
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        destroyEffectPool = GameObject.Find("Boom3Pool").GetComponent<ObjectPooler>();
        hitSound = AudioManager.instance.hitBoss;
        destroySound = AudioManager.instance.boom2;
        // ChargingState();
        // AudioManager.instance.PlaySound(AudioManager.instance.bossSpawn);    
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        // WinGame();
        float playerPosition = PlayerMovement.instance.transform.position.x;
        // as long as switchtimer is more than 0, minus 1 second
        if(switchTimer > 0)
        {
            switchTimer -= Time.deltaTime;
        }
        else
        {
            if(charging && transform.position.x > playerPosition)
            {
                PatrolState();
            }
            else
            {
                ChargingState();
            }    
        }
        
        // times -1 if greater than 3 or less than -3. This flips boss into opposite vertical direction.
        if(transform.position.y > 3 || transform.position.y < -3)
        {
            speedY *= -1;
        }
        else if(transform.position.x < playerPosition)
        {
            ChargingState();
        }

        bool boost = PlayerMovement.instance.boosting;
        float moveX;
        if(boost && !charging)
        {
            moveX = GameManager.instance.mapSpeed * Time.deltaTime * -0.5f;
        }
        else
        {
            moveX = speedX * Time.deltaTime;
        }
        //float moveX = speedX * Player.instance.boost * Time.deltaTime;
        float moveY = speedY * Time.deltaTime;
        transform.position += new Vector3(moveX, moveY);
        // if(transform.position.x < -11)
        // {
        //     Destroy(gameObject);
        // }        
        // deactivating boss instead of destroying it when it goes off the map
        if(transform.position.x < -11)
        {
            gameObject.SetActive(false);
        }
    }

    void PatrolState()
    {
        speedX = 0;
        speedY = Random.Range(-1f,2f); 
        switchInterval = Random.Range(5f, 10f);
        switchTimer = switchInterval;
        charging = false;
        anim.SetBool("Charging", false);   
    }

    void ChargingState()
    {
        if(!charging)
        AudioManager.instance.PlaySound(AudioManager.instance.bossCharge);  
        // boss moves left at a speed of 5 to melee attack player
        speedX = -5f;
        speedY = 0;
        switchInterval = Random.Range(0.5f, 2f);
        switchTimer = switchInterval;
        charging = true;
        anim.SetBool("Charging", true);
    
    }

    // void WinGame()
    // {
    //     if(lives <= 0)
    //     {
    //         Instantiate(WinConditonPreFab, transform.position, transform.rotation);
    //         GameManager.instance.CanSpawn = true;        
    //     }
    // }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        AudioManager.instance.PlayModifiedSound(AudioManager.instance.hitBoss);
        lives -= damage;
        if(lives <= 0)
        {
            
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            AudioManager.instance.PlayModifiedSound(AudioManager.instance.boom2);
            destroyEffect.SetActive(true);
            //Destroy(gameObject);
            gameObject.SetActive(false);
            PlayerMovement.instance.GetExperience(experienceToGive);
            //Instantiate(WinConditonPreFab, transform.position, transform.rotation);
            // this code below lets us respawn boss right after the first one dies.
            //GameManager.instance.CanSpawn = true;
        }     
    }

    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     // if(col.gameObject.CompareTag("Bullet"))
    //     // {
    //     //     TakeDamage(1);
    //     // }
    //     if(col.gameObject.CompareTag("Player"))
    //     {
    //         Player player = col.gameObject.GetComponent<Player>();
    //         if(player) player.TakeDamage(damage);   
    //     }
    // }
}
