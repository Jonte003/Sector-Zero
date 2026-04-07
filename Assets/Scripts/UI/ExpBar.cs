using TMPro;
using UnityEngine;

public class ExpBar : MonoBehaviour
{
    private PlayerLevels playerLevels;

    [SerializeField] Transform bgBar;
    [SerializeField] Transform fgBar;
    [SerializeField] TextMeshProUGUI expTextNumber;
    [SerializeField] TextMeshProUGUI expTextLevel;
    void Start()
    {
        playerLevels = GameObject.FindWithTag("Player").GetComponent<PlayerLevels>();
    }

    void Update()
    {
        float fill = playerLevels.Experience / playerLevels.NextLevelExperience;

        RectTransform fgRec = fgBar.GetComponent<RectTransform>();
        RectTransform bgRec = bgBar.GetComponent<RectTransform>();

        if (fill > 1) fill = 1;
        fgRec.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bgRec.rect.width * fill);

        expTextNumber.text = Mathf.Ceil(playerLevels.Experience).ToString() + " / " + Mathf.Ceil(playerLevels.NextLevelExperience).ToString();
        expTextLevel.text = playerLevels.Level.ToString();
    }
}