using UnityEngine;

public class DroneFollowPlayer : MonoBehaviour
{
    
    [SerializeField] Transform player;
    
    // this prevents drone from overlapping player object
    public Vector2 offset = new Vector2(-2f,0f);
    public float delayTime = 0.3f;

    private Vector2 velocity = Vector2.zero;

    [SerializeField] int speed;
    
    private Rigidbody2D rigidbody;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player == null) return;
        Vector2 targetPlayer = (Vector2)player.position + offset;
        rigidbody.position = Vector2.SmoothDamp(rigidbody.position, targetPlayer, ref velocity, delayTime, speed);
    }
}
