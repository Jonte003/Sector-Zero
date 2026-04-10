using TMPro;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Transform bgBar;
    [SerializeField] Transform barSeg5;
    [SerializeField] Transform barSeg10;
    [SerializeField] Transform barSeg15;
    [SerializeField] Transform barSeg20;
    [SerializeField] Transform barSeg25;
    [SerializeField] Transform barSeg30;
    [SerializeField] Transform barSeg35;
    [SerializeField] Transform barSeg40;
    [SerializeField] Transform barSeg45;
    [SerializeField] Transform barSeg50;
    [SerializeField] Transform barSeg55;
    [SerializeField] Transform barSeg60;
    [SerializeField] Transform barSeg65;
    [SerializeField] Transform barSeg70;
    [SerializeField] Transform barSeg75;
    [SerializeField] Transform barSeg80;
    [SerializeField] Transform barSeg85;
    [SerializeField] Transform barSeg90;
    [SerializeField] Transform barSeg95;
    [SerializeField] Transform barSeg100;

    Transform[] barSegments;

    private EnemyStats enemyStats;
    private Camera cam;
    void Start()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
        cam = Camera.main;
        barSegments = new Transform[] { barSeg5, barSeg10, barSeg15, barSeg20, barSeg25, barSeg30, barSeg35, barSeg40, barSeg45, barSeg50, barSeg55, barSeg60, barSeg65, barSeg70, barSeg75, barSeg80, barSeg85, barSeg90, barSeg95, barSeg100 };
    }

    void LateUpdate()
    {
        transform.rotation = cam.transform.rotation; //Rotates the health bar to always face the camera

        if (enemyStats.MaxHealth > 0)
        { 
            float fill = enemyStats.Health / enemyStats.MaxHealth;
            for (int i = 0; i < barSegments.Length; i++)
            {
                if (fill >= (i + 1) * 0.05f)
                {
                    barSegments[i].gameObject.SetActive(true);
                }
                else
                {
                    barSegments[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
