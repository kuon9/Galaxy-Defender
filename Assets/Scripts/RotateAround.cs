using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RotateAround : MonoBehaviour
{
    [SerializeField] GameObject [] orbs;
    [SerializeField] float rotationSpeed;
    public Transform player;
    [SerializeField] float rotatingTimer = 10f;


    //PowerUps powerUps;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameObject.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {
        if(!PowerUps.isRotating) {return;}
        //this.gameObject.SetActive(true);
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
    public void Rotate()
    {
        for(int i = 0; i < orbs.Length; i++)
        {
            orbs[i].SetActive(true);
            transform.RotateAround(player.position, -Vector3.forward, rotationSpeed * Time.deltaTime);    
        }
    }
    public void StopRotate()
    {
        for(int i = 0; i < orbs.Length; i++)
        {
            orbs[i].SetActive(false);
            PowerUps.isRotating = false;
            rotatingTimer = 10f;

        }
    }
}
