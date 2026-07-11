using UnityEngine;

public class YellowShipWave : EnemyShipWave
{
    
    public override void Awake()
    {
        base.Awake();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        projectilePool = GameObject.Find("EnemyBulletThreePool").GetComponent<ObjectPooler>();    
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>(); 
        destroySound = AudioManager.instance.shipExplosion;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();  
    }


}
