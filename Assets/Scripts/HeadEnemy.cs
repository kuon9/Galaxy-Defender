using UnityEngine;

public class HeadEnemy : Enemy
{
    
    [SerializeField] private Sprite [] sprites;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float moveSpeed;
    private float targetMoveSpeed;


    public override void OnEnable()
    {
        base.OnEnable();
        moveSpeed = 6f;
        targetMoveSpeed = Random.Range(0.8f, 2f);
        transform.rotation = Quaternion.Euler(0,0,90);
 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();    
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        // assign targetPosition as player's transform.position
        targetPosition = PlayerMovement.instance.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);        
        Vector3 currentPos = targetPosition - transform.position;
        if(currentPos != Vector3.zero)
        {
            
            targetRotation = Quaternion.LookRotation(Vector3.forward, currentPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 90 * Time.deltaTime);
        }
        // this makes a cool effect of increased movement when spawning into the map
        if(moveSpeed != targetMoveSpeed)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, targetMoveSpeed, Time.deltaTime * 4f);
        }    
    }  
}
