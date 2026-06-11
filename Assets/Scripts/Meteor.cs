using UnityEngine;
using System.Collections;


public class Meteor : MonoBehaviour
{
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    FlashWhite flashWhite;

    [SerializeField] private Sprite[] sprites;

    [SerializeField] int lives;
    [SerializeField] private int maxLives;
    [SerializeField] private int dmg = 1;

    private ObjectPooler destroyEffectPool;


    float pushX;
    float pushY;

    
    void OnEnable()
    {
        lives = maxLives;
        transform.rotation = Quaternion.identity;
        pushX = Random.Range(-1f,0);
        pushY = Random.Range(-1f,1f);
        if(rb)rb.linearVelocity = new Vector2(pushX,pushY);
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flashWhite = GetComponent<FlashWhite>(); 
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 4 different variation of sprites on spawn for asteroid.
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        float randomScale = Random.Range(1.8f, 2f);  
        transform.localScale = new Vector2(randomScale, randomScale);   
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = col.gameObject.GetComponent<PlayerMovement>();
            if(playerMovement)playerMovement.TakeDamage(dmg);
            // this to test if asteroid flashes when taking damage froma player
        }
    }
    
    
    public void TakeDamage(int damage)
    {
        Debug.Log("Meteor taking damage");
        flashWhite.Flash();
        lives -= damage;
        if(lives > 0)
        {
            flashWhite.Flash();
        }
        else
        {
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.position = transform.position;
            destroyEffect.transform.rotation = transform.rotation;
            // we customized the transform.localscale to make the explosion smaller. 
            // if we made destroyeffect's local scale same as current gameobject local scale
            // then it is way too big
            destroyEffect.transform.localScale = new Vector3(1f,1f,1f);
            flashWhite.Reset();
            destroyEffect.SetActive(true);
            gameObject.SetActive(false);
            // destory gameobject doesn't work with object pooling
            // we need to make gameobject setactive false and true to reuse
            // we can't reuse if we destroy the gameobject
            //Destroy(GameObject);
        }
    }
}
