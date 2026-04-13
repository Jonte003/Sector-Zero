using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;


public class NavPointManager : MonoBehaviour
{
    List<GameObject> allNodes;
    Vector3 nodeCheckLineOfSightOffset;
    List<GameObject> avaliableNodes;
    LayerMask obstacles;
    Transform playerTransform;

    [SerializeField] float height2;
    [SerializeField] float height3;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector3(-10, height2, -10), new Vector3(10, height2, 10));
        Gizmos.DrawLine(new Vector3(-10, height3, -10), new Vector3(10, height3, 10));

    }
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
        ChangeNavCosts();



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

    public Vector3 GetRandomAvaliableNode()
    {
        int numberOfAvaliableNodes = avaliableNodes.Count;

        int random = Random.Range(0, numberOfAvaliableNodes - 1);

        return avaliableNodes[random].transform.position;
    }

    void CheckAllNodes()
    {
        avaliableNodes.Clear();

        foreach (GameObject node in allNodes)
        {
            Collider[] hits = Physics.OverlapSphere(node.transform.position, 0.1f);

            if (hits.Length <= 0)
            {
                Vector3 targetPos = node.transform.position - nodeCheckLineOfSightOffset;
                Vector3 dir = (targetPos - playerTransform.position).normalized;

                Vector3 origin = playerTransform.position + dir * 0.1f;

                if (CheckIfLineOfSight(origin, targetPos, obstacles))
                {
                    avaliableNodes.Add(node);
                    node.GetComponent<DrawGizmoSphere>().ChangeColor(Color.green);
                    continue;
                }
            }

            node.GetComponent<DrawGizmoSphere>().ChangeColor(Color.red);
        }
    }


    void ChangeNavCosts()
    {
        float currentHeight = playerTransform.position.y;

        int areaHeight2 = NavMesh.GetAreaFromName("Height2");
        int areaHeight3 = NavMesh.GetAreaFromName("Height3");

        if (currentHeight >= height3)
        {
            NavMesh.SetAreaCost(areaHeight3, 1f);
            NavMesh.SetAreaCost(areaHeight2, 1f); 
            return;
        }

        if (currentHeight >= height2)
        {
            NavMesh.SetAreaCost(areaHeight3, 100);
            NavMesh.SetAreaCost(areaHeight2, 1f); 
            return;
        }

        NavMesh.SetAreaCost(areaHeight2, 100);
        NavMesh.SetAreaCost(areaHeight3, 100);
    }


}
