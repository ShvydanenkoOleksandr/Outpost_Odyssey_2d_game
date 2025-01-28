using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class OpenStats : Singleton<OpenStats>
{
    private GameObject stats;

    private TMP_Text CurHealthText;
    const string CUR_HEALTH_TEXT = "CurHealthStats";
    private TMP_Text EneregyStatsText;
    const string ENERGY_STATS_TEXT = "MaxEnergyStats";
    private TMP_Text DamageText;
    const string DAMAGE_TEXT = "DamageStats";
    private TMP_Text MaxHealthText;
    const string MAX_HEALTH_TEXT = "MaxHealthStats";

    void Start()
    {
        stats = GameObject.Find("StatsPanel");
        if (stats != null)
        {
            stats.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (stats != null && other.gameObject.GetComponent<PlayerController>())
        {
            stats.SetActive(true);
            if (CurHealthText == null)
            {
                CurHealthText = GameObject.Find(CUR_HEALTH_TEXT).GetComponent<TMP_Text>();
            }
            CurHealthText.text = "Current health: " + PlayerHealth.Instance.currentHealth.ToString();

            if (MaxHealthText == null)
            {
                MaxHealthText = GameObject.Find(MAX_HEALTH_TEXT).GetComponent<TMP_Text>();
            }
            MaxHealthText.text = "Max health: " + PlayerHealth.Instance.maxHealth.ToString();

            if (EneregyStatsText == null)
            {
                EneregyStatsText = GameObject.Find(ENERGY_STATS_TEXT).GetComponent<TMP_Text>();
            }
            EneregyStatsText.text = "Max energy: " + Energy.Instance.maxEnergy.ToString();

            /*
             * error NullReferenceException: Object reference not set to an instance of an object
OpenStats.OnTriggerEnter2D (UnityEngine.Collider2D other) (at Assets/Scripts/OpenStats.cs:63)
            if (DamageSource.Instance != null)
            {
                if (DamageText == null)
                {
                    DamageText = GameObject.Find(DAMAGE_TEXT).GetComponent<TMP_Text>();
                }
                DamageText.text = "Damage per hit: " + DamageSource.Instance.damageAmount.ToString();
            }
            else
            {
                DamageText.text = "Damage per hit: 1";
            }
            */
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (stats != null && other.gameObject.GetComponent<PlayerController>())
        {
            stats.SetActive(false);
        }
    }
}
