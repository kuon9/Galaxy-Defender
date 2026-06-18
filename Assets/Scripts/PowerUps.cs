using UnityEngine;

public class PowerUps : MonoBehaviour
{

    public static bool isRotating;

    public void Start()
    {
        isRotating = false;    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            isRotating = true;
            //setactive false so it can be used for object pooling
            //destroying this gameobject prevents it from spawning from object pooler's script
            gameObject.SetActive(false);    
        }   
    }
}
