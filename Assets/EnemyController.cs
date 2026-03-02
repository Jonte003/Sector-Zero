using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class EnemyController : MonoBehaviour
{
    List<GameObject> allChildren = new List<GameObject>();
    int counter;
    [SerializeField] GameObject navNodes;
    List<GameObject> allNavNodes = new List<GameObject>();

    private void Start()
    {
        foreach(Transform child in transform)
        {
            allChildren.Add(child.gameObject);
        }
        foreach(Transform child in navNodes.transform)
        {
            allNavNodes.Add(child.gameObject);
        }
    }

    public void SetDestinationToAllChilds(Vector3 desination, float inRange)
    {
        Debug.Log("SetDestionationInEnemyController Called");
        foreach(GameObject child in allChildren)
        {
            
            EnemyAi movementScript = child.GetComponent<EnemyAi>();
            movementScript.SetDestinationFromOutside(desination, inRange); 
        }
    }
    public Vector3 GetRandomNavNodePos()
    {
        return allNavNodes[Random.Range(0, allNavNodes.Count)].transform.position;
    }
    //public Vector3 GetRandomNavNodePosForwards(Vector3 rotation, Vector3 position, float maxAngle)
    //{
    //    int counter = 0;
    //    while(counter < 10)
    //    {
    //        Vector3 navNodePos = allNavNodes[Random.Range(0, allNavNodes.Count)].transform.position;
    //        Vector3 vectorToNode = navNodePos - position;
    //        if (Vector3.Angle(rotation, vectorToNode) <= maxAngle)
    //        {
    //            return navNodePos;
    //        }
    //        counter++;
    //    }
    //    return allNavNodes[Random.Range(0, allNavNodes.Count)].transform.position;
    //}
    public Vector3 GetRandomNavNodePosForwards(Vector3 rotation, Vector3 position, float maxAngle) 
    {
        List<Vector3> allNodesForward = new List<Vector3>();

        foreach (GameObject node in allNavNodes)
        {
            Vector3 vectorToNode = node.transform.position - position;
            if (Vector3.Angle(rotation, vectorToNode) <= maxAngle)
            {
                allNodesForward.Add(node.transform.position);
            }
        }

        if (allNodesForward.Count > 0)
            return allNodesForward[Random.Range(0, allNodesForward.Count)];
        else 
            return allNavNodes[Random.Range(0, allNavNodes.Count)].transform.position;
    }
    void Update()
    {



    }
}
