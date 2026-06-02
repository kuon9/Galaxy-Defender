using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour
{
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    [SerializeField] private Sprite[] sprites;

    [SerializeField] int lives;
    [SerializeField] private int maxLives;
    [SerializeField] private int dmg = 1;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        float randomScale = Random.Range(0.6f, 1f);  
        transform.localScale = new Vector2(randomScale, randomScale);     
    }
}
