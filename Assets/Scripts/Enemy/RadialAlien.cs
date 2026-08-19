using UnityEngine;

public class RadialAlien : Enemy
{
    private float initialPositionX;
    private float moveSpeed;
    private float shootInterval;
    private float shootTimer;
    private ObjectPooler projectilePool;
    [SerializeField] int bulletCount;
    [SerializeField] float radius;
    [SerializeField] int bulletSpeed;
    private float timeBeforeShooting = 2f;
    private bool canShoot;
    private float currentAngleOffset = 0f;
    [SerializeField] float rotationSpeed = 10f;
    private Animator anim;           
    
    void OnEnable()
    {
        base.OnEnable();
        initialPositionX = 11f + Random.Range(-1f,1f);
        moveSpeed = Random.Range(2f,3f);
        speedY = Random.value < 0.5 ? -2f : 2f;
        shootInterval = Random.Range(2f, 3.5f);
        HpScaling();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
        projectilePool = GameObject.Find("RadialBulletPool").GetComponent<ObjectPooler>();    
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
        timeBeforeShooting -= Time.deltaTime;
        if(timeBeforeShooting <= 0)
        {
            canShoot = true;
            shootTimer -= Time.deltaTime;    
        }
        else
        {
            canShoot = false;
        }            
        //moves to transform.position.x after spawning
        float currentX = transform.position.x;
        if(Mathf.Abs(currentX - initialPositionX) > 0.1f)
        {
            float posX = Mathf.Lerp(currentX, initialPositionX, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(posX, transform.position.y);
        }
        if(transform.position.y > 3 || transform.position.y < -3)
        {
            speedY *= -1;
        }
        if(shootTimer <= 0 && canShoot == true)
        {
            shootTimer += shootInterval;
            RadialAttack();
        }
    }
    void RadialAttack()
    {
        float angleStep = 360f/ bulletCount;
        for(int i = 0; i < bulletCount; i++)
        {
            float angle = (i * angleStep) + currentAngleOffset;
            float radian = angle * Mathf.Deg2Rad;
            float bulletDirX = Mathf.Cos(radian);
            float bulletDirY = Mathf.Sin(radian);
            Vector2 bulletDir = new Vector2(bulletDirX,bulletDirY);
            GameObject projectile = projectilePool.GetPooledObject();
            projectile.transform.position = transform.position;
            projectile.transform.rotation = Quaternion.Euler(0,0,angle);
            projectile.SetActive(true);
            projectile.GetComponent<RadialBullet>().SetBulletDirection(bulletDir, bulletSpeed);
        }        
    }
    void HpScaling()
    {
        // makes enemy hp scale based on current player's hp
        maxLives = PlayerMovement.instance.currentLevel * livesMultipler;
        lives = maxLives;        
    }
}
