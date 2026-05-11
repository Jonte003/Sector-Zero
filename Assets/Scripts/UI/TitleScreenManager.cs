using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] Canvas titleScreenCanvas;
    [SerializeField] Canvas lobbyCanvas;
    [SerializeField] PlayerMovement playerMovement;
    void Start()
    {
        titleScreenCanvas.gameObject.SetActive(true);
        lobbyCanvas.gameObject.SetActive(false);
        Pause.IsPaused = true;
        playerMovement.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnClickPlay()
    {
        titleScreenCanvas.gameObject.SetActive(false);
        lobbyCanvas.gameObject.SetActive(true);
        Pause.IsPaused = false;
        playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
