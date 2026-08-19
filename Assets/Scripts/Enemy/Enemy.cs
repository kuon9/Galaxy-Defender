using UnityEngine;

public class Enemy : MonoBehaviour
{
    

     // this allows only sub class and inherited class to access this
    protected SpriteRenderer spriteRenderer;
    protected ObjectPooler destroyEffectPool;
    protected AudioSource hitSound;
    protected AudioSource destroySound;
    protected float speedX = 0;
    protected float speedY = 0;
    [SerializeField] protected float lives;
    [SerializeField] protected float maxLives;
    [SerializeField] protected float damage;
    [SerializeField] protected float livesMultipler = 2;
    [SerializeField] protected int experienceToGive;
  
    //[SerializeField] int scoreToGive;
    //ScoreKeeper scoreKeeper;

    private FlashWhite flashWhite;
    
    public virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void OnEnable()
    {
        lives = maxLives;
    }

    public virtual void Start()
    {
        flashWhite = GetComponent<FlashWhite>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
    }

   //virtual will always run first because its the parent class, then child class afterwards
   public virtual void Update()
    {
        transform.position += new Vector3(speedX * Time.deltaTime, speedY * Time.deltaTime);
        if(transform.position.x < -4)
        {
            gameObject.SetActive(false);
        }        
    }
    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
            if(player)player.TakeDamage(damage);
        }
    }
    public virtual void TakeDamage(float damage)
    {
        // AudioManager.instance.PlayModifiedSound(hitSound);
        lives -= damage;
        if(lives > 0 )
        {
            flashWhite.Flash();
        }
        else
        {
            // AudioManager.instance.PlayModifiedSound(destroySound);
            flashWhite.Reset();
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            destroyEffect.SetActive(true);
            PlayerMovement.instance.GetExperience(experienceToGive);
            GameManager.instance.enemyCounter++;
            ScreenClear.instance.nukeEnergy ++;
            gameObject.SetActive(false);
        }
    }
}
