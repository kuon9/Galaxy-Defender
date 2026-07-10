using UnityEngine;

public class SquidEnemy : Enemy
{
    [SerializeField] private Sprite[] sprites;
    private Quaternion targetRotation;
    private float targetSpeedX;
    private float shootTimer;
    private float shootInterval;
    private ObjectPooler projectilePool;


    public override void OnEnable()
    {
        base.OnEnable();
        // manually change gameobject's rotation when spawning
        transform.rotation = Quaternion.Euler(0,0,90);
        // reduce speed at which it comes into map
        speedX = -7f;
        targetSpeedX = Random.Range(-0.1f, -0.6f);
        speedY = Random.Range(-0.3f,0.3f);
        shootInterval = Random.Range(1f, 4f);
        shootTimer = 1f;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        destroyEffectPool = GameObject.Find("SquidPopPool").GetComponent<ObjectPooler>();
        projectilePool = GameObject.Find("SquidCritterPool").GetComponent<ObjectPooler>();
        hitSound = AudioManager.instance.squidHit2;
        destroySound = AudioManager.instance.squidDestroy2;                
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        // makes enemies go up and down based on current transform.position.y 
        // any y position higher than 5 * -1 and becomes -5 = goes down
        // any y position less than -5 *-1 and becomes 5 = goes up
        if(transform.position.y > 5 || transform.position.y < -5)
        {
            speedY *= -1;
        }
        
        if(speedX != targetSpeedX)
        {
            speedX = Mathf.Lerp(speedX, targetSpeedX, Time.deltaTime * 2.5f);
            if(Mathf.Abs(speedX - targetSpeedX) < 1f)
            {
                speedX = targetSpeedX;
            }
        }
        else
        {
            Vector3 relativePos = PlayerMovement.instance.transform.position - transform.position;
            if(relativePos != Vector3.zero)
            {
            targetRotation = Quaternion.LookRotation(Vector3.forward, relativePos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1080 * Time.deltaTime);
            } 
            shootTimer -= Time.deltaTime;
            if(shootTimer <=  0)
            {
                shootTimer += shootInterval;
                Shoot();
            }
        }
    }
    private void Shoot()
    {
        GameObject projectile = projectilePool.GetPooledObject();
        projectile.transform.position = transform.position;
        projectile.transform.rotation = transform.rotation;
        projectile.SetActive(true);
        AudioManager.instance.PlaySound(AudioManager.instance.squidShoot);
    }

    // public override void TakeDamage(int damage)
    // {
    //     base.TakeDamage(damage);
    //     // if health is 50% of max HP then enemy will charge
    //     if(lives <= 0)
    //     {
    //         //GameManager.instance.squidCounter++;
    //     }
    // }
}
