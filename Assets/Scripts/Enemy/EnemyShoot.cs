using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShoot : EnemyAI
{
    [SerializeField] int tracerSize;
    Vector3 toPlayer;
    [SerializeField] Vector2 spreadFrom;
    [SerializeField] Vector2 spreadTo;
    [SerializeField] float range;

    float shootDelay = 0.4f;
    
    Animator droneAnimator;
    DroneStats stats;
    private Queue<BulletTracer> TracerPool;

    [SerializeField] GameObject tracerPrefab;

    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        droneAnimator = GetComponent<Animator>();
        stats = GetComponent<DroneStats>();

        TracerPool = new Queue<BulletTracer>();

        for (int i = 0; i < tracerSize; i++)
        {
            var t = Instantiate(tracerPrefab);
            t.gameObject.SetActive(false);
            TracerPool.Enqueue(t.GetComponent<BulletTracer>());
        }

        Debug.Log("tracers in queue: " + TracerPool.Count);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        transform.LookAt(targetLocation);
        toPlayer = targetLocation - transform.position;


        if (CheckIfLineOfSight(targetLocation, transform.position, obstacles) && CheckIfPositionsInRange(transform.position, targetLocation, range));
        {

            if (timer <= 0)
            {
                timer = stats.ReloadRate;
                //Shoot();

                droneAnimator.SetTrigger("Shoot");

            }
            else
            {
                timer -= Time.deltaTime;
            }


        }




    }
    public void ShootWithDelay()
    {
        StartCoroutine(DelayedShoot(shootDelay));
    }

    private IEnumerator DelayedShoot(float delay)
    {
        yield return new WaitForSeconds(delay);
        Shoot();
    }

    public void Shoot()
    {
        Vector3 end = SimulateShot();

        Vector3 start = transform.position;

        SpawnTracer(start, end);
    }


    private void SpawnTracer(Vector3 start, Vector3 end)
    {
        BulletTracer tracer = TracerPool.Dequeue();

        tracer.GetComponent<TrailRenderer>().enabled = false;

        tracer.gameObject.SetActive(true);

        Vector3 dir = (end - start).normalized;
        tracer.transform.rotation = Quaternion.LookRotation(dir);

        tracer.Init(start, end, ReturnTracerToPool);
    }

    private Vector3 SimulateShot()
    {

        float spreadX = Random.Range(spreadFrom.x, spreadTo.x);
        float spreadY = Random.Range(spreadFrom.y, spreadTo.y);

        Quaternion spreadRot = Quaternion.AngleAxis(spreadX, transform.up) * Quaternion.AngleAxis(spreadY, transform.right);
        
        Vector3 direction = (spreadRot * toPlayer).normalized;
         

        Vector3 targetPoint;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, range))
        {
            targetPoint = hit.point;

            if (hit.transform.CompareTag("Player"))
            {
                stats.DoDamageToTarget();                
            }
        }
        else
        {
            targetPoint = transform.position + direction * range;
        }

        return targetPoint;
    }

    private void ReturnTracerToPool(BulletTracer tracer)
    {
        TracerPool.Enqueue(tracer);
    }


}
