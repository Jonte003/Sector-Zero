using UnityEngine;

public class DrawGizmoSphere : MonoBehaviour
{

    Color gizmosColor = Color.blue;
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    public void ChangeColor(Color color)
    {
        gizmosColor = color;
    }
}
