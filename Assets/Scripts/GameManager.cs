using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance;
    public float mapSpeed;

    private ObjectPooler BossPool;
    public int enemyCounter;
    public bool canSpawn;
    
    
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


    void Start()
    {
        BossPool = GameObject.Find("BossPool").GetComponent<ObjectPooler>();
        enemyCounter = 0;
    }

    void Update()
    {
    if(enemyCounter >= 30)
        {
            enemyCounter = 0;
            GameObject Boss = BossPool.GetPooledObject();
            // this is the transform where the boss will be spawned at
            Boss.transform.position = new Vector2(17f,0);
            Boss.transform.rotation = Quaternion.Euler(0,0,-90);
            Boss.SetActive(true);
        }        
    }
}
