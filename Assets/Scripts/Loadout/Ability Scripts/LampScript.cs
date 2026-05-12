using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LampScript : MonoBehaviour
{
    public float timeToDespawn = 5;
    public float radius = 5;

    private float timer;

    void Start()
    {
        timer = timeToDespawn;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Destroy(this);
        }
    }

    public static void Spawn(GameObject lamp, float radius, float throwForce, Vector3 direction)
    {
        GameObject ob = Instantiate(lamp);

        ob.GetComponent<LampScript>().radius = radius;

        ob.GetComponent<Rigidbody>().AddForce(direction * throwForce);
    }
}