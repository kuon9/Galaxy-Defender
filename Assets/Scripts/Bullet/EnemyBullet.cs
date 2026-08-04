using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;
    [SerializeField] int dmg = 1;

   void Update()
    {
        transform.position += new Vector3(-bulletSpeed * Time.deltaTime, 0f);
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
