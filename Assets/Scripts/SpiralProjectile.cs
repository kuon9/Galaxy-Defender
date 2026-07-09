using UnityEngine;

public class SpiralProjectile : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Speed of the clockwise spiral
    public float expansionRate = 2f;  // How fast the projectile moves outward
    public float startSpeed = 5f;     // Base speed of the projectile

    private float angle = 0f;
    private float timeElapsed = 0f;
    private Vector2 centerPoint;

    void Start()
    {
        // Store the starting position as the center of the spiral
        centerPoint = transform.position;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Increase angle over time (Subtracting creates a CLOCKWISE spiral)
        angle -= rotationSpeed * Time.deltaTime;

        // Radius expands the further out it goes
        float currentRadius = expansionRate * timeElapsed;

        // Calculate new X and Y using trig (cos is x, sin is y)
        float x = centerPoint.x + (Mathf.Cos(angle) * currentRadius);
        float y = centerPoint.y + (Mathf.Sin(angle) * currentRadius);

        // Move the object to the new position
        transform.position = new Vector2(x, y);
    }
}
