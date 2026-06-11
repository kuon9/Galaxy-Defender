using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public static Fade instance;
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeSpeed = 1f;
    
    private IEnumerator fadeRoutine;


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


    public void FadeToBlack()
    {
        if(fadeRoutine != null )
        {
            StopCoroutine(fadeRoutine);
        }
    
        // the param in FadeRoutine is targetting Alpha of the spriterendere so 1 equal full transparency
        // wheres 0 is no transparency
        fadeRoutine= FadeRoutine(1);
        StartCoroutine(fadeRoutine);
    }


    public void FadeToClear()
    {
        if(fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = FadeRoutine(0);
        // can make two couroutine and and assign one couroutine to another
        StartCoroutine(fadeRoutine);
    }



    private IEnumerator FadeRoutine(float targetAlpha)
    {
        while(!Mathf.Approximately(fadeImage.color.a, targetAlpha))
        {
            
            float alpha = Mathf.MoveTowards(fadeImage.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }
    }
}
