using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private PlayerStats playerStats;

    [SerializeField] Transform bgBar;
    [SerializeField] Transform fgBar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI maxHealthText;
    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        float fill = playerStats.CurrentHealth / playerStats.MaxHealth;

        RectTransform fgRec = fgBar.GetComponent<RectTransform>();
        RectTransform bgRec = bgBar.GetComponent<RectTransform>();

        fgRec.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bgRec.rect.width * fill);

        maxHealthText.text = Mathf.Ceil(playerStats.MaxHealth).ToString();
        healthText.text = Mathf.Ceil(playerStats.CurrentHealth).ToString();

        if (playerStats.CurrentHealth <= 0)
        {
            healthText.text = "DEAD";
        }
    }
}
