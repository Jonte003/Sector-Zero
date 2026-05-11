using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float timeRemaining { get; set; }

    // Update is called once per frame


    void Update()
    {
        timeRemaining += Time.deltaTime;

        int min = Mathf.FloorToInt(timeRemaining / 60);
        int sec = Mathf.FloorToInt((timeRemaining) % 60);

        timerText.text = string.Format("{0:D2}:{1:D2}", min, sec);
    }
}