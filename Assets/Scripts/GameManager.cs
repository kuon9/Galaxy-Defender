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
    public int enemyCounter;
    public bool canSpawn;
    public bool isTransitioning;
    public bool isSwitchingLevel;

    [SerializeField] string mainMenuScene;
    [SerializeField] string currentLevelScene;
    [SerializeField] string nextLevelScene;

    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject levelCompletedUI;

    [SerializeField] GameObject[] firstObjectSpawners;
    [SerializeField] GameObject[] secondObjectSpawners;
    [SerializeField] GameObject  firstWaveSpawner;
    [SerializeField] GameObject  secondWaveSpawner;    
    private AudioSource bossSpawn;
    

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
        bossSpawn = AudioManager.instance.bossSpawnMusic;
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
    if(enemyCounter >= 200)
        {
            enemyCounter = 0;
            GameObject Boss = BossPool.GetPooledObject();
            AudioManager.instance.PlayModifiedSound(bossSpawn);
            // this is the transform where the boss will be spawned at
            Boss.transform.position = new Vector2(17f,0);
            Boss.transform.rotation = Quaternion.Euler(0,0,-90);
            Boss.SetActive(true);
        }        
        if(isSwitchingLevel)
        {
            NewLevel();            
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
    public void NextLevel()
    {
        // SceneManager.LoadScene(nextLevelScene);
        // StartFadeToBlack();
        StartCoroutine(FadingToNextLevel());
    }

  IEnumerator FadingToNextLevel()
    {
        Time.timeScale = 1;
        Fade.instance.FadeToBlack();
        yield return new WaitForSeconds(2f);
        //UiController.instance.ActivateUI();
        SceneManager.LoadScene(nextLevelScene);
        Fade.instance.FadeToClear();
        // Player.instance.StartTakingDamage();
        levelCompletedUI.SetActive(false);
    }    

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

    public void NewLevel()
    {
        foreach (GameObject obj in firstObjectSpawners)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in secondObjectSpawners)
        {
            obj.SetActive(true);
        }
        firstWaveSpawner.SetActive(false);
        secondWaveSpawner.SetActive(true);
    }
}
