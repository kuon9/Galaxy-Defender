using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;

    Weapon weapon;

    void Update()
    {
        transform.position += new Vector3(bulletSpeed * Time.deltaTime, 0f);       
        if(transform.position.x > 19)
        {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        weapon = Weapon.instance;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Obstacles"))
        {
            Asteroid asteroid = col.gameObject.GetComponent<Asteroid>();
            Meteor meteor = col.gameObject.GetComponent<Meteor>();
            if(asteroid) asteroid.TakeDamage(1);
            if(meteor) meteor.TakeDamage(1);
            gameObject.SetActive(false);
        }
        else if(col.gameObject.CompareTag("Enemy"))
        {
            // GetComponent of the actual gameobject name and not the tag or layer of it
            EnemyShip enemyship = col.gameObject.GetComponent<EnemyShip>();
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            if(enemyship) enemyship.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            if(enemy)enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            gameObject.SetActive(false);
            Debug.Log("Enemy ship is taking damage");            
        }
    }
}
