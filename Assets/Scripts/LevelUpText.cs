using UnityEngine;
using System.Collections;

public class LevelUpText : MonoBehaviour
{

    public static LevelUpText instance;
    private Animator anim;
    private bool Leveling;
    private float resetBool = 2f;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        Leveling = false;    
    }

      
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


    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayAnimation()
    {
        StartCoroutine(LevelingUpAnimation());
    }

    IEnumerator LevelingUpAnimation()
    {
        Leveling = true;
        anim.Play("Level");
        yield return new WaitForSeconds(resetBool);
        Leveling = false;        
    }
}
