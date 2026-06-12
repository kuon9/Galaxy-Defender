using UnityEngine;

public class AudioManager : MonoBehaviour
{
public static AudioManager instance;

    public AudioSource monsterHit;
    public AudioSource laserShoot;
    public AudioSource backGroundMusic;
    public AudioSource missileShoot;
    public AudioSource hitImpact;
    public AudioSource hitRock;
    public AudioSource bossDeath;
    public AudioSource bossSpawnMusic;
    public AudioSource monsterDeath;
    public AudioSource playerDeathExplosion;
    public AudioSource boostSound;
    public AudioSource shipExplosion;

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
    public void PlaySound(AudioSource sound)
    {
        sound.Stop();
        sound.Play();
    }

    public void PlayModifiedSound(AudioSource sound)
    {
        sound.pitch = Random.Range(0.7f, 1.3f);
        sound.Stop();
        sound.Play();
    }
}
