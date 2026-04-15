using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Prefabs & Dependencies")]
    [SerializeField] private GridElement blockPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private ObjectSpawner objectSpawner;

    [Header("Generation Settings")]
    [SerializeField] private int mapSize;
    [Tooltip("Controls the frequency of slope generation. Higher values create more verticality.")]
    [Range(0, 1f)][SerializeField] private float hilliness = 0.5f;

    [Header("AI Navigation")]
    [SerializeField] private NavMeshSurface navMeshSurface;

    private const float blockSpacingHorizontal = 10f;
    private const float blockSpacingVertical = 6f;

    private GridElement[,] gridElements;
    private GridElement currentElement;
    private Vector2Int lockedDirection = Vector2Int.zero;

    private readonly Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    public void GenerateMap()
    {
        ClearMap();
        GenerateTerrain(Random.Range(0, mapSize), Random.Range(0, mapSize));
        BakeNavigation();

        Debug.Log("Generate Map");
    }

    public void ClearMap()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (Application.isPlaying)
                Destroy(child);
            else
                DestroyImmediate(child);
        }

        gridElements = null;
        currentElement = null;
        lockedDirection = Vector2Int.zero;

        Debug.Log("Clear Map");
    }

    public void BakeNavigation()
    {
        if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh();
        else
            Debug.LogWarning("NavMeshSurface not assigned on TerrainGenerator");

        Debug.Log("Baking Navigation");
    }

    public GridElement GetGridElement(int x, int z)
    {
        if (x < 0 || x >= mapSize || z < 0 || z >= mapSize) return null;
        return gridElements[x, z];
    }

    private void GenerateTerrain(int startX, int startZ)
    {
        gridElements = new GridElement[mapSize, mapSize];

        CreateElement(startX, 0, startZ);
        currentElement = gridElements[startX, startZ];

        while (currentElement != null)
            ExpandElement();

        CreateWalls();
        BakeNavigation();

        Debug.Log("Generating Terrain");
    }

    private void ExpandElement()
    {
        Vector2Int direction = lockedDirection != Vector2Int.zero ? lockedDirection : GetRandomDirectionFromElement(currentElement);

        if (direction == Vector2Int.zero)
        {
            currentElement = FirstNonSlopeElementWithAvailableSpace();
            if (currentElement == null) return;
            direction = GetRandomDirectionFromElement(currentElement);
        }

        currentElement = CreateElement(currentElement.Coordinates.x + direction.x, currentElement.Coordinates.y, currentElement.Coordinates.z + direction.y);

        if (CanRaiseElevationInDirection(currentElement, direction) && Random.value < (hilliness / 5f))
        {
            currentElement.SetElevation(currentElement.Coordinates.y + 1, (currentElement.Coordinates.y + 1) * blockSpacingVertical);
            currentElement.MakeSlope(direction);
            lockedDirection = direction;
        }
        else
        {
            lockedDirection = Vector2Int.zero;
            objectSpawner.SpawnRandomObject(currentElement);
        }

        currentElement.ConfigureBottomPart(blockSpacingVertical);
    }

    private GridElement FirstNonSlopeElementWithAvailableSpace()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (gridElements[i, j] == null || gridElements[i, j].IsSlope) continue;

                foreach (Vector2Int dir in directions)
                {
                    int newX = i + dir.x;
                    int newZ = j + dir.y;

                    if (newX >= 0 && newX < mapSize && newZ >= 0 && newZ < mapSize && gridElements[newX, newZ] == null)
                        return gridElements[i, j];
                }
            }
        }
        return null;
    }

    private bool CanRaiseElevationInDirection(GridElement element, Vector2Int direction)
    {
        Vector2Int target = new Vector2Int(element.Coordinates.x + direction.x, element.Coordinates.z + direction.y);

        if (target.x < 0 || target.x >= mapSize || target.y < 0 || target.y >= mapSize)
            return false;

        return gridElements[target.x, target.y] == null;
    }

    private GridElement CreateElement(int x, int y, int z)
    {
        Vector3 position = new Vector3(x * blockSpacingHorizontal, y * blockSpacingVertical, z * blockSpacingHorizontal);
        GridElement element = Instantiate(blockPrefab, position, Quaternion.identity, transform);

        element.Coordinates = new Vector3Int(x, y, z);
        gridElements[x, z] = element;

        return element;
    }

    private Vector2Int GetRandomDirectionFromElement(GridElement element)
    {
        List<Vector2Int> availableDirections = new();

        foreach (Vector2Int dir in directions)
        {
            int newX = element.Coordinates.x + dir.x;
            int newZ = element.Coordinates.z + dir.y;

            if (newX >= 0 && newX < mapSize && newZ >= 0 && newZ < mapSize && gridElements[newX, newZ] == null)
                availableDirections.Add(dir);
        }

        if (availableDirections.Count == 0) return Vector2Int.zero;

        return availableDirections[Random.Range(0, availableDirections.Count)];
    }

    private void CreateWalls()
    {
        float offset = (mapSize / 2f) * blockSpacingHorizontal - (mapSize % 2 == 0 ? blockSpacingHorizontal / 2f : 0);
        float length = mapSize * blockSpacingHorizontal;
        float height = 50 * blockSpacingVertical;
        float thickness = blockSpacingHorizontal;

        SpawnWallSegment(new Vector3(offset, 0, length), new Vector3(length, height, thickness));
        SpawnWallSegment(new Vector3(offset, 0, -thickness), new Vector3(length, height, thickness));
        SpawnWallSegment(new Vector3(length, 0, offset), new Vector3(thickness, height, length));
        SpawnWallSegment(new Vector3(-thickness, 0, offset), new Vector3(thickness, height, length));

        Debug.Log("Creating Walls");
    }

    private void SpawnWallSegment(Vector3 position, Vector3 scale)
    {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
        wall.transform.localScale = scale;

        Debug.Log("Spawning Walls");
    }
}