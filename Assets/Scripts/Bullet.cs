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
            EnemyShipWave enemyShipWave = col.gameObject.GetComponent<EnemyShipWave>();
            EnemyBug enemyBug = col.gameObject.GetComponent<EnemyBug>();
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            OctopusWave octopusWave = col.gameObject.GetComponent<OctopusWave>();
            BugWave bugWave = col.gameObject.GetComponent<BugWave>();
            BeetleWave beetleWave = col.gameObject.GetComponent<BeetleWave>();
            if(enemyShipWave) enemyShipWave.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            if(enemy)enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            if(octopusWave)octopusWave.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            if(enemyBug)enemyBug.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            if(bugWave)bugWave.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            if(beetleWave)beetleWave.TakeDamage(weapon.stats[weapon.weaponLevel].damage);   
            gameObject.SetActive(false);            
        }
    }
}
