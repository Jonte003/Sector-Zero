using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";

    public void OnPlayClicked()
    {
        Debug.Log("Play button clicked. Scene not connected yet.");
    }

    public void OnSettingsClicked()
    {
        Debug.Log("Settings button clicked. Scene not connected yet.");
    }

    public void OnExitClicked()
    {
        Application.Quit();
        Debug.Log("Exit button clicked.");
    }
}
