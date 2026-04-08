using System.Collections;
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

    private bool buttonsInteractable = false;

    [SerializeField] private AudioClip sfxLevelUp;
    private AudioSource audioSource;

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

        buttonNewSkill.onClick.AddListener(OnNewSkillClicked);
        buttonLevelUpSkill.onClick.AddListener(OnLevelUpSkillClicked);
        buttonLevelUpStats.onClick.AddListener(OnLevelUpStatsClicked);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sfxLevelUp;
    }

    private void OnNewSkillClicked()
    {
        if (!buttonsInteractable) return;
        //Make button actually do something here
        playerLevels.ConfirmLevelUp();
    }
    private void OnLevelUpSkillClicked()
    {
        if (!buttonsInteractable) return;
        //Make button actually do something here
        playerLevels.ConfirmLevelUp();
    }
    private void OnLevelUpStatsClicked()
    {
        if (!buttonsInteractable) return;
        //Make button actually do something here
        playerLevels.ConfirmLevelUp();
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
            StartCoroutine(PlaySoundThenEnableButtons());
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

    private IEnumerator PlaySoundThenEnableButtons()
    {
        buttonsInteractable = false;
        audioSource.PlayOneShot(sfxLevelUp);
        yield return new WaitForSecondsRealtime(sfxLevelUp.length);
        buttonsInteractable = true;
    }
}
