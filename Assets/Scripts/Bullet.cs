using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;
    void Update()
    {
        transform.position += new Vector3(bulletSpeed * Time.deltaTime, 0f);       
        if(transform.position.x > 19)
        {
            gameObject.SetActive(false);
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Obstacles"))
        {
            Asteroid asteroid = col.gameObject.GetComponent<Asteroid>();
            if(asteroid) asteroid.TakeDamage(1);
            gameObject.SetActive(false);
        }
    }
}
