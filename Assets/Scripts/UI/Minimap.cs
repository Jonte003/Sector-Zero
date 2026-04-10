using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [Header("References")]
    public Transform Player;
    public RectTransform MinimapContent;
    public RectTransform PlayerDot;
    public GameObject EnemyDotPrefab;
    public Transform EnemyDotParent;

    [Header("Level Bounds (world space)")]
    public Vector2 LevelMin;
    public Vector2 LevelMax;

    private Dictionary<Transform, RectTransform> trackedEnemies = new();

    void LateUpdate()
    {
        //Debug.Log($"MapSize: {MinimapContent.rect.width} x {MinimapContent.rect.height} | LevelMin: {LevelMin} | LevelMax: {LevelMax} | PlayerWorld: {Player.position}");

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        #region Start tracking each enemy that is not already tracked
        foreach (GameObject enemy in enemies)
        {
            TrackEnemy(enemy.transform);
        }
        #endregion
        #region Stop tracking enemies that no longer exist
        foreach (Transform t in new List<Transform>(trackedEnemies.Keys))
        {
            if (t == null) UntrackEnemy(t);
        }
        #endregion

        PlayerDot.anchoredPosition = WorldToMinimap(Player.position);

        foreach (var (enemyTransform, enemyDot) in trackedEnemies)
        {
            enemyDot.anchoredPosition = WorldToMinimap(enemyTransform.position);
            enemyDot.localRotation = Quaternion.Euler(0, 0, -enemyTransform.eulerAngles.y + 90);
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
