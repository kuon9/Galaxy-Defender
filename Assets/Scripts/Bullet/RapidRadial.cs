using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class RapidRadial : MonoBehaviour
{
    [SerializeField] float shootInterval;
    private float shootTimer;
    private ObjectPooler projectilePool;
    
    [SerializeField] int bulletCount;
    [SerializeField] float radius;
    [SerializeField] int bulletSpeed;
    private float rapidCD = 5f;
    private float rapidTimer = 8f;
    private float timeBeforeShooting = 2f;
    private bool canShoot;
    private bool canRapid;

    private float currentAngleOffset = 0f;
    [SerializeField] float rotationSpeed = 10f;    

    [SerializeField] Transform bulletSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projectilePool = GameObject.Find("RadialBulletPool").GetComponent<ObjectPooler>();               
        canRapid = true;    
    }

    // Update is called once per frame
    void Update()
    {
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
        currentAngleOffset += rotationSpeed * Time.deltaTime;
        //shootTimer -= Time.deltaTime;
        if(shootTimer <= 0 && canRapid && canShoot)
        {
            shootTimer += shootInterval;
            //RadialPattern();
            RadialPattern();
        }            
    }
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
            projectile.transform.position = bulletSpawn.position;
            projectile.transform.rotation = Quaternion.Euler(0,0,angle);
            projectile.SetActive(true);
            projectile.GetComponent<RadialBullet>().SetBulletDirection(bulletDir, bulletSpeed);
            StartCoroutine(RapidCD());
        }
    }
    IEnumerator RapidCD()
    {
        yield return new WaitForSeconds(rapidTimer);
        canRapid = false;
        //shootTimer = 0;
        yield return new WaitForSeconds(rapidCD);
        canRapid = true;
    }
}
