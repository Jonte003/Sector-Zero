using TMPro;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Transform bgBar;
    [SerializeField] Transform fgBar;
    [SerializeField] TextMeshProUGUI healthText;

    private EnemyStats enemyStats;
    private Camera cam;
    void Start()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
        cam = Camera.main;
    }

    void LateUpdate()
    {
        transform.rotation = cam.transform.rotation; //Rotates the health bar to always face the camera

        if (enemyStats.MaxHealth > 0)
        { 
            float fill = enemyStats.Health / enemyStats.MaxHealth;

            RectTransform fgRec = fgBar.GetComponent<RectTransform>();
            RectTransform bgRec = bgBar.GetComponent<RectTransform>();

            fgRec.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bgRec.rect.width * fill);

            healthText.text = Mathf.Ceil(enemyStats.Health).ToString();
        }
    }
}
