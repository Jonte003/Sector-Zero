using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private float transparency = 1.0f;
    
    public void Show()
    {
        transparency = 1.0f;
        updateTransparency();
    }
    public void Hide()
    {
        transparency = 0.0f;
        updateTransparency();
    }

    private void updateTransparency()
    {
        gameObject.GetComponent<RawImage>().color = new Color(1f, 1f, 1f, transparency);
    }
}
