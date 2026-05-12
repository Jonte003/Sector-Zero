using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ChainLightningScript : MonoBehaviour
{
    public float timeToDespawn = 3;

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

    public static GameObject Spawn(GameObject lightning)
    {
        return Instantiate(lightning);
    }
}