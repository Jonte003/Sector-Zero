using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    private float timeElapsed = 0;

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        int min = Mathf.FloorToInt(timeElapsed / 60);
        int sec = Mathf.FloorToInt((timeElapsed) % 60);

        timerText.text = string.Format("{0:D2}:{1:D2}", min, sec);
    }
}