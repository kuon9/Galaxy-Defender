using UnityEngine;

public class HoldDownToPlay : MonoBehaviour
{
    public AudioSource audioSource;
    public bool stopOnRelease = false;
    
    void Update()
    {
        if(PlayerMovement.instance.boosting)
        {
            if(!audioSource.isPlaying)
            {
                audioSource.Play();        
            }
        }
        else if(!PlayerMovement.instance.boosting)
        {
            audioSource.Stop();        
        }
    }
}
