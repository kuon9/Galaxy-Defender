using UnityEngine;
using System.Collections;

public class FlashWhite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material defaultMaterial;
    private Material whiteMaterial;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        whiteMaterial = Resources.Load<Material>("Materials/mWhite");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flash()
    {
        spriteRenderer.material = whiteMaterial;
        StartCoroutine(ResetMaterial());
    }

    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = defaultMaterial;

    }


    public void Reset()
    {
        if(defaultMaterial) spriteRenderer.material = defaultMaterial;  
    }
}
