using UnityEngine;

public class ShipRadialBullet : MonoBehaviour
{
    private float shootInterval = 1.2f;
    private float shootTimer;
    private ObjectPooler projectilePool;
    
    [SerializeField] int bulletCount;
    [SerializeField] float radius;
    [SerializeField] int bulletSpeed;

    private float currentAngleOffset = 0f;
    [SerializeField] float rotationSpeed = 10f;    

    [SerializeField] Transform bulletSpawn;

    void OnEnable()
    {
        //RadialPattern();    
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projectilePool = GameObject.Find("SecondRadialBulletPool").GetComponent<ObjectPooler>();
        //shootInterval = Random.Range(2f,3f);        
    }

    // Update is called once per frame
    void Update()
    {
        // this executes the same way as the one above
        shootTimer += Time.deltaTime;
        if(shootTimer >= shootInterval)
        {
            RadialPattern();
            shootTimer = 0;
        }
    }

    // void RadialPattern()
    // {
    //     Debug.Log("FIRING RADIAL");
    //     float angleStep = 360f / bulletCount;
    //     float angle = 0f;
    //     for(int i = 0; i <= bulletCount; i++);
    //     {
    //         float bulletDirectionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
    //         float bulletDirectionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;
    //         Vector3 bulVector = new Vector3(bulletDirectionX , bulletDirectionY, 0f);
    //         Vector2 bulDir = (bulVector - transform.position).normalized;
    //         GameObject projectile = projectilePool.GetPooledObject();
    //         projectile.transform.position = transform.position;
    //         projectile.transform.rotation = Quaternion.Euler(0,0,angle);
    //         projectile.SetActive(true);
    //         projectile.GetComponent<RadialBullet>().SetBulletDirection(bulDir, bulletSpeed);
    //         angle += angleStep;
    //     }
    // }
    public void RadialPattern()
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
}
