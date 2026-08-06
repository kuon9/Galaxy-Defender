using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpreadShootingPowerUp : MonoBehaviour
{
    
    PowerUpTracker powerUpTracker;

    [SerializeField] float shootingTimer = 20f;
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

    public void SpreadPowerUp()
    {
        originalLevel = Weapon.instance.weaponLevel;
        //weapon level then becomes boosted level;
        Weapon.instance.weaponLevel = boostedLevel;
        Debug.Log("SHOOTING POWER UP PEW PEW");
        StartCoroutine(SpreadShoot());           
    }

    IEnumerator SpreadShoot()
    {
        // we'll be in this shooting state for 20 seconds
        yield return new WaitForSeconds(shootingTimer);
        Debug.Log("Returning to original level before Shooting PowerUp");
        // this reverts our current enum mode which is Spread mode to regular.
        FiringMode.instance.currentMode = FiringMode.shootingMode.Regular;
        // revert back to the level before touching the shooting powerups
        Weapon.instance.weaponLevel = originalLevel;
        powerUpTracker.SpreadMode = false;   
    }    
}
