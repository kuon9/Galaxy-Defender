using UnityEngine;

public class PowerUps : MonoBehaviour
{

    //public static bool isRotating, shootingPowerUp;
    [SerializeField] bool unlockRotating, unlockshootingPowerUp, unlockSpreadMode;
    
    ShootingPowerUp shootingPowerUp;
    SpreadShootingPowerUp spreadShootingPowerUp;

    public void Start()
    {
        //isRotating = false; 
        shootingPowerUp = Object.FindAnyObjectByType<ShootingPowerUp>();
        spreadShootingPowerUp = Object.FindAnyObjectByType<SpreadShootingPowerUp>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        // look for PowerUpTracker in hiearchy and references to it
        // we're assigning the PowerTracker script as player
        PowerUpTracker player = other.GetComponentInParent<PowerUpTracker>();
        if(other.tag == "Player")
        {
            if(unlockRotating)
            {
                player.isRotating = true;
                //setactive false so it can be used for object pooling
                //destroying this gameobject prevents it from spawning from object pooler's script                                  
            }
            if(unlockshootingPowerUp)
            {
                player.isShooting = true;
                shootingPowerUp.ShootingForm();
            }

            if(unlockSpreadMode)
            {
                player.SpreadMode = true;
                FiringMode.instance.currentMode = FiringMode.shootingMode.Spread;
                spreadShootingPowerUp.SpreadPowerUp();
            }
            // makes gameobject setactive false after people touches it
            gameObject.SetActive(false);  
        }   
    }
}
