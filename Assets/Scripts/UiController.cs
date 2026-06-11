using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
    public static UiController instance;
    [SerializeField] Slider energySlider;
    [SerializeField] TMP_Text energyText;
    [SerializeField] Slider healthSlider;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Slider experienceSlider;
    [SerializeField] TMP_Text experienceText;

    [SerializeField] public GameObject pausePanel;


    void Awake()
    {
        if(instance!= null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void UpdateEnergySlider(float current, float max)
    {
        energySlider.value = Mathf.RoundToInt(current);
        energySlider.maxValue = max;
        energyText.text = energySlider.value + "/" + energySlider.maxValue;
    }

    public void UpdateHealthSlider(float current, float max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = Mathf.RoundToInt(current);
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;        
    }
    public void UpdateExperienceSlider(float current, float max)
    {
        experienceSlider.maxValue = max;
        experienceSlider.value = Mathf.RoundToInt(current);
        experienceText.text = experienceSlider.value + "/" + experienceSlider.maxValue;
    }
    public void DeactivateUI()
    {
        energySlider.gameObject.SetActive(false);
        energyText.gameObject.SetActive(false);
        healthSlider.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
        experienceSlider.gameObject.SetActive(false);
        experienceText.gameObject.SetActive(false);
    }

    public void ActivateUI()
    {
        energySlider.gameObject.SetActive(true);
        energyText.gameObject.SetActive(true);
        healthSlider.gameObject.SetActive(true);
        healthText.gameObject.SetActive(true);
        experienceSlider.gameObject.SetActive(true);
        experienceText.gameObject.SetActive(true);
    }

}
