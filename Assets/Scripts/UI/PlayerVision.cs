using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    [Header("Fog Settings")]
    public float minRange;
    public float maxRange;
    public float minStatValue;
    public float maxStatValue;
    public float falloffDistance;

    public void UpdateVisionRange(float statValue)
    {
        float t = Mathf.InverseLerp(minStatValue, maxStatValue, statValue);
        float range = Mathf.Lerp(minRange, maxRange, t);

        RenderSettings.fogEndDistance = range;
        RenderSettings.fogStartDistance = range - falloffDistance;
    }
}
