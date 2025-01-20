using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Energy : Singleton<Energy>
{
    public int CurrentEnergy { get; private set; }

    [SerializeField] private int timeBetweenStaminaRefresh = 3;
    [SerializeField] private int startingEnergy = 5;

    private TMP_Text EneregyStatsText;
    const string ENERGY_STATS_TEXT = "MaxEnergyStats";

    private Slider energySlider;
    public int maxEnergy;
    const string ENERGY_SLIDER_TEXT = "Energy Slider";

    protected override void Awake()
    {
        base.Awake();

        maxEnergy = startingEnergy;
        CurrentEnergy = startingEnergy;
    }

    private void Start()
    {
        energySlider = GameObject.Find(ENERGY_SLIDER_TEXT).GetComponent<Slider>();
        UpdateEnergySlider();
    }

    public void UseStamina()
    {
        if (CurrentEnergy > 0)
        {
            CurrentEnergy--;
            UpdateEnergySlider();
        }
    }

    public void MaxEnergyUpgradePlayer()
    {
        if (EconomyManager.Instance.currentGold >= 7)
        {
            maxEnergy += 1;
            UpdateEnergySlider();
            EconomyManager.Instance.currentGold -= 7;
            EconomyManager.Instance.UpdateCurrentGold();
            if (EneregyStatsText == null)
            {
                EneregyStatsText = GameObject.Find(ENERGY_STATS_TEXT).GetComponent<TMP_Text>();
            }
            EneregyStatsText.text = "Max energy: " + maxEnergy.ToString();
        }
    }

    public void RefreshStamina()
    {
        if (CurrentEnergy < maxEnergy)
        {
            CurrentEnergy++;
            UpdateEnergySlider();
        }
    }

    private IEnumerator RefreshStaminaRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    }

    private void UpdateEnergySlider()
    {
        if (energySlider == null)
        {
            energySlider = GameObject.Find(ENERGY_SLIDER_TEXT).GetComponent<Slider>();
        }

        energySlider.maxValue = maxEnergy;
        energySlider.value = CurrentEnergy;

        if (CurrentEnergy < maxEnergy)
        {
            StopAllCoroutines();
            StartCoroutine(RefreshStaminaRoutine());
        }
    }
}
