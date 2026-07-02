using UnityEngine;
using System.Collections;
// Need System.Collections.Generic to make use of lists
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;


    private Rigidbody2D rb;
    private Animator anim;
    private Vector2  playerDirection;
    [Header("Player's Attributes")]
    // SerializeField doesn't allow other scripts to access this variable due to protection level
    [SerializeField] float moveSpeed;
    [SerializeField] float energy;
    [SerializeField] float maxEnergy;
    [SerializeField] float energyRegen;
    public bool boosting;
    public float boost = 1f;
    public float boostPower = 5f;
    public float fireRate = 3f;
    private float nextFireTime = 0.0f;
    private ObjectPooler playerBoomPool;

    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material whiteMaterial;
    [SerializeField] int experience;
    public int currentLevel;
    [SerializeField] int maxLevel;
    [SerializeField] List<int> playerLevels;


    FlashWhite flashWhite;
    SpriteRenderer spriteRenderer;

    [SerializeField] ParticleSystem boostEffect;
    public bool canMove;
    private AudioSource laserShoot;

    private AudioSource deathSFX;

    private AudioSource boostSound;

    PowerUpTracker powerUpTracker;
    
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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flashWhite = GetComponent<FlashWhite>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerBoomPool = GameObject.Find("playerBoomPool").GetComponent<ObjectPooler>();
        experience = 0;
        UiController.instance.UpdateExperienceSlider(experience, playerLevels[currentLevel]);
        deathSFX = AudioManager.instance.playerDeathExplosion;
        laserShoot = AudioManager.instance.laserShoot;
        powerUpTracker = GetComponent<PowerUpTracker>();    
    }

    // Update is called once per frame
    void Update()
    {
        // if player can't move then don't execute the code below
        if(!canMove) {return;}
        //RotatingPowerUp();
        float directionX = Input.GetAxisRaw("Horizontal");
        float directionY = Input.GetAxisRaw("Vertical");
        anim.SetFloat("moveX", directionX);
        anim.SetFloat("moveY", directionY);
        playerDirection = new Vector2(directionX, directionY).normalized;

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2"))
        {
            Boosting();
        }
        else if(Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2"))
        {
            StopBoosting();
        }
        if(Input.GetButtonDown("Fire1") || Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Weapon.instance.Shoot();
            nextFireTime = Time.time + fireRate;
            AudioManager.instance.PlayModifiedSound(laserShoot);
        }
        if(Input.GetButtonDown("Fire3"))
        {
            Weapon.instance.ShootMissile();
        }

    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(playerDirection.x * moveSpeed, playerDirection.y * moveSpeed);
        if(boosting)
        {
            if(energy >= 0.2f) energy -= 0.2f;
            else
            {
                StopBoosting();
            }
        }
        else
        {
            if(energy < maxEnergy)
            {
                energy += energyRegen;
            }
        }
        UiController.instance.UpdateEnergySlider(energy,maxEnergy);
    }
    public void Boosting()
    {
        // can only boost once energy full, this prevents us from spamming it as energy is regeneing
        if(energy > 10)
        {
            boostEffect.Play();
            //anim.SetBool("Boosting", true);
            boost = boostPower;
            boosting = true;            
        }
    }


    public void StopBoosting()
    {
         boostEffect.Stop();
        //anim.SetBool("Boosting", false);
        boost = 1f;
        boosting = false;
    }

    // public void RotatingPowerUp()
    // {
    //     if(!powerUpTracker.isRotating) {return;}
    //     {
    //         if(rotatingTimer >= 0)
    //         {
    //             Rotate();
    //             rotatingTimer -= Time.deltaTime;
    //         }
    //         else
    //         {
    //             StopRotate();
    //         }
    //     }    
    // }

    // void Rotate()
    // {
    //     for(int i = 0; i < orbs.Length; i++)
    //     {
    //         orbs[i].SetActive(true);
    //         transform.RotateAround(player.position, -Vector3.forward, rotationSpeed * Time.deltaTime);    
    //     }        
    // }

    // void StopRotate()
    // {
    //    for(int i = 0; i < orbs.Length; i++)
    //     {
    //         orbs[i].SetActive(false);
    //         powerUpTracker.isRotating = false;
    //         rotatingTimer = 10f;
    //     }            
    // }


    public void TakeDamage(int damage)
    {
        flashWhite.Flash();
        health -= damage;
        spriteRenderer.material = whiteMaterial;
        UiController.instance.UpdateHealthSlider(health,maxHealth);
        if(health <=0)
        {
            AudioManager.instance.PlayModifiedSound(deathSFX);
            GameObject playerBoom = playerBoomPool.GetPooledObject();
            playerBoom.transform.position = transform.position;
            playerBoom.transform.rotation = transform.rotation;
            playerBoom.SetActive(true);
            Destroy(gameObject);
            boost = 0f;
            GameManager.instance.GameOver();
        }
    }
    public void GetExperience(int exp)
    {
        experience += exp;
        UiController.instance.UpdateExperienceSlider(experience, playerLevels[currentLevel]);
        // if experience hit required amount to level up then execute level up function
        if( experience > playerLevels[currentLevel])
        {
            LevelUp(); 
        }
    }
    public void LevelUp()
    {
        // this allows excessive experience to carry over for next level
        
        experience -= playerLevels[currentLevel];
        if(currentLevel < maxLevel - 1) currentLevel++;
        UiController.instance.UpdateExperienceSlider(experience, playerLevels[currentLevel]);
        LevelUpText.instance.PlayAnimation();
        //Weapon.instance.LevelUp();
        maxHealth++;
        health = maxHealth;
        UiController.instance.UpdateHealthSlider(health,maxHealth);
        // only level up weapon at these level breakpoints 
        if(currentLevel is 3 or 6 or 8 or 10)
        {
            Weapon.instance.LevelUp();             
        }
        // This code below functions the same way as above if(currentLevel is 10 or 15 or 20)
        // if(currentLevel == 10|| currentLevel == 15|| currentLevel == 20)
        // {
        //     Weapon.instance.LevelUp();             
        // }
    }
}
