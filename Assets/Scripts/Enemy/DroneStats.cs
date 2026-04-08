using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class DroneStats : EnemyStats
{
    [Space]
    [SerializeField] float damagePerShoot;
    [SerializeField] float reloadRate;
    



    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }



    public void DoDamageToTarget()
    {
        playerStats.DoDamageFixed(damagePerShoot);
    }

    public float ReloadRate
    {
        get { return reloadRate; }
    }


}
