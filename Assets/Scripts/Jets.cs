using UnityEngine;

public class Jets : MonoBehaviour
{
    
    [SerializeField] GameObject [] blueJets;
    [SerializeField] GameObject [] redJets;

    PowerUpTracker powerUpTracker;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerUpTracker = Object.FindFirstObjectByType<PowerUpTracker>(); 
    }

    // Update is called once per frame
    void Update()
    {
        BlueJetsActive();
        RedJetsActive();    
    }

    public void BlueJetsActive()
    {
        foreach(GameObject jet in blueJets)
        {
            if(!powerUpTracker.isShooting)
            {
                jet.SetActive(true);
            }
            else
            {
                jet.SetActive(false);
            }
        }
    }
    public void RedJetsActive()
    {
        foreach(GameObject jet in redJets)
        {
            if(powerUpTracker.isShooting)
            {
                jet.SetActive(true);
            }
            else
            {
                jet.SetActive(false);
            }
        }
    }

}
