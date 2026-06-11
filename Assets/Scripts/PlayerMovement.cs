using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;


    private Rigidbody2D rb;
    private Animator anim;
    private Vector2  playerDirection;
    [SerializeField] float moveSpeed;
    [SerializeField] float energy;
    [SerializeField] float maxEnergy;
    [SerializeField] float energyRegen;
    private bool boosting;
    public float boost = 1f;
    public float boostPower = 5f;
    private ObjectPooler playerBoomPool;

    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material whiteMaterial;

    FlashWhite flashWhite;
    SpriteRenderer spriteRenderer;

    [SerializeField] ParticleSystem boostEffect;
    
    
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
    }

    // Update is called once per frame
    void Update()
    {
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
        if(Input.GetButtonDown("Fire1"))
        {
            Weapon.instance.Shoot();
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
    void Boosting()
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

    void StopBoosting()
    {
         boostEffect.Stop();
        //anim.SetBool("Boosting", false);
        boost = 1f;
        boosting = false;
    }

    public void TakeDamage(int damage)
    {
        flashWhite.Flash();
        health -= damage;
        spriteRenderer.material = whiteMaterial;
        UiController.instance.UpdateHealthSlider(health,maxHealth);
        if(health <=0)
        {
            GameObject playerBoom = playerBoomPool.GetPooledObject();
            playerBoom.transform.position = transform.position;
            playerBoom.transform.rotation = transform.rotation;
            playerBoom.SetActive(true);
            Destroy(gameObject);
            boost = 0f;
        }
    }
}
