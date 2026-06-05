using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : Weapons
{
    
    public static Weapon instance;
    
    
    [SerializeField] private ObjectPooler bulletPool;
    [SerializeField] private ObjectPooler missilePool;

    [SerializeField] Transform missleSpawn;
    private bool missileAvailable = true;
    [SerializeField] int missileCD;


    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Shoot()
    {
        for (int i = 0; i < stats[weaponLevel].amount; i++)
        {
            GameObject bullet = bulletPool.GetPooledObject();
            // this spacing makes the bullet spread apart
            float yPos = transform.position.y;
            
            if(stats[weaponLevel].amount > 1)
            {
                float spacing = stats[weaponLevel].range / (stats[weaponLevel].amount - 1);
                yPos = transform.position.y - (stats[weaponLevel].range/2 ) + i * spacing;
            }
            bullet.transform.position = new Vector2(transform.position.x, yPos);
            //bullet.transform.position = transform.position;
            bullet.transform.localScale = new Vector2(stats[weaponLevel].size, stats[weaponLevel].size);
            bullet.SetActive(true);
        }
    }

    public void ShootMissile()
    {   
        if(missileAvailable)
        {
            missileAvailable = false;
            // AudioManager.instance.PlayModifiedSound(AudioManager.instance.shoot);
            GameObject missle = missilePool.GetPooledObject();
            missle.transform.position = missleSpawn.position;
            missle.SetActive(true);
            StartCoroutine(MissileCD());             
        }       
    }
    IEnumerator MissileCD()
    {
        yield return new WaitForSeconds(missileCD);
        missileAvailable = true;    
    }

    public void LevelUp()
    {
        if(weaponLevel < stats.Count - 1)
        {
            weaponLevel++;
        }
    }

}
 
