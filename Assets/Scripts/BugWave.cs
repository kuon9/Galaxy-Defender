using UnityEngine;

public class BugWave : MonoBehaviour
{
    private ObjectPooler destroyEffectPool;
    private AudioSource destroySound;
    [SerializeField] private int lives;
    [SerializeField] private int maxLives;
    [SerializeField] int expToGive;
    [SerializeField] int livesMultipler = 1;
    private FlashWhite flashWhite;

    private int dmg = 1;


        void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0,0,-90);
        HpScaling();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
        flashWhite = GetComponent<FlashWhite>();
        lives = maxLives;
        destroySound = AudioManager.instance.monsterDeath;        
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
            PlayerHealth playerHealth = col.gameObject.GetComponent<PlayerHealth>();
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
