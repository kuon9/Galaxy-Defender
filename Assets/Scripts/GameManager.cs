using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance;
    public float mapSpeed;

    private ObjectPooler BossPool;
    private ObjectPooler BossTwoPool;
    private ObjectPooler BossThreePool;
    public int enemyCounter;
    public bool canSpawn;
    public bool isTransitioning;
    public bool isSwitchingLevel;
    [SerializeField] float fadeSlowMo = 0.5f;
    [SerializeField] float levelloadDelay = 2f;

    [SerializeField] string mainMenuScene;
    [SerializeField] string currentLevelScene;
    [SerializeField] string nextLevelScene;

    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject levelCompletedUI;

    // [SerializeField] GameObject[] firstObjectSpawners;
    // [SerializeField] GameObject[] secondObjectSpawners;
    // [SerializeField] GameObject  firstWaveSpawner;
    // [SerializeField] GameObject  secondWaveSpawner;
    [SerializeField] GameObject firstLevelBackground;
    [SerializeField] GameObject secondLevelBackground; 
    [SerializeField] GameObject thirdLevelBackground; 

    public GameObject firstLevelChunk;
    public GameObject secondLevelChunk;   
    public GameObject thirdLevelChunk;
    public GameObject victoryScreen;
    private AudioSource bossSpawn;
    
    public bool isLevelOne;
    public bool isLevelTwo;
    
    public bool victory;

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
        BossTwoPool = GameObject.Find("ShipBossPool").GetComponent<ObjectPooler>();
        BossThreePool = GameObject.Find("AlienBossPool").GetComponent<ObjectPooler>();
        enemyCounter = 0;
        bossSpawn = AudioManager.instance.bossSpawnMusic;
        isLevelOne = true;
        victory = false;
    }
    void Update()
    {
    if(isTransitioning)
        {
            Fade.instance.FadeToClear();            
        }
    
    if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
    if(enemyCounter >= 200 & isLevelOne)
        {
            //this disables all spawners so that we only have boss on the screen
            firstLevelChunk.SetActive(false);
            enemyCounter = 0;
            GameObject Boss = BossPool.GetPooledObject();
            AudioManager.instance.PlayModifiedSound(bossSpawn);
            // this is the transform where the boss will be spawned at
            Boss.transform.position = new Vector2(17f,0);
            Boss.transform.rotation = Quaternion.Euler(0,0,-90);
            Boss.SetActive(true);
        }    

    if(enemyCounter >= 200 & !isLevelOne && isLevelTwo)
        {
            secondLevelChunk.SetActive(false);
            enemyCounter = 0;
            GameObject BossTwo = BossTwoPool.GetPooledObject();
            AudioManager.instance.PlayModifiedSound(bossSpawn);
            // this is the transform where the boss will be spawned at
            BossTwo.transform.position = new Vector2(17f,0);
            BossTwo.transform.rotation = Quaternion.Euler(0,0,-90);
            BossTwo.SetActive(true);            
        }
    if(enemyCounter >= 200 & !isLevelOne && !isLevelTwo)
        {
            thirdLevelChunk.SetActive(false);
            enemyCounter = 0;
            GameObject BossThree = BossThreePool.GetPooledObject();
            AudioManager.instance.PlayModifiedSound(bossSpawn);
            // this is the transform where the boss will be spawned at
            BossThree.transform.position = new Vector2(17f,0);
            BossThree.transform.rotation = Quaternion.Euler(0,0,-90);
            BossThree.SetActive(true);                       
        }    
        if(isSwitchingLevel && isLevelOne)
        {
            //SecondLevel();
            StartCoroutine(SecondLevel());            
        }
        if(isSwitchingLevel && !isLevelOne && isLevelTwo)
        {
            StartCoroutine(ThirdLevel());
        }
        if(isSwitchingLevel && victory)
        {
            StartCoroutine(VictoryScreen());
        }
    }
    public void Pause()
    {
        // if the pausepanel is inactive in the hierarchy then setactive will be true
        // when escape or p key is pressed
        if(UiController.instance.pausePanel.activeSelf == false)
        {
            //AudioManager.instance.PlaySound(AudioManager.instance.pause);
            UiController.instance.pausePanel.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true; 

        }
        else
        {
            //AudioManager.instance.PlaySound(AudioManager.instance.unpause);
            UiController.instance.pausePanel.SetActive(false);
            Cursor.visible = false;
            Time.timeScale = 1;
            // makes player exit boost after unpausing
            //Player.instance.NotBoosting();    
        }
    }
    public void BackToMainMenu()
    {
        StartCoroutine(FadingToMainMenu());
    }

    IEnumerator FadingToMainMenu()
    {
        Time.timeScale = 1;
        Fade.instance.FadeToBlack();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(mainMenuScene);
        Fade.instance.FadeToClear();
        // UiController.instance.ResetScore();
        // Player.instance.ResetPlayer();
        UiController.instance.DeactivateUI();        
    }
//     public void NextLevel()
//     {
//         // SceneManager.LoadScene(nextLevelScene);
//         // StartFadeToBlack();
//         StartCoroutine(FadingToNextLevel());
//     }

//   IEnumerator FadingToNextLevel()
//     {
//         Time.timeScale = 1;
//         Fade.instance.FadeToBlack();
//         yield return new WaitForSeconds(2f);
//         //UiController.instance.ActivateUI();
//         SceneManager.LoadScene(nextLevelScene);
//         Fade.instance.FadeToClear();
//         // Player.instance.StartTakingDamage();
//         levelCompletedUI.SetActive(false);
//     }    

    public void Restart()
    {
        StartCoroutine(RestartingLevel());
    }
    
    IEnumerator RestartingLevel()
    {
        Time.timeScale = 1;
        Fade.instance.FadeToBlack();
        //Player.instance.isAlive = true;  
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(currentLevelScene);
        Fade.instance.FadeToClear();
        // UiController.instance.ResetScore();
        // Player.instance.ResetPlayer();
        gameOverUI.SetActive(false);   
    }
    public void GameOver()
    {
        StartCoroutine(GameOverScreen());
        // below is same function as above but more precise
        //SceneManager.LoadScene("Game Over");

    }
    IEnumerator GameOverScreen()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }
    public void Quit()
    {
        Fade.instance.FadeToBlack();
        Application.Quit();
    }
   public void ActivateLevelCompletedUI()
    {
        Time.timeScale = 0;
        levelCompletedUI.SetActive(true);
    }

    // this allows new sets of enemies to spawn 
    // we disable one objectspawner with old enemies loaded
    // and we enable a second objectspawner with new enemies loaded
    // we apply this same principle to the enemywaveSpawner
    // public void SecondLevel()
    // {
    //     isLevelOne = false;
    //     foreach (GameObject obj in firstObjectSpawners)
    //     {
    //         obj.SetActive(false);
    //     }
    //     foreach(GameObject obj in secondObjectSpawners)
    //     {
    //         obj.SetActive(true);
    //     }
    //     firstWaveSpawner.SetActive(false);
    //     secondWaveSpawner.SetActive(true);
    //     firstLevelBackground.SetActive(false);
    //     secondLevelBackground.SetActive(true);
    // }

    // this method is way more easier and efficient.
    // Make a Master Parent GameObject and it setactive false or true every children below
    // public void SecondLevel()
    // {
    //     //firstLevelChunk.SetActive(false);
    //     isLevelOne = false;
    //     firstLevelBackground.SetActive(false);
    //     secondLevelBackground.SetActive(true);
    //     secondLevelChunk.SetActive(true);    
    // }

    // fade between next level.
    IEnumerator SecondLevel()
    {
        Fade.instance.FadeToBlack();
        Time.timeScale = fadeSlowMo;
        yield return new WaitForSeconds(1.5f);
        Fade.instance.FadeToClear();
        Time.timeScale = 1;
        isLevelOne = false;
        isLevelTwo = true;
        firstLevelBackground.SetActive(false);
        secondLevelBackground.SetActive(true);
        secondLevelChunk.SetActive(true);
        // this coroutine is running over and over.
        // we gotta make isSwitchingBoolean to false so coroutine stops running because of condition
        isSwitchingLevel = false;          
    }
   IEnumerator ThirdLevel()
    {
        Fade.instance.FadeToBlack();
        Time.timeScale = fadeSlowMo;
        yield return new WaitForSeconds(1.5f);
        Fade.instance.FadeToClear();
        Time.timeScale = 1;
        //isLevelOne = false;
        isLevelTwo = false;
        secondLevelBackground.SetActive(false);
        thirdLevelBackground.SetActive(true);
        thirdLevelChunk.SetActive(true);
        // this coroutine is running over and over.
        // we gotta make isSwitchingBoolean to false so coroutine stops running because of condition
        isSwitchingLevel = false;          
    }

    IEnumerator VictoryScreen()
    {
        Fade.instance.FadeToBlack();
        Time.timeScale = fadeSlowMo;
        yield return new WaitForSeconds(1.5f);
        Fade.instance.FadeToClear();
        Time.timeScale = 1;
        victoryScreen.SetActive(true);        
    }

}
