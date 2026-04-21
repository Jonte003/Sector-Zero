using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth;
    [SerializeField] float regenPerSecond;
    [SerializeField] float visionRange = 5f;
    [SerializeField] PlayerVision playerVision;

    private float HealthAfterBuffs => maxHealth + hpBuffs;

    private float RegenAfterBuffs => regenPerSecond + regenBuffs;

    public float MaxHealth => HealthAfterBuffs;
    public float CurrentHealth => currentHealth;

    public float hpBuffs = 0;
    public float regenBuffs = 0;
    public float damageBuffs = 0;
    public float abilityHasteBuffs = 0;
    public float defenseBuffs = 0;
    public float jumpHeightBuffs = 0;
    public float movementSpeedBuffs = 0;

    // Ability Related
    [HideInInspector] public bool invincible = false;
    

    void Start()
    {
        currentHealth = HealthAfterBuffs;
        playerVision.UpdateVisionRange(visionRange);
    }


    void Update()
    {
        if (currentHealth < HealthAfterBuffs) //Regenerate health
        {
            currentHealth += HealthAfterBuffs * (RegenAfterBuffs / 100) * Time.deltaTime;
            if (currentHealth > HealthAfterBuffs)
                currentHealth = HealthAfterBuffs;
        }
    }

    public void DoDamageDPS(float DPS)
    {
        if (invincible)
            return;

        float modifier = 1 - (defenseBuffs / (defenseBuffs + 100));

        currentHealth -= DPS * modifier * Time.deltaTime;

        if (currentHealth <= 0) //PLAYER DEAD
        {

        }
    }

    public void DoDamageFixed(float amount)
    {
        if (invincible)
            return;

        float modifier = 1 - (defenseBuffs / (defenseBuffs + 100));

        currentHealth -= amount * modifier;


        if (currentHealth <= 0) //PLAYER DEAD
        {
            SceneManager.LoadScene("Title Screen");

        }
    }

    public void ApplyStatBuff(Stat stat)
    {
        if (stat.StatType == PossibleLevelUpStats.Hp)
        {
            hpBuffs += stat.Value;
            currentHealth += stat.Value;
        }
        else if (stat.StatType == PossibleLevelUpStats.Regen)
        {
            regenBuffs += stat.Value;
        }
        else if (stat.StatType == PossibleLevelUpStats.Damage)
        {
            damageBuffs += stat.Value;
        }
        else if (stat.StatType == PossibleLevelUpStats.AbilityHaste)
        {
            abilityHasteBuffs += stat.Value;
        }
        else if (stat.StatType == PossibleLevelUpStats.Defense)
        {
            defenseBuffs += stat.Value;
        }
        else if (stat.StatType == PossibleLevelUpStats.JumpHeight)
        {
            jumpHeightBuffs += stat.Value;
        }
        else if (stat.StatType == PossibleLevelUpStats.MovementSpeed)
        {
            movementSpeedBuffs += stat.Value;
        }
    }
}