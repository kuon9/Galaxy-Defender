using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] GameObject [] orbs;
    [SerializeField] float rotationSpeed;
    public Transform player;
    

    public bool isActive;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isActive = false;        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            Rotate();            
        }    
    }

    public void Rotate()
    {
        for(int i = 0; i < orbs.Length; i++)
        {
            transform.RotateAround(player.position, -Vector3.forward, rotationSpeed * Time.deltaTime);    
        }
    }
}
