using UnityEngine;

public class PowerUps : MonoBehaviour
{

    //public static bool isRotating, shootingPowerUp;
    [SerializeField] bool unlockRotating, unlockshootingPowerUp;
    
    ShootingPowerUp shootingPowerUp;
    

    public void Start()
    {
        //isRotating = false; 
        shootingPowerUp = Object.FindAnyObjectByType<ShootingPowerUp>();  
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
            // makes gameobject setactive false after people touches it
            gameObject.SetActive(false);  
        }   
    }
}
