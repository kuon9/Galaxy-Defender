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
    [SerializeField] int originalLevel;
    private int storeLevel;

    Weapons weapons;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerUpTracker = GetComponentInParent<PowerUpTracker>();   
        weapons = GetComponentInParent<Weapons>();      
    }
    // Update is called once per frame
    void Update()
    {
        if(powerUpTracker.isShooting)
        {
            regularMode.SetActive(false);
            shootingMode.SetActive(true);
            StartCoroutine(Shoot());               
        }   
    }
    IEnumerator Shoot()
    {
        // stores the variable before powering up
        originalLevel = Weapon.instance.weaponLevel;

        //weapon level then becomes boosted level;
        Weapon.instance.weaponLevel = boostedLevel;
        Debug.Log("SHOOTING POWER UP PEW PEW");

        // we'll be in this shooting state for 20 seconds
        yield return new WaitForSeconds(shootingTimer);
        Debug.Log("Returning to original level before Shooting PowerUp");

        // revert back to the level before touching the shooting powerups
        Weapon.instance.weaponLevel = originalLevel;
        regularMode.SetActive(true);
        shootingMode.SetActive(false);
        powerUpTracker.isShooting = false;   
    }
// make IEnumerator that checks for player's level every few frames.
// however this IEnumetarot doesn't run if we're shooting power up;
}
