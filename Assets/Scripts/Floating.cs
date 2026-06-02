using UnityEngine;

public class Floating : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 

    // Update is called once per frame
    void Update()
    {
        float moveX = (GameManager.instance.mapSpeed * PlayerMovement.instance.boost) * Time.deltaTime;        
        transform.position += new Vector3(-moveX, 0);
        if(transform.position.x < -10)
        {
            gameObject.SetActive(false);
        }
    }
}
