using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RotateAround : MonoBehaviour
{
    [SerializeField] GameObject [] orbs;
    [SerializeField] float rotationSpeed;
    public Transform player;
    [SerializeField] float rotatingTimer = 10f;
    PowerUpTracker powerUpTracker;

    private Collider2D playerCollider;


    //PowerUps powerUps;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameObject.SetActive(false);
        //powerUpTracker = GetComponent<PowerUpTracker>();
        // GetComponentInParent is used if the component is not on this gameobject
        // instead it searches for the script as it goes up the hiearchy aka the parent of thi gameobject
        powerUpTracker = GetComponentInParent<PowerUpTracker>();
        // cache our component reference inside start or awake to avoid 
        // unncessary CPU Overhead.
        playerCollider = GetComponentInParent<Collider2D>();

    }

    void Update()
    {
        RotatingPowerUp();
    }
    
    public void RotatingPowerUp()
    {
        if(!powerUpTracker.isRotating) {return;}
        {
            if(rotatingTimer >= 0)
            {
                Rotate();
                rotatingTimer -= Time.deltaTime;
            }
            else
            {
                StopRotate();
            }
        }    
    }

    void Rotate()
    {
        for(int i = 0; i < orbs.Length; i++)
        {
            orbs[i].SetActive(true);
            transform.RotateAround(player.position, -Vector3.forward, rotationSpeed * Time.deltaTime);    
            playerCollider.enabled = false;
            // This disables the first Collider2D found on the parent object aka our PlayerMovement
            // this works too but puts more strain on our cpu
            //GetComponentInParent<Collider2D>().enabled = false;
            // This disables only the BoxCollider2D on the parent object
            //GetComponentInParent<BoxCollider2D>().enabled = false/true;
            // This disables all colliders attached to the parent
            // foreach(Collider2D col in parentColliders)
            // {
            //     col.enabled = false;
            // }
        }        
    }
    void StopRotate()
    {
       for(int i = 0; i < orbs.Length; i++)
        {
            playerCollider.enabled = true;
            
            //GetComponentInParent<Collider2D>().enabled = true;
            orbs[i].SetActive(false);
            powerUpTracker.isRotating = false;
            rotatingTimer = 10f;
        }            
    }

}
