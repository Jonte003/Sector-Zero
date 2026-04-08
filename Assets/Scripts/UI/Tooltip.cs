using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tooltipText;
    private RectTransform rectTransform;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();
    private PointerEventData pointerEventData;

    void Start()
    {
        tooltipText.text = "Tooltip";
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        rectTransform.position = new Vector3(mousePos.x, mousePos.y, 0);

        pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData .position = mousePos;

        raycastResults.Clear();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            TooltipTrigger trigger = result.gameObject.GetComponent<TooltipTrigger>();
            {
                if (trigger != null)
                {
                    tooltipText.text = trigger.tooltipText;
                    return;
                }
            }
        }
        tooltipText.text = "";
    }
    public void SetTooltipText(string text)
    {
        tooltipText.text = text;
    }
}
