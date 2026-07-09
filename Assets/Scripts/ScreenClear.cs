using UnityEngine;

public class ScreenClear : MonoBehaviour
{
    public static ScreenClear instance;
    [SerializeField] GameObject nukeSlider;
    Animator anim;
 
    public int nukeEnergy;
    public int nukeMaxEnergy;
    public ParticleSystem screenNukeParticles;

    public int nukeDamage;
    public float nukeRange;

    //public GameObject [] enemies;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenNukeParticles = GetComponentInChildren<ParticleSystem>();
        //nukeEnergy = 0;
        anim = nukeSlider.GetComponent<Animator>();
        // looks for all gameobject with enemy tags
        // however, since its only at start, means it won't scan new enemies
        // that will spawn into the scene
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // always need this if we're making an public static instance
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
        // if(Input.GetKeyDown(KeyCode.R) && nukeMaxEnergy == 50)
        // {
        //     // logic to destroy every enemy in screen
        //     Nuke();
        //     nukeEnergy = 0;
        // }        

        NukeisReady();
        UiController.instance.UpdateNukeSlider(nukeEnergy,nukeMaxEnergy);
        if(Input.GetKeyDown(KeyCode.R) && nukeEnergy >= 50)
        {
            // logic to destroy every enemy in screen
            screenNukeParticles.Play();
            Nuke();
            Debug.Log("NUKING");
            nukeEnergy = 0;
        }        
    }
    void Nuke()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, nukeRange);
        
        foreach(Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            EnemyShip enemyship = collider.GetComponent<EnemyShip>();
            EnemyBug enemyBug = collider.GetComponent<EnemyBug>();
            OctopusWave octopusWave = collider.GetComponent<OctopusWave>();
            BugWave bugWave = collider.GetComponent<BugWave>();
            Asteroid asteroid = collider.GetComponent<Asteroid>();
            Meteor meteor = collider.GetComponent<Meteor>();     

            if(enemy)enemy.TakeDamage(nukeDamage);
            if(enemyship)enemyship.TakeDamage(nukeDamage);
            if(enemyBug)enemyBug.TakeDamage(nukeDamage);
            if(octopusWave)octopusWave.TakeDamage(nukeDamage);
            if(bugWave)bugWave.TakeDamage(nukeDamage);
            if(asteroid)asteroid.TakeDamage(nukeDamage);
            if(meteor)meteor.TakeDamage(nukeEnergy);
        }
    }

    void NukeisReady()
    {
        if(nukeEnergy >= 50)
        {
            anim.SetBool("NukeReady", true);
            UiController.instance.nukeText.text = "Nuke!";
        }
        else
        {
            anim.SetBool("NukeReady", false);
            UiController.instance.nukeText.text = "";
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, nukeRange);    
    }

}