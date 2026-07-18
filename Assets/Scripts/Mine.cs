using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mine : MonoBehaviour
{
    private ObjectPooler mineExplosionPool;
    [SerializeField] float blastRange;
    public LayerMask Player;

    public int damageAmount;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mineExplosionPool = GameObject.Find("MineExplosionPool").GetComponent<ObjectPooler>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Explode()
    {
        GameObject mineExplosion = mineExplosionPool.GetPooledObject();
        mineExplosion.transform.position = transform.position;
        mineExplosion.transform.rotation = transform.rotation;
        // mineExplosion.localscale so that it doesn't scale to gameobject's scale instead
        mineExplosion.SetActive(true);
        gameObject.SetActive(false);
        Collider2D[] DamagePlayer = Physics2D.OverlapCircleAll(transform.position, blastRange, Player);
        if(DamagePlayer.Length > 0)
        {
                        // All Collider2Ds labeled as col in Enemy 
            foreach(Collider2D col in DamagePlayer)
            {
                if(col.tag == "Player")
                {
                    col.GetComponent<PlayerMovement>().TakeDamage(damageAmount);
                    Debug.Log("Player damaged");    
                }
            }
        }        
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRange);
    }
}
