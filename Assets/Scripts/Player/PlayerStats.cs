using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth;
    [SerializeField] float regenPerSecond;

    private float HealthAfterBuffs => maxHealth + hpFromLevels;

    private float RegenAfterBuffs => regenPerSecond + regenFromLevels;

    public float MaxHealth => HealthAfterBuffs;
    public float CurrentHealth => currentHealth;

    public float hpFromLevels = 0;
    public float regenFromLevels = 0;
    public float damageFromLevels = 0;
    public float abilityHasteFromLevels = 0;
    public float defenseFromLevels = 0;
    public float jumpHeightFromLevels = 0;
    public float movementSpeedFromLevels = 0;

    public enum PossibleLevelUpStats
    {
        Hp,
        Regen,
        Damage,
        AbilityHaste,
        Defense,
        JumpHeight,
        MovementSpeed
    }
    
    void Start()
    {
        currentHealth = HealthAfterBuffs;
    }


    void Update()
    {
        if (currentHealth < HealthAfterBuffs) //Regenerate health
        {
            currentHealth += RegenAfterBuffs * Time.deltaTime;
            if (currentHealth > HealthAfterBuffs)
                currentHealth = HealthAfterBuffs;
        }
    }


    public void DoDamage(float DPS)
    {
        currentHealth -= DPS * Time.deltaTime;
        if (currentHealth <= 0) //PLAYER DEAD
        {

        }
    }
}
