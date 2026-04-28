using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] Color colorClosed;
    [SerializeField] Color colorOpen;
    bool isClosed = false;
    Renderer barrierRenderer;
    Collider barrierCollider;
    int blockCounter = 0;
    void Start()
    {
        barrierRenderer = GetComponent<Renderer>();
        barrierCollider = GetComponent<Collider>();
        UpdateState();
    }

    void Update()
    {
        blockCounter++;
        if (blockCounter > 100)
        {
            blockCounter = 0;
            isClosed = !isClosed;
        }
        UpdateState();
    }

    void UpdateState()
    {
        if (isClosed)
        {
            barrierRenderer.material.color = colorClosed;
            barrierCollider.enabled = true;
        }
        else
        {
            barrierRenderer.material.color = colorOpen;
            barrierCollider.enabled = false;
        }
    }
}
