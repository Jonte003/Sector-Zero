using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth;
    [SerializeField] float RegenPerSecond;
    [SerializeField] float experience;
    void Start()
    {
        currentHealth = maxHealth;
    }


    void Update()
    {


        if (currentHealth < maxHealth) //Regenerate health
        {
            currentHealth += RegenPerSecond * Time.deltaTime;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
    }


    public void DoDamage(float DPS)
    {
        currentHealth -= DPS * Time.deltaTime;
        if (currentHealth <= 0) //PLAYER DEAD
        {

        }
    }

    public void IncreaseMaxHealthByFixedAmount(float amount)
    {
        maxHealth += amount;
    }

    public void IncreaseMaxHealthByProcentage(float procentInDecimal)
    {
        maxHealth *= procentInDecimal;
    }

    public void AddExperience(float amount)
    {
        experience += amount;
    }
}
