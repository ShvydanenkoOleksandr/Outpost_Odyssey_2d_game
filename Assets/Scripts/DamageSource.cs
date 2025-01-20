using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageSource : Singleton<DamageSource>
{
    [SerializeField] public int damageAmount = 1;

    private TMP_Text DamageText;
    const string DAMAGE_TEXT = "DamageStats";

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDamage(damageAmount);
    }

    public void UpgradeDamage()
    {
        if (EconomyManager.Instance.currentGold >= 20)
        {
            damageAmount += 1;
            EconomyManager.Instance.currentGold -= 20;
            EconomyManager.Instance.UpdateCurrentGold();
            if (DamageText == null)
            {
                DamageText = GameObject.Find(DAMAGE_TEXT).GetComponent<TMP_Text>();
            }
            DamageText.text = "Damage per hit: " + damageAmount.ToString();
        }

    }
}
