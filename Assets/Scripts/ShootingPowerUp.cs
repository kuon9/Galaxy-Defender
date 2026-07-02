using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ShootingPowerUp : MonoBehaviour
{
    PowerUpTracker powerUpTracker;

    [SerializeField] float shootingTimer = 20f;

    [SerializeField] GameObject regularMode;
    [SerializeField] GameObject shootingMode;

    [SerializeField] int boostedLevel;

    [SerializeField] int StoreLevel;

    Weapons weapons;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //shootingMode.SetActive(false);
        // we're storing the current level of our weapon
        // StoreLevel only store the level when the game starts
        // we're moving this to update
        //StoreLevel = Weapon.instance.weaponLevel;
        powerUpTracker = GetComponentInParent<PowerUpTracker>();   
        weapons = GetComponentInParent<Weapons>();      
    }
    // Update is called once per frame
    void Update()
    {
        // game will always be updated with weapon's level now
        // this allows player to resume their weapon level after getting powerup since weapon level
        // is constantly being updated each frame
        StoreLevel = Weapon.instance.weaponLevel;
        Debug.Log(StoreLevel);
        if(!powerUpTracker.isShooting) {return;}
        {
            regularMode.SetActive(false);
            shootingMode.SetActive(true);
            StartCoroutine(Shoot());               
        }   
        shootingMode.transform.position = regularMode.transform.position;
    }
    IEnumerator Shoot()
    {
        // current weapon level becomes the "op level"
        Weapon.instance.weaponLevel = boostedLevel;
        Debug.Log("SHOOTING POWER UP PEW PEW");

        // we'll be in this shooting state for 20 seconds
        yield return new WaitForSeconds(shootingTimer);
        Debug.Log("Returning to original level before Shooting PowerUp");

        // revert back to the level before touching the shooting powerups
        Weapon.instance.weaponLevel = StoreLevel;
        regularMode.SetActive(true);
        shootingMode.SetActive(false);
        powerUpTracker.isShooting = false;   
    }
}
