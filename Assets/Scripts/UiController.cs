using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
    public static UiController instance;
    [SerializeField] Slider energySlider;
    [SerializeField] TMP_Text energyText;
     
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
}
