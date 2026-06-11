using UnityEngine;

public class MainMenuBackground : MonoBehaviour
{
    [SerializeField] private float backgroundSpeed;
    float backgroundImageWidth;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        backgroundImageWidth = sprite.texture.width / sprite.pixelsPerUnit;    
    }

    // Update is called once per frame
    void Update()
    {
        
        float moveX = backgroundSpeed * Time.deltaTime;
        // transform.position of background is constantly moving right at 0.01f speed
        transform.position += new Vector3(moveX , 0);
        // transform.position resets back to 0 if more than 5
        // with Mathf.Abs, transform position resets to 0 if its less than -5 or more than 5
        //if(Mathf.Abs(transform.position.x) > 5)
        if (Mathf.Abs(transform.position.x) - backgroundImageWidth > 0)
        {
                                                //this is the same as 0 or whatever the original transform.position.y is at.
            transform.position = new Vector3(0f, transform.position.y);
        }
    }
}
