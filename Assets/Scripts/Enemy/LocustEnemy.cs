using UnityEngine;
using System.Collections.Generic;

public class LocustEnemy : Enemy
{
    [SerializeField] private List<Frames> frames;
    private int enemyVariant;
    private bool charging;
    

    public override void OnEnable()
    {
        base.OnEnable();
        enemyVariant = Random.Range(0, frames.Count);
        EnterIdle();
        HpScaling();
    }


    public override void Start()
    {
        // this runs the start method from the base/parent/Super-class
        base.Start();
        // anything below base.Start(); is unique to this child class
        // enemy can spawn in any of the 4 sprites randomly 
        //spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        destroyEffectPool = GameObject.Find("LocustPopPool").GetComponent<ObjectPooler>();
        hitSound = AudioManager.instance.locustHit;
        destroySound = AudioManager.instance.locustDestroy;
        // speedX = Random.Range(0.1f, 0.6f);
        // speedY = Random.Range(-0.9f, 0.9f);
    }

    public override void Update()
    {
        base.Update();
        if(transform.position.y > 4 || transform.position.y < - 4)
        {
            speedY *= -1;
        }
    }

    private void EnterIdle()
    {
        charging = false;
        spriteRenderer.sprite = frames[enemyVariant].sprites[0];
        speedX = Random.Range(0.1f, 0.6f);
        speedY = Random.Range(-0.9f, 0.9f);
    }

    private void EnterCharge()
    {
        if(!charging)
        {
            charging = true;
            spriteRenderer.sprite = frames[enemyVariant].sprites[1];
            AudioManager.instance.PlaySound(AudioManager.instance.locustCharge);
            speedX = Random.Range(-4f, -6f);
            speedY = 0;
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        // if health is 50% of max HP then enemy will charge
        if(lives <= maxLives * 0.5f)
        {
            EnterCharge();
        }
        // if(lives <= 0)
        // {
        //     GameManager.instance.locustCounter++;
        // }
    }

    [System.Serializable]
    private class Frames
    {
        public Sprite[] sprites;    
    }
    void HpScaling()
    {
        // makes enemy hp scale based on current player's hp
        maxLives = PlayerMovement.instance.currentLevel * livesMultipler;
        lives = maxLives;        
    }
}
