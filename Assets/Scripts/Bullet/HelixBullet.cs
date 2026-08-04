using UnityEngine;

public class HelixBullet : MonoBehaviour
{
    
    public float speed = 2f;
    // how fast for the waves
    public float frequency = 2f;
    // how wide the wave
    public float amplitude = 1f;
    // 0f for first bullet, Math.PI for second bullet
    public float offset = 0f; 

    public int dmg = 3;
    private Vector2 startPosition;
    private Vector2 direction;

    private float timer;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        startPosition = transform.position;

        // direction of travel
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < -5)
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }     
        
        timer += Time.deltaTime * frequency;

        // Calculate wave motion perpendicular to the bullet's direction
        Vector2 forwardMovement = direction * speed * timer;
        Vector2 waveMovement = new Vector2(-direction.y, direction.x) * Mathf.Sin(timer + offset) * amplitude;

        transform.position = startPosition + forwardMovement + waveMovement;           
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            //Player player = col.gameObject.GetComponent<Player>();
            Debug.Log("Taking Damage");
            PlayerMovement.instance.TakeDamage(dmg);
            //gameObject.SetActive(false);
            Destroy(gameObject);    
        }
    }
}
