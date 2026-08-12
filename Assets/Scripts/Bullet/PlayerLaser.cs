using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    
    [SerializeField] float damagePerSecond = 40f;
    // Time in seconds between each tick
    public LineRenderer lineRenderer;
    public float maxDistance = 10f;
    public LayerMask hitLayers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        Vector2 startPoint = transform.position;
        Vector2 endPoint = startPoint + (Vector2)transform.up * maxDistance;
        // Shoot a raycast forward (using transform.up or transform.right depending on your art orientation)
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, maxDistance, hitLayers);
        RaycastHit2D [] hits = Physics2D.LinecastAll(startPoint, endPoint, hitLayers);

        // Always set the starting point of the laser line to the emitter
        lineRenderer.SetPosition(0, transform.position);

        //if (hit.collider != null)
        foreach (RaycastHit2D hit in hits)
        {
            // I want laser to extend to maximum distance no matter what even if it hits the player
            // If we hit something, cut the line short at the collision point
            // lineRenderer.SetPosition(1, hit.point);
            
            // Handle dealing damage or triggering events here
            //if(hit.collider.gameObject.CompareTag("Obstacles"))
            //{
                Asteroid asteroid = hit.collider.GetComponent<Asteroid>();
                Meteor meteor = hit.collider.GetComponent<Meteor>();
                if(asteroid) asteroid.TakeDamage(damagePerSecond  * Time.deltaTime);
                if(meteor) meteor.TakeDamage(damagePerSecond  * Time.deltaTime);
                gameObject.SetActive(false);
            //}
            //else if (hit.collider.CompareTag("Enemy"))
            //{
                EnemyShipWave enemyShipWave = hit.collider.GetComponent<EnemyShipWave>();
                EnemyBug enemyBug = hit.collider.GetComponent<EnemyBug>();
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                OctopusWave octopusWave = hit.collider.GetComponent<OctopusWave>();
                BugWave bugWave = hit.collider.GetComponent<BugWave>();
                BeetleWave beetleWave = hit.collider.GetComponent<BeetleWave>();
                if(enemyShipWave) enemyShipWave.TakeDamage(damagePerSecond  * Time.deltaTime);
                if(enemy)enemy.TakeDamage(damagePerSecond  * Time.deltaTime);
                if(octopusWave)octopusWave.TakeDamage(damagePerSecond  * Time.deltaTime);
                if(enemyBug)enemyBug.TakeDamage(damagePerSecond  * Time.deltaTime);
                if(bugWave)bugWave.TakeDamage(damagePerSecond  * Time.deltaTime);
                if(beetleWave)beetleWave.TakeDamage(damagePerSecond  * Time.deltaTime);   
                // Player is given iframes when hit by laser only.
                // whereas other attacks don't give players iframes and keep damaging player continiously
        }
        //
        //{
            // If nothing is hit, extend the line to its maximum range
            Vector3 maxEndPoint = transform.position + (transform.up * maxDistance);
            lineRenderer.SetPosition(1, maxEndPoint);
        //}        
    }
}
