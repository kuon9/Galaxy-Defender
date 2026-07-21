using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BossHomingProjectile : MonoBehaviour
{
    
    public float speed = 5f;
    public float rotateSpeed = 200f;

    public Transform playerTransform;
    private Rigidbody2D rigidbody;

    private ObjectPooler explosionVFX;
    public int dmg;

    public float homingProjectileTimer;
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        explosionVFX = GameObject.Find("GreenHomingVFXPool").GetComponent<ObjectPooler>();
    }

    void OnEnable()
    {
        GameObject player = GameObject.FindWithTag("Player");
        // if player is available then playertransform = player.transform
        if(player != null)
        {
            // we're assigning it here because we can't assign it in the prefab
            playerTransform = player.transform;
        }
        StartCoroutine(Timer());
    }

    void FixedUpdate()
    {
        // if playertransform is unassigned, or if variable is empty the it's null
        if(playerTransform == null) return;
        
        // Calculate the direction vector to the player
        Vector2 direction = (Vector2)playerTransform.position - rigidbody.position;
        direction.Normalize();

        //  Calculate the rotation required to face the player
        // Vector2.right assumes the projectile sprite faces right by default
        float rotateAmount = Vector3.Cross(direction, transform.right).z;

        //  Apply angular velocity to steer toward the player
        rigidbody.angularVelocity = -rotateAmount * rotateSpeed;

        //  Move forward in the direction the projectile is facing
        rigidbody.linearVelocity = transform.right * speed;        
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        // Handle impact logic here (e.g., damage player, spawn explosion VFX)
        if(col.gameObject.CompareTag("Player"))
        {
            GameObject explosion = explosionVFX.GetPooledObject();
            explosion.transform.position = transform.position;
            explosion.transform.rotation = transform.rotation;
            explosion.SetActive(true);
            PlayerMovement.instance.TakeDamage(dmg);
            gameObject.SetActive(false);    
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(homingProjectileTimer);
        GameObject explosion = explosionVFX.GetPooledObject();
        explosion.transform.position = transform.position;
        explosion.transform.rotation = transform.rotation;
        explosion.SetActive(true);
        gameObject.SetActive(false);  
    }

}
