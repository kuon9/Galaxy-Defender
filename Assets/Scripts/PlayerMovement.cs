using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;


    private Rigidbody2D rb;
    private Animator anim;
    private Vector2  playerDirection;
    [SerializeField] float moveSpeed;

    public float boost = 1f;
    private float boostPower = 5f;
    
    
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        float directionX = Input.GetAxisRaw("Horizontal");
        float directionY = Input.GetAxisRaw("Vertical");
        anim.SetFloat("moveX", directionX);
        anim.SetFloat("moveY", directionY);
        playerDirection = new Vector2(directionX, directionY).normalized;

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2"))
        {
            Boosting();
        }
        else if(Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2"))
        {
            StopBoosting();
        }
    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(playerDirection.x * moveSpeed, playerDirection.y * moveSpeed);
    }

    void Boosting()
    {
        anim.SetBool("Boosting", true);
        boost = boostPower;
    }

    void StopBoosting()
    {
        anim.SetBool("Boosting", false);
        boost = 1f;
    }
}
