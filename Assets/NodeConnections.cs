using UnityEngine;
using System.Collections.Generic;

public class NodeConnections : MonoBehaviour
{
    [SerializeField] List<GameObject> connectedNodes;
    [SerializeField] GameObject ObjectWithAllNodes;

    private NavNodeController navNodeController;

    private void Awake()
    {
        navNodeController = ObjectWithAllNodes.GetComponent<NavNodeController>();
        connectedNodes = navNodeController.GetConnectedNavNodes(transform);

    }


    private void OnDrawGizmos()
    {
        if (navNodeController == null)
            navNodeController = ObjectWithAllNodes.GetComponent<NavNodeController>();

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.8f);

        var connectedNodes = navNodeController.GetConnectedNavNodes(transform);

        foreach (GameObject node in connectedNodes)
            Gizmos.DrawLine(transform.position, node.transform.position);
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
