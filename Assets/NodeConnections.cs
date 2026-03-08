using UnityEngine;
using System.Collections.Generic;

public class NodeConnections : MonoBehaviour
{
    [SerializeField] List<GameObject> connectedNodes;
    [SerializeField] GameObject ObjectWithAllNodes;

    [SerializeField] List<GameObject> manuallyConnectedNodes = new List<GameObject>();
    [SerializeField] List<GameObject> manuallyDisconectedNodes = new List<GameObject>();

    private NavNodeController navNodeController;

    private void Awake()
    {
        navNodeController = ObjectWithAllNodes.GetComponent<NavNodeController>();
        connectedNodes = navNodeController.GetConnectedNavNodes(transform);

        foreach (GameObject node in manuallyConnectedNodes)
        {
            connectedNodes.Add(node);
        }
        foreach (GameObject node in manuallyDisconectedNodes)
        {
            connectedNodes.Remove(node);
        }
    }



    private void OnDrawGizmos()
    {
        if (navNodeController == null)
            navNodeController = ObjectWithAllNodes.GetComponent<NavNodeController>();

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.8f);

        var connectedNodes = navNodeController.GetConnectedNavNodes(transform);

        foreach (GameObject node in manuallyConnectedNodes)
        {
            connectedNodes.Add(node);
        }
        foreach (GameObject node in manuallyDisconectedNodes)
        {
            connectedNodes.Remove(node);
        }

        foreach (GameObject node in connectedNodes)
        {
            Vector3 midpoint = (transform.position + node.transform.position) * 0.5f;
            Gizmos.DrawLine(transform.position, midpoint);

        }
    }

    public GameObject GetRandomConnectedNode(GameObject previousNode)
    {
        while (true)
        {
            GameObject node = connectedNodes[Random.Range(0, connectedNodes.Count)];
            if (node != previousNode)
                return node;
        }
    }


}
