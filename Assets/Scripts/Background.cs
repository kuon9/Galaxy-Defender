using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.01f;
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
        float moveX = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(moveSpeed, 0);

        if(Mathf.Abs(transform.position.x) > backgroundImageWidth)
        {
            transform.position = new Vector3(0, transform.position.y);
        }
    }
}
