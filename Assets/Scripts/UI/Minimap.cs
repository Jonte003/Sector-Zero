using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [Header("References")]
    public Transform Player;
    public RectTransform MinimapContent;
    public RectTransform PlayerDot;
    public GameObject EnemyDotPrefab;
    public Transform EnemyDotParent;
    [SerializeField] public RectTransform PlayerVisionCircle;

    [Header("Level Bounds (world space)")]
    public Vector2 LevelMin;
    public Vector2 LevelMax;

    [Header("Dot Colors")]
    [SerializeField] private Color enemySeenColor;
    [SerializeField] private Color enemyUnseenColor;

    List<Transform> enemies;

    private float playerVisionRange;

    private Dictionary<Transform, RectTransform> trackedEnemies = new();
    private void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        enemies = GameObject.FindWithTag("EnemyController").GetComponent<Controller>().AllEnemies;
    }
    void LateUpdate()
    {
        //Debug.Log($"MapSize: {MinimapContent.rect.width} x {MinimapContent.rect.height} | LevelMin: {LevelMin} | LevelMax: {LevelMax} | PlayerWorld: {Player.position}");

        playerVisionRange = Player.GetComponent<PlayerStats>().VisionRange;
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        #region Start tracking each enemy that is not already tracked
        foreach (Transform enemy in enemies)
        {
            TrackEnemy(enemy);
        }
        #endregion
        #region Stop tracking enemies that no longer exist
        foreach (Transform t in new List<Transform>(trackedEnemies.Keys))
        {
            if (t == null) UntrackEnemy(t);
        }
        #endregion

        PlayerDot.anchoredPosition = WorldToMinimap(Player.position);
        PlayerVisionCircle.sizeDelta = Vector2.one * (playerVisionRange / (LevelMax.x - LevelMin.x) * MinimapContent.rect.width) * 2;

        foreach (var (enemyTransform, enemyDot) in trackedEnemies)
        {
            enemyDot.anchoredPosition = WorldToMinimap(enemyTransform.position);
            enemyDot.localRotation = Quaternion.Euler(0, 0, -enemyTransform.eulerAngles.y + 90);

            float distanceToPlayer = Vector3.Distance(Player.position, enemyTransform.position);
            Image dotImage = enemyDot.GetComponent<Image>();
            if (distanceToPlayer <= playerVisionRange) { dotImage.color = enemySeenColor; }
            else { dotImage.color = enemyUnseenColor; }
        }

        PlayerDot.localRotation = Quaternion.Euler(0, 0, -Player.eulerAngles.y + 90);
    }

    Vector2 WorldToMinimap(Vector3 worldPos)
    {
        float normX = Mathf.InverseLerp(LevelMin.x, LevelMax.x, worldPos.x);
        float normY = Mathf.InverseLerp(LevelMin.y, LevelMax.y, worldPos.z);

        float mapWidth = MinimapContent.rect.width;
        float mapHeight = MinimapContent.rect.height;

        return new Vector2(normX * mapWidth - mapWidth/ 2, normY * mapHeight - mapHeight / 2);
    }

    public void TrackEnemy(Transform enemy)
    {
        if (trackedEnemies.ContainsKey(enemy)) return;

        GameObject dot = Instantiate(EnemyDotPrefab, EnemyDotParent);
        trackedEnemies[enemy] = dot.GetComponent<RectTransform>();
    }

    public void UntrackEnemy(Transform enemy)
    {
        if (trackedEnemies.TryGetValue(enemy, out var dot))
        {
            Destroy(dot.gameObject);
            trackedEnemies.Remove(enemy);
        }
    }
}
