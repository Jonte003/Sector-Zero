using UnityEngine;

public class DroneNode : EnemyAI
{

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Update();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
