using UnityEngine;

public class OrbDamage : MonoBehaviour
{
    
    [SerializeField] int dmg = 10;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy" || col.tag == "Obstacles")
        {
            EnemyShipWave enemyShipWave = col.gameObject.GetComponent<EnemyShipWave>();
            EnemyBug enemyBug = col.gameObject.GetComponent<EnemyBug>();
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            OctopusWave octopusWave = col.gameObject.GetComponent<OctopusWave>();
            BugWave bugWave = col.gameObject.GetComponent<BugWave>();
            Asteroid asteroid = col.gameObject.GetComponent<Asteroid>();
            Meteor meteor = col.gameObject.GetComponent<Meteor>();
            BeetleWave beetleWave = col.gameObject.GetComponent<BeetleWave>();
            if(enemyShipWave) enemyShipWave.TakeDamage(dmg);
            if(enemy)enemy.TakeDamage(dmg);
            if(octopusWave)octopusWave.TakeDamage(dmg);
            if(enemyBug)enemyBug.TakeDamage(dmg);
            if(bugWave)bugWave.TakeDamage(dmg);
            if(asteroid)asteroid.TakeDamage(dmg);
            if(meteor)meteor.TakeDamage(dmg);
            if(beetleWave)beetleWave.TakeDamage(dmg);                
        }
    }
}
