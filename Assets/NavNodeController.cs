using UnityEngine;
using System.Collections.Generic;
[ExecuteAlways]
public class NavNodeController : MonoBehaviour
{
    [SerializeField] List<GameObject> allNodes = new List<GameObject>();
    [SerializeField] float maxDistanceBetweenNodes;


    static bool CheckIfPositionsInRange(Vector3 pos1, Vector3 pos2, float distance)
    {
        return (pos1 - pos2).sqrMagnitude < distance * distance;
    }

    private void RefreshNodeList()
    {
        allNodes.Clear();
        foreach (Transform child in transform)
            allNodes.Add(child.gameObject);

    }

    private void Awake()
    {
        RefreshNodeList();
    }

    private void OnTransformChildrenChanged()
    {
        RefreshNodeList();
        Debug.Log("children changed");
    }

    public List<GameObject> GetConnectedNavNodes(Transform transform)
    {
        List<GameObject> connectedNavNodes = new List<GameObject>();

        foreach (GameObject node in allNodes)
        {
            if (CheckIfPositionsInRange(transform.position, node.transform.position, maxDistanceBetweenNodes) && node.transform != transform)
            {
                connectedNavNodes.Add(node);
            }
        }

        return connectedNavNodes;
    }



}
