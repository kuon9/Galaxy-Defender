using UnityEngine;

public class BossLaser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float maxDistance = 10f;
    public LayerMask hitLayers; // Select layers the laser should stop at

    [SerializeField] int damage = 10;

    void Update()
    {
        // Shoot a raycast forward (using transform.up or transform.right depending on your art orientation)
        // i'm using -transform.up because enemy is facing left, so i want the attack to be facing left also.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, maxDistance, hitLayers);

        // Always set the starting point of the laser line to the emitter
        lineRenderer.SetPosition(0, transform.position);

        if (hit.collider != null)
        {
            // I want laser to extend to maximum distance no matter what even if it hits the player
            // If we hit something, cut the line short at the collision point
            //lineRenderer.SetPosition(1, hit.point);
            
            // Handle dealing damage or triggering events here
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerMovement>().TakeDamage(damage);
                // Player is given iframes when hit by laser only.
                // whereas other attacks don't give players iframes and keep damaging player continiously
                PlayerMovement.instance.iFrames();
            }
        }
        else
        {
            // If nothing is hit, extend the line to its maximum range
            Vector3 maxEndPoint = transform.position + (-transform.up * maxDistance);
            lineRenderer.SetPosition(1, maxEndPoint);
        }
    }
}
