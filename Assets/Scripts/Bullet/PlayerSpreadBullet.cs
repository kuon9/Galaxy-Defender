using UnityEngine;

public class PlayerSpreadBullet : MonoBehaviour
{
    public int bulletSpeed = 6;
    public int dmg = 1;

    private Rigidbody2D rigidbody;

   void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        transform.SetParent(null);
    }
    void Start()
    {
        transform.SetParent(null);
    }
   void Update()
    {
        //transform.position += new Vector3(bulletSpeed * Time.deltaTime, 0f);
        // this makes sure that bullet's transform/direction is based on its rotation.
        transform.Translate(Vector2.right * bulletSpeed* Time.deltaTime , Space.Self);
        if(transform.position.x > 20)
        {
            gameObject.SetActive(false);
        }    
    }

    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     if(col.gameObject.CompareTag("Obstacles"))
    //     {
    //         Asteroid asteroid = col.gameObject.GetComponent<Asteroid>();
    //         Meteor meteor = col.gameObject.GetComponent<Meteor>();
    //         if(asteroid) asteroid.TakeDamage(1);
    //         if(meteor) meteor.TakeDamage(1);
    //         gameObject.SetActive(false);
    //     }
    //     else if(col.gameObject.CompareTag("Enemy"))
    //     {
    //         // GetComponent of the actual gameobject name and not the tag or layer of it
    //         EnemyShipWave enemyShipWave = col.gameObject.GetComponent<EnemyShipWave>();
    //         EnemyBug enemyBug = col.gameObject.GetComponent<EnemyBug>();
    //         Enemy enemy = col.gameObject.GetComponent<Enemy>();
    //         OctopusWave octopusWave = col.gameObject.GetComponent<OctopusWave>();
    //         BugWave bugWave = col.gameObject.GetComponent<BugWave>();
    //         BeetleWave beetleWave = col.gameObject.GetComponent<BeetleWave>();
    //         if(enemyShipWave) enemyShipWave.TakeDamage(dmg);
    //         if(enemy)enemy.TakeDamage(dmg);
    //         if(octopusWave)octopusWave.TakeDamage(dmg);
    //         if(enemyBug)enemyBug.TakeDamage(dmg);
    //         if(bugWave)bugWave.TakeDamage(dmg);
    //         if(beetleWave)beetleWave.TakeDamage(dmg);   
    //         gameObject.SetActive(false);            
    //     }
    // }
    void OnCollisionEnter2D(Collision2D col)
    {
            // GetComponent of the actual gameobject name and not the tag or layer of it
            EnemyShipWave enemyShipWave = col.gameObject.GetComponent<EnemyShipWave>();
            EnemyBug enemyBug = col.gameObject.GetComponent<EnemyBug>();
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            OctopusWave octopusWave = col.gameObject.GetComponent<OctopusWave>();
            BugWave bugWave = col.gameObject.GetComponent<BugWave>();
            BeetleWave beetleWave = col.gameObject.GetComponent<BeetleWave>();
            if(enemyShipWave) enemyShipWave.TakeDamage(dmg);
            if(enemy)enemy.TakeDamage(dmg);
            if(octopusWave)octopusWave.TakeDamage(dmg);
            if(enemyBug)enemyBug.TakeDamage(dmg);
            if(bugWave)bugWave.TakeDamage(dmg);
            if(beetleWave)beetleWave.TakeDamage(dmg);   
            gameObject.SetActive(false);      
    }
}
