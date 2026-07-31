using UnityEngine;

public class Ring : MonoBehaviour
{
    
    public int bulletCount = 12;
    public float fireRate = 2f;

    public float bulletSpeed = 5f;
    private float timer;
    
    // Update is called once per frame
    void Update()
    {
        // opposite of ShootTimer -= Time.deltaTime but its essentially the same logic
        timer += Time.deltaTime;
        // spawn rings every 2 secs
        if(timer >= fireRate)
        {
            //SpawnRing();
            // if it was shoot timer -=
            // then we put timer = 5f; instead of 0
            timer = 0f;
        }        
    }

    // void SpawnRing()
    // {
    //     // calculates the anglestep between each bullet
    //     float angleStep = 360f/ bulletCount;
    //     float angle = 0f;

    //     for (int i = 0; i (out Bullet bullerScript))
    //     {
    //         bulletComponent.SetDirection(bulletDirection, bulletSpeed);
    //     }

    //         // Optional: Rotate the bullet asset visually to face outwards
    //         bullet.transform.right = bulletDirection; 

    //         angle += angleStep;        
    // }
}
