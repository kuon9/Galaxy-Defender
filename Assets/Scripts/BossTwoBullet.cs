using UnityEngine;

public class BossTwoBullet : MonoBehaviour
{
    public static int bulletSpeed;
    public static int dmg;

    private Rigidbody2D rigidbody;

   void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

   void Update()
    {
        //transform.position += new Vector3(-bulletSpeed * Time.deltaTime, 0f);
        // this makes sure that bullet's transform/direction is based on its rotation.
        transform.Translate(Vector2.left * bulletSpeed * Time.deltaTime, Space.Self);
        if(transform.position.x < -5)
        {
            gameObject.SetActive(false);
        }    
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
