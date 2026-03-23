using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    [SerializeField, Tooltip("Units per second")] private float speed = 300f;

    private Vector3 end;
    private bool active;

    public void Init(Vector3 start, Vector3 end)
    {
        this.end = end;
        transform.position = start;
        active = true;
    }

    private void Update()
    {
        if (!active) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            end,
            speed * Time.deltaTime
        );

        if ((transform.position - end).sqrMagnitude < 0.01f)
        {
            active = false;
            gameObject.SetActive(false);
        }
    }
}