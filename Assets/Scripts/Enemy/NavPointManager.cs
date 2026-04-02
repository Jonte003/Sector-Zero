using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;


public class NavPointManager : MonoBehaviour
{
    List<GameObject> allNodes;
    Vector3 nodeCheckLineOfSightOffset;
    List<GameObject> avaliableNodes;
    LayerMask obstacles;
    Transform playerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allNodes = new List<GameObject>();
        avaliableNodes = new List<GameObject>();
        obstacles = LayerMask.GetMask("obstacle");
        playerTransform = GameObject.FindWithTag("Player").transform;
        foreach (Transform child in transform)
        {
            allNodes.Add(child.gameObject);
        }
        nodeCheckLineOfSightOffset = new Vector3(0, -1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckAllNodes();




    }

    public static bool CheckIfLineOfSight(Vector3 from, Vector3 target, LayerMask layerMask)
    {
        Vector3 direction = target - from;
        float distance = direction.magnitude;

        return !Physics.Raycast(from, direction.normalized, distance, layerMask);
    }

    public Vector3 GetClosestNode(Vector3 pos)
    {
        Vector3 closestNode = Vector3.zero;
        float distance = Mathf.Infinity;
        foreach (GameObject node in avaliableNodes)
        {

            Vector3 toNode = node.transform.position - pos;
            float dis = Vector3.SqrMagnitude(toNode);
            if (dis < distance)
                
            {
                closestNode = node.transform.position;
                distance = dis;
            }
        }
        return closestNode;
    }

    void CheckAllNodes()
    {
        avaliableNodes.Clear();

        foreach (GameObject node in allNodes)
        {
            Collider[] hits = Physics.OverlapSphere(node.transform.position, 0.1f); //Check if node is inside collider

            if (hits.Length <= 0 && CheckIfLineOfSight(node.transform.position - nodeCheckLineOfSightOffset, playerTransform.position, obstacles))
            {
                avaliableNodes.Add(node);
                node.GetComponent<DrawGizmoSphere>().ChangeColor(Color.green);
            }
            else
            {
                node.GetComponent<DrawGizmoSphere>().ChangeColor(Color.red);
            }

        }
    }


}
