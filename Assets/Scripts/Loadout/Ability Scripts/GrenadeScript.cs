using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    public float timeToExplode = 4;
    public float radius = 5;
    public float damage = 10;
    public List<Transform> enemies;

    private float timer;

    void Start()
    {
        timer = timeToExplode;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (math.abs((enemies[i].position - transform.position).magnitude) <= radius)
            {
                enemies[i].GetComponent<EnemyStats>().DoDamageToEnemy(damage);
            }
        }

        Destroy(this);
    }

    public static void Spawn(GameObject grenade, float radius, float damage, float throwForce, Vector3 direction)
    {
        GameObject ob = Instantiate(grenade);

        ob.GetComponent<GrenadeScript>().radius = radius;
        ob.GetComponent<GrenadeScript>().damage = damage;

        ob.GetComponent<Rigidbody>().AddForce(direction * throwForce);
    }
}