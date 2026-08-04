using UnityEngine;

public class BeetleWave : MonoBehaviour
{

    private ObjectPooler destroyEffectPool;
    private AudioSource destroySound;
    private AudioSource hitSound;
    [SerializeField] private int lives;
    [SerializeField] private int maxLives;
    [SerializeField] int expToGive;
    [SerializeField] int livesMultipler = 1;
    private FlashWhite flashWhite;
    [SerializeField] private Sprite[] sprites;

    SpriteRenderer spriteRenderer;

    private int dmg = 1;


        void OnEnable()
    {
        // adjust this Quaternion euler so that it faces the player gameobject when spawning.
        transform.rotation = Quaternion.Euler(0,0,90);
        HpScaling();
    }


   //For internal initialization (references on the same object).
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashWhite = GetComponent<FlashWhite>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        lives = maxLives;        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //For external dependencies (references to other game objects or cross-script interactions).
    void Start()
    {
        destroyEffectPool = GameObject.Find("BeetlePopPool").GetComponent<ObjectPooler>();
        hitSound = AudioManager.instance.beetleHit;
        destroySound = AudioManager.instance.beetleDestroy; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        lives -= damage;
        if(lives > 0)
        {
            flashWhite.Flash();
        }
        else
        {
            //AudioManager.instance.PlayModifiedSound(destroySound);
            flashWhite.Reset();
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            GameManager.instance.enemyCounter++;
            destroyEffect.SetActive(true);
            gameObject.SetActive(false);
            PlayerMovement.instance.GetExperience(expToGive);
        }   
    }   
    
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlayModifiedSound(destroySound);
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            destroyEffect.SetActive(true);
            gameObject.SetActive(false);
            PlayerMovement playerMovement = col.gameObject.GetComponent<PlayerMovement>();
            Debug.Log("Taking Damage");
            PlayerMovement.instance.TakeDamage(dmg);
            gameObject.SetActive(false);    
        }
    }

    void HpScaling()
    {
        maxLives = PlayerMovement.instance.currentLevel * livesMultipler;
        lives = maxLives;    
    }
}
