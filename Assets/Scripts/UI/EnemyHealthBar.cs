using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Transform foregroundBar;

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
            foregroundBar.localScale = new Vector3(fill, 1, 1);
        }
    }
}
