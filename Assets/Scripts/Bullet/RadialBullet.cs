using UnityEngine;

public class RadialBullet : MonoBehaviour
{
    private Vector2 bulletDir;
    private float moveSpeed;

    [SerializeField] int dmg = 3;   

    // Update is called once per frame
    void Update()
    {
        transform.Translate(bulletDir * moveSpeed * Time.deltaTime, Space.World);
        if(transform.position.x < -5)
        {
            gameObject.SetActive(false);
        }    
        if(transform.position.y > 6 || transform.position.y < -6)
        {
            gameObject.SetActive(false);
        }    
    }
    public void SetBulletDirection(Vector2 dir, float speed)
    {
        bulletDir = dir;
        moveSpeed = speed;
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
