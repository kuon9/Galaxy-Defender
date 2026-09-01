using UnityEngine;

public class ScreenClear : MonoBehaviour
{
    public static ScreenClear instance;
    [SerializeField] GameObject nukeSlider;
    Animator anim;
 
    // nuke energy needs to be public so other classes can access
    // aka enemies adding to nuke energy when killed
    public float nukeEnergy;
    [SerializeField] float nukeMaxEnergy;

    [SerializeField] float nukeRegen;
    public ParticleSystem screenNukeParticles;

    public int nukeDamage;
    public int nukeBossDamage;
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
        if(Input.GetKeyDown(KeyCode.R) && nukeEnergy >= 50)
        {
            // logic to destroy every enemy in screen
            screenNukeParticles.Play();
            Nuke();
            Debug.Log("NUKING");
            nukeEnergy = 0;
        }      

        if(nukeEnergy < nukeMaxEnergy)
        {
            nukeEnergy += nukeRegen;
        }
        UiController.instance.UpdateNukeSlider(nukeEnergy,nukeMaxEnergy);  
    }
    void Nuke()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, nukeRange);
        
        foreach(Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            EnemyShipWave enemyShipWave = collider.GetComponent<EnemyShipWave>();
            EnemyBug enemyBug = collider.GetComponent<EnemyBug>();
            OctopusWave octopusWave = collider.GetComponent<OctopusWave>();
            BugWave bugWave = collider.GetComponent<BugWave>();
            // Asteroid asteroid = collider.GetComponent<Asteroid>();
            // Meteor meteor = collider.GetComponent<Meteor>();   

            if(enemy)enemy.TakeDamage(nukeDamage);
            if(enemyShipWave)enemyShipWave.TakeDamage(nukeDamage);
            if(enemyBug)enemyBug.TakeDamage(nukeDamage);
            if(octopusWave)octopusWave.TakeDamage(nukeDamage);
            if(bugWave)bugWave.TakeDamage(nukeDamage);
            // if(asteroid)asteroid.TakeDamage(nukeDamage);
            // if(meteor)meteor.TakeDamage(nukeDamage);
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