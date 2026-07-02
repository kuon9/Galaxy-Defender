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


    //PowerUps powerUps;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameObject.SetActive(false);
        //powerUpTracker = GetComponent<PowerUpTracker>();
        // GetComponentInParent is used if the component is not on this gameobject
        // instead it searches for the script as it goes up the hiearchy aka the parent of thi gameobject
        powerUpTracker = GetComponentInParent<PowerUpTracker>();    
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
        }        
    }
    void StopRotate()
    {
       for(int i = 0; i < orbs.Length; i++)
        {
            orbs[i].SetActive(false);
            powerUpTracker.isRotating = false;
            rotatingTimer = 10f;
        }            
    }

}
