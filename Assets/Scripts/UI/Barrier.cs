using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] Color colorClosed;
    [SerializeField] Color colorOpen;
    bool isClosed = true;
    Renderer barrierRenderer;
    Collider barrierCollider;
    void Start()
    {
        barrierRenderer = GetComponent<Renderer>();
        barrierCollider = GetComponent<Collider>();
        UpdateState();
    }

    void Update()
    {
        if (LoadoutManager.Settings == null) return;
        if (LoadoutManager.Settings.GunName != "None")
        {
            isClosed = false;
        }
        else
        {
            isClosed = true;
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
