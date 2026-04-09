using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Prop Pools")]
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private GameObject[] stonePrefabs;
    [SerializeField] private GameObject[] logPrefabs;

    public void SpawnRandomObject(GridElement gridElement)
    {
        float roll = Random.value;

        if (roll > 0.75f)
            SpawnTree(gridElement);
        else if (roll > 0.50f)
            SpawnStone(gridElement);
        else if (roll > 0.40f)
            SpawnLog(gridElement);
    }

    private GameObject SpawnProp(GridElement parentElement, GameObject[] prefabs, float localScale, Vector3 localPosition, Vector3 localEulerAngles)
    {
        if (prefabs == null || prefabs.Length == 0) return null;

        GameObject selectedPrefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject spawnedProp = Instantiate(selectedPrefab, parentElement.transform.position, Quaternion.identity, parentElement.transform);

        spawnedProp.transform.localScale = Vector3.one * localScale;
        spawnedProp.transform.localPosition = localPosition;
        spawnedProp.transform.localEulerAngles = localEulerAngles;

        return spawnedProp;
    }

    private void SpawnTree(GridElement gridElement)
    {
        Vector3 offset = new Vector3(Random.Range(-0.25f, 0.25f), 0.25f, Random.Range(-0.25f, 0.25f));
        Vector3 rotation = new Vector3(90f, Random.Range(0f, 360f), 0f);
        SpawnProp(gridElement, treePrefabs, 0.25f, offset, rotation);
    }

    private void SpawnStone(GridElement gridElement)
    {
        Vector3 offset = new Vector3(Random.Range(-0.25f, 0.25f), 0.25f, Random.Range(-0.25f, 0.25f));
        Vector3 rotation = new Vector3(0f, Random.Range(0f, 360f), 0f);
        SpawnProp(gridElement, stonePrefabs, Random.Range(0.25f, 0.5f), offset, rotation);
    }

    private void SpawnLog(GridElement gridElement)
    {
        Vector3 offset = new Vector3(Random.Range(-0.25f, 0.25f), 0.32f, Random.Range(-0.25f, 0.25f));
        Vector3 rotation = new Vector3(0f, Random.Range(0f, 360f), 0f);
        SpawnProp(gridElement, logPrefabs, Random.Range(0.1f, 0.25f), offset, rotation);
    }
}