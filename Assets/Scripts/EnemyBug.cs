using UnityEngine;

public class EnemyBug : Enemy
{
    [SerializeField] private Sprite[] sprites;
    private float timer;
    private float frequency;
    private float amplitude;
    private float centerY;

    public override void OnEnable()
    {
        base.OnEnable();
        // enemy sprite looks at player when spawning rather than original top down 
        transform.rotation = Quaternion.Euler(0,0,-90);
        timer = transform.position.y;
        frequency = Random.Range(0.5f, 1f);
        amplitude = Random.Range(1f, 1.5f);
        centerY = transform.position.y;
    }

    // only child classes can use public override void
    public override void Start()
    {
        // this runs the start method from the base/parent/Super-class
        base.Start();
        // anything below base.Start(); is unique to this child class
        // enemy can spawn in any of the 4 sprites randomly 
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
        hitSound = AudioManager.instance.hitImpact;
        destroySound = AudioManager.instance.monsterDeath;
        speedX = Random.Range(-0.8f, -1.5f);
    }
    public override void Update()
    {
        // runs Update method from base/parent/super-class first
        base.Update();

        // this makes enemy go up and down like a sine wave
        timer -= Time.deltaTime;
        float sine = Mathf.Sin(timer * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, centerY + sine);
        // original sprite is top down so we're doing -90 in Z rotation
        // this makes sprite look towards the player sprite because its a horizontal shooter
        // Vector3 lookDir = new Vector3(0, 0, -90);
        // Quaternion rotation = Quaternion.Euler(lookDir);
        // this makes a lower frequency sine wave movement
        // float sine = Mathf.Sin(timer);  
        // this code below makes a more aggressive sine wave movement
        //float sine = Mathf.Sin(transform.position.x);
        // transform.position = new Vector3(transform.position.x,sine);
    }
}