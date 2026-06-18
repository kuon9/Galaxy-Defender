using UnityEngine;

public class OrbDamage : MonoBehaviour
{
    
    [SerializeField] int dmg = 10;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            EnemyShip enemyship = col.gameObject.GetComponent<EnemyShip>();
            EnemyBug enemyBug = col.gameObject.GetComponent<EnemyBug>();
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            OctopusWave octopusWave = col.gameObject.GetComponent<OctopusWave>();
            BugWave bugWave = col.gameObject.GetComponent<BugWave>();
            if(enemyship) enemyship.TakeDamage(dmg);
            if(enemy)enemy.TakeDamage(dmg);
            if(octopusWave)octopusWave.TakeDamage(dmg);
            if(enemyBug)enemyBug.TakeDamage(dmg);
            if(bugWave)bugWave.TakeDamage(dmg);                
        }
    }
}
