using UnityEngine;
using System;

public class BulletTracer : MonoBehaviour
{
    private Action<BulletTracer> onFinished;

    [SerializeField, Tooltip("Units per second")] private float speed = 300;

    private Vector3 end;
    private bool active;

    public void Init(Vector3 start, Vector3 end, Action<BulletTracer> onFinished)
    {
        this.end = end;
        this.onFinished = onFinished;
        transform.position = start;
        active = true;
        GetComponent<TrailRenderer>().enabled = true;
    }

    private void Update()
    {
        if (!active) return;

        Vector3 newPos = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
        transform.position = newPos;

        if (newPos == end)
        {
            active = false;
            gameObject.SetActive(false);
            onFinished?.Invoke(this);
        }
    }
}