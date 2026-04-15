using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    [Header("Visual Components")]
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject slope;

    [Header("Foundation Generation")]
    [Tooltip("Parent transform for the vertical filler blocks generated below this element.")]
    [SerializeField] private GameObject bottomPart;
    [SerializeField] private GameObject bottomPartBlockPrefab;

    private static readonly Dictionary<Vector2Int, float> directionToYaw = new()
    {
        { Vector2Int.right, 90f  },
        { Vector2Int.left,  270f },
        { Vector2Int.up,    0f   },
        { Vector2Int.down,  180f }
    };

    public Vector3Int Coordinates { get; set; }
    public bool IsSlope => slope.activeSelf;

    public void SetElevation(int elevation, float yPosition)
    {
        Coordinates = new Vector3Int(Coordinates.x, elevation, Coordinates.z);
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(currentPosition.x, yPosition, currentPosition.z);
    }

    public void MakeSlope(Vector2Int direction)
    {
        cube.SetActive(false);
        slope.SetActive(true);

        if (directionToYaw.TryGetValue(direction, out float yaw))
        {
            slope.transform.localEulerAngles = new Vector3(0f, yaw, 0f);
        }
    }

    public void ConfigureBottomPart(float blockSpacingVertical)
    {
        if (Coordinates.y <= 0) return;

        // midpoint
        float centerOffsetMultiplier = (Coordinates.y + 1) / 2f;
        Vector3 spawnPosition = transform.position + (Vector3.down * centerOffsetMultiplier * blockSpacingVertical);

        // Instanciate
        GameObject filler = Instantiate(bottomPartBlockPrefab, spawnPosition, Quaternion.identity, bottomPart.transform);

        // stretch Y-axis
        Vector3 newScale = filler.transform.localScale;
        newScale.y *= Coordinates.y;
        filler.transform.localScale = newScale;

        // Vertical tiling
        Renderer fillerRenderer = filler.GetComponentInChildren<Renderer>();
        if (fillerRenderer != null)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

            fillerRenderer.GetPropertyBlock(propBlock);

            // (Tiling X, Tiling Y, Offset X, Offset Y)
            Vector4 tilingAndOffset = new Vector4(1, Coordinates.y, 0, 0);

            propBlock.SetVector("_BaseMap_ST", tilingAndOffset);

            fillerRenderer.SetPropertyBlock(propBlock);
        }
    }
}