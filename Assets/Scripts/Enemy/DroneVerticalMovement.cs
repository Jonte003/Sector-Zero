using UnityEngine;

public class DroneVerticalMovement : EnemyAI
{
    float force;
    [SerializeField] float distanceOverPlayer;
    [SerializeField] float distanceOverGround;
    [SerializeField] float forceUp;

    [SerializeField] float amplitude;
    [SerializeField] float frequency;
    float offset;

    float currentDistanceOverGround;
    float currentDistanceOverPlayer;

    float distanceRelativeToNavMesh;

    float sinOfTime;
    float yValueWithoutSin;

    protected override void Start()
    {
        base.Start();
        amplitude = amplitude * Random.Range(0.7f, 1.2f);
        frequency = frequency * Random.Range(0.7f, 1.2f);
        offset = Random.Range(0f, 4.6f) * amplitude;

        
    }
    protected override void Update()
    {
        base.Update();

        sinOfTime = Mathf.Sin(Time.time * frequency + offset) * amplitude;
        yValueWithoutSin = transform.position.y - sinOfTime;



        currentDistanceOverGround = GetDistanceToObstacle(Vector3.down);
        currentDistanceOverPlayer = GetDistanceOverPlayer();






        if (currentDistanceOverPlayer < distanceOverPlayer || currentDistanceOverGround < distanceOverGround)  //move up
        {
            MoveVertically(forceUp, Mathf.Abs(currentDistanceOverPlayer - distanceOverPlayer));
        }

        else if (currentDistanceOverPlayer > distanceOverPlayer + 1 && currentDistanceOverGround > distanceOverGround) //move down
        {
            MoveVertically(-forceUp, Mathf.Abs(currentDistanceOverPlayer - distanceOverPlayer));
        }



        transform.localPosition = new Vector3(0, distanceRelativeToNavMesh + sinOfTime, 0);

        //Vector3 rot = transform.eulerAngles;
        //rot.z = 0f; rot.x = 0f;
        //transform.eulerAngles = rot;


    }



    public float GetDistanceToObstacle(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, obstacles))
        {
            return hit.distance;
        }

        return 0;
    }


    public float GetDistanceOverPlayer()
    {
        return yValueWithoutSin - targetLocation.y;
    }




    private void MoveVertically(float force, float deltaToThreshold)
    {
        distanceRelativeToNavMesh += force * Time.deltaTime * deltaToThreshold;

    }

}
