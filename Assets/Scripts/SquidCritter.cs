using UnityEngine;
using System.Collections.Generic;

public class SquidCritter : Enemy
{
    [SerializeField] private Sprite[] sprites;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float moveSpeed;
    private float targetMoveSpeed;

    public override void OnEnable()
    {
        base.OnEnable();
        moveSpeed = 6f;
        targetMoveSpeed = Random.Range(0.8f, 2f);    
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        // randomize sprite on spawn based on array
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        destroyEffectPool = GameObject.Find("SquidCritterPopPool").GetComponent<ObjectPooler>();
        hitSound = AudioManager.instance.squidHit;
        destroySound = AudioManager.instance.squidDestroy;    
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        // we're identifying our target, which is the player's tranform.position;
        targetPosition = PlayerMovement.instance.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        Vector3 relativePos = targetPosition - transform.position; 
        // if relativePOS doesn't equal (0,0,0) vector then execute code below
        if(relativePos != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(Vector3.forward, relativePos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 90 * Time.deltaTime);
        }   
        // this makes a cool effect of increased movement for critters when they spawn into map
        if(moveSpeed != targetMoveSpeed)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, targetMoveSpeed, Time.deltaTime * 4f);
        }
    }
    // public override void TakeDamage(int damage)
    // {
    //     base.TakeDamage(damage);
    //     if(lives <= 0)
    //     {
    //         GameManager.instance.critterCounter++;
    //     }
    // }
}
