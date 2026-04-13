using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnPlayClicked()
    {
        SceneManager.LoadScene("Loadout Selection");
    }

    public void OnSettingsClicked()
    {
        Debug.Log("Settings button clicked. Scene not connected yet.");
    }

    public void OnCreditsClicked()
    {
        Debug.Log("Credits button clicked. Scene not connected yet.");
    }
    public void OnExitClicked()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
