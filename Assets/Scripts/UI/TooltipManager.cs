using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [SerializeField] GameObject tooltipPanel;
    [SerializeField] TextMeshProUGUI tooltipHeader;
    [SerializeField] TextMeshProUGUI tooltipBody;
    void Awake()
    {
       Instance = this;
       ClearTooltip();
    }

    public void ShowTooltip(string header, string body)
    {
        tooltipHeader.text = header;
        tooltipBody.text = body;
        tooltipPanel.SetActive(true);
    }

    public void ClearTooltip()
    {
        tooltipHeader.text = "";
        tooltipBody.text = "";
        tooltipPanel.SetActive(false);
    }
}
