using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] public int maxHealth = 30;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    public bool isDead { get; private set; }

    private Slider healthSlider;
    public int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;
    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string TOWN_TEXT = "Home";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    private TMP_Text MaxHealthText;
    const string MAX_HEALTH_TEXT = "MaxHealthStats";

    private TMP_Text CurHealthText;
    const string CUR_HEALTH_TEXT = "CurHealthStats";



    protected override void Awake()
    {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy && canTakeDamage)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void RecoveryPlayer()
    {
        if (EconomyManager.Instance.currentGold >= 1)
        {
            currentHealth = maxHealth;
            UpdateHealthSlider();
            EconomyManager.Instance.currentGold -= 1;
            EconomyManager.Instance.UpdateCurrentGold();
            if (CurHealthText == null)
            {
                CurHealthText = GameObject.Find(CUR_HEALTH_TEXT).GetComponent<TMP_Text>();
            }

            CurHealthText.text = "Current health: " + currentHealth.ToString();
        }
    }

    public void MaxHealthUpgradePlayer()
    {
        if (EconomyManager.Instance.currentGold >=3)
        {
            maxHealth += 1;
            UpdateHealthSlider();
            EconomyManager.Instance.currentGold -= 3;
            EconomyManager.Instance.UpdateCurrentGold();
            if (MaxHealthText == null)
            {
                MaxHealthText = GameObject.Find(MAX_HEALTH_TEXT).GetComponent<TMP_Text>();
            }

            MaxHealthText.text = "Max health: "+maxHealth.ToString();
        }
    }


    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        //StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }

    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        EconomyManager.Instance.currentGold = 0;
        EconomyManager.Instance.UpdateCurrentGold();
        SceneManager.LoadScene(TOWN_TEXT);
    }


    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

}