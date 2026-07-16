using UnityEngine;

public class BossLaser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float maxDistance = 20f;
    public LayerMask hitLayers; // Select layers the laser should stop at

    [SerializeField] int damage = 10;

    void Update()
    {
        // Shoot a raycast forward (using transform.up or transform.right depending on your art orientation)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, maxDistance, hitLayers);

        // Always set the starting point of the laser line to the emitter
        lineRenderer.SetPosition(0, transform.position);

        if (hit.collider != null)
        {
            // If we hit something, cut the line short at the collision point
            lineRenderer.SetPosition(1, hit.point);
            
            // Handle dealing damage or triggering events here
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerMovement>().TakeDamage(damage);
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
