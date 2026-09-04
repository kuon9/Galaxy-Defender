using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RedModePowerUp : MonoBehaviour
{
    PowerUpTracker powerUpTracker;

    [SerializeField] float powerUpTimer = 20f;
    // [SerializeField] GameObject regularMode;
    // [SerializeField] GameObject shootingMode;
    public SpriteRenderer regularModeSpriteRenderer;
    public SpriteRenderer shootingFormSpriteRenderer;
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

    // we made the shooting form a function instead of IEnumerator.
    // we also removed weapon.instance.level from update because it was being updated each frame.
    // this made that our original level that was storing previous left before powering up
    // the same as weapon.instance.level, preventing us from returning to level before powering up.
    // void Update()
    // {
        
    // }
    public void RedForm()
    {
        //regularMode.SetActive(false);
        regularModeSpriteRenderer.enabled = false;
        shootingFormSpriteRenderer.enabled = true;
        //shootingMode.SetActive(true);     
        // stores the variable before powering up
        originalLevel = Weapon.instance.weaponLevel;
        //weapon level then becomes boosted level;
        Weapon.instance.weaponLevel = boostedLevel;
        Debug.Log("SHOOTING POWER UP PEW PEW");
        StartCoroutine(Shoot());   
    }

    IEnumerator Shoot()
    {
        // we'll be in this shooting state for 20 seconds
        yield return new WaitForSeconds(powerUpTimer);
        Debug.Log("Returning to original level before Shooting PowerUp");
        // revert back to the level before touching the shooting powerups
        Weapon.instance.weaponLevel = originalLevel;
        // return to shootingmode.Regular
        FiringMode.instance.currentMode = FiringMode.shootingMode.Regular;
        regularModeSpriteRenderer.enabled = true;
        //shootingMode.SetActive(false);
        shootingFormSpriteRenderer.enabled = false;
        Weapon.instance.StopLaser();  
        powerUpTracker.isRedMode = false;   
    }
// make IEnumerator that checks for player's level every few frames.
// however this IEnumetarot doesn't run if we're shooting power up;
}
