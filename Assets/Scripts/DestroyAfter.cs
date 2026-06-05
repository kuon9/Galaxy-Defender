using UnityEngine;
using System.Collections;
public class DestroyAfter : MonoBehaviour
{
    private Animator anim;
    
    
    void OnEnable()
    {
        anim = GetComponent<Animator>();
        StartCoroutine("Deactivate"); 
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
    }
}
