using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string nextlevelScene;
    [SerializeField] private string sceneTransitionName;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void NewGame()
    {
        Fade.instance.FadeToBlack();
        StartCoroutine(FadeBeforeTransition());
    }
    private IEnumerator FadeBeforeTransition()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextlevelScene);
    }
    public void Quit()
    {
        Application.Quit();
    }    
}
