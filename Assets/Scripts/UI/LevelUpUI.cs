using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    private PlayerLevels playerLevels;
    private bool visible = false;
    private GameObject levelUpUIPanel;
    private Crosshair crosshair;

    [SerializeField] private Button buttonNewSkill;
    [SerializeField] private Button buttonLevelUpSkill;
    [SerializeField] private Button buttonLevelUpStats;

    private void setPanelVisible(bool visible)
    {
        levelUpUIPanel.SetActive(visible);
    }

    private void PauseTime()
    {
        Time.timeScale = 0f;
    }

    private void UnpauseTime()
    {
        Time.timeScale = 1f;
    }
    void Start()
    {
        playerLevels = GameObject.FindWithTag("Player").GetComponent<PlayerLevels>();
        levelUpUIPanel = GameObject.Find("Level Up Popup");
        crosshair = GameObject.Find("Crosshair").GetComponent<Crosshair>();
        setPanelVisible(visible);
    }

    void Update()
    {
        if (playerLevels.PendingLevelUp && !visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            crosshair.Hide();
            visible = true;
            setPanelVisible(visible);
            PauseTime();
        }
        else if (!playerLevels.PendingLevelUp && visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            crosshair.Show();
            visible = false;
            setPanelVisible(visible);
            UnpauseTime();
        }
    }
}
