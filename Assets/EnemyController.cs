using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class EnemyController : MonoBehaviour
{
    List<GameObject> allChildren = new List<GameObject>();
    

    private void Start()
    {
        foreach(Transform child in transform)
        {
            allChildren.Add(child.gameObject);
        }
    }

    public void SetDestinationToAllChilds(Vector3 desination, float inRange)
    {
        Debug.Log("SetDestionationInEnemyController Called");
        foreach(GameObject child in allChildren)
        {

            MovementScript movementScript = child.GetComponent<MovementScript>();
            movementScript.SetDestinationFromOutside(desination, inRange);
        }
    }
}
