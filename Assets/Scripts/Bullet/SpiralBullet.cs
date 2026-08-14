using UnityEngine;

public class SpiralBullet : MonoBehaviour
{
    
    public float bulletSpeed = 1f;
    private Vector2 bulletDir;
    
    public int dmg = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(bulletDir * bulletSpeed * Time.deltaTime, Space.Self);
        
        if(transform.position.x < -5 || transform.position.x > 19)
        {
            gameObject.SetActive(false);
        }    
        if(transform.position.y > 6 || transform.position.y < -6)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetBulletDirection(Vector2 dir)
    {
        bulletDir = dir;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            //Player player = col.gameObject.GetComponent<Player>();
            Debug.Log("Taking Damage");
            PlayerMovement.instance.TakeDamage(dmg);
            gameObject.SetActive(false);    
        }
    }

}


