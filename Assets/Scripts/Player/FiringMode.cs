using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class FiringMode : MonoBehaviour
{
    public static FiringMode instance;
    public enum shootingMode
    {
        Regular,
        Spread,
        RedMode,
    }
    public shootingMode currentMode;
    public float fireRate = 3f;
    private float nextFireTime = 0.0f;
    private AudioSource laserShoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentMode = shootingMode.Regular;
        laserShoot = AudioManager.instance.laserShoot;        
    }
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
    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        switch(currentMode)
        {
            case shootingMode.Regular:
            if(Input.GetButtonDown("Fire1") || Input.GetButton("Fire1") && Time.time >= nextFireTime)
                {
                    Weapon.instance.Shoot();
                    nextFireTime = Time.time + fireRate;
                    AudioManager.instance.PlayModifiedSound(laserShoot);                    
                }
            break;           
            
            case shootingMode.Spread:
            if(Input.GetButtonDown("Fire1") || Input.GetButton("Fire1") && Time.time >= nextFireTime)
                {
                    Weapon.instance.SpreadPattern();
                    nextFireTime = Time.time + fireRate;
                    AudioManager.instance.PlayModifiedSound(laserShoot);                    
                }            
            break;
        
            case shootingMode.RedMode:
            if(Input.GetButtonDown("Fire1") || Input.GetButton("Fire1") && Time.time >= nextFireTime)
                {
                    Weapon.instance.Laser();
                    AudioManager.instance.PlayModifiedSound(laserShoot);                    
                }
            else if(Input.GetKeyUp(KeyCode.Mouse0) || Input.GetButtonUp("Fire1"))
                {
                    Weapon.instance.StopLaser();    
                }        
            break;
        }
    }
}
