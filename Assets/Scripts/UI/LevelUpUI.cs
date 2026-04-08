using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    private PlayerLevels playerLevels;
    private bool visible = false;
    private GameObject levelUpUIPanel;
    private GameObject newSkillPanel;
    private GameObject levelUpSkillPanel;
    private GameObject statsPanel;
    private Crosshair crosshair;

    [SerializeField] private Button buttonNewSkill;
    [SerializeField] private Button buttonLevelUpSkill;
    [SerializeField] private Button buttonLevelUpStats;

    [SerializeField] private Button buttonNewSkill1;
    [SerializeField] private Button buttonNewSkill2;
    [SerializeField] private Button buttonNewSkill3;

    [SerializeField] private Button buttonLevelUpSkill1;
    [SerializeField] private Button buttonLevelUpSkill2;
    [SerializeField] private Button buttonLevelUpSkill3;

    [SerializeField] private Button buttonLevelUpStats1;
    [SerializeField] private Button buttonLevelUpStats2;
    [SerializeField] private Button buttonLevelUpStats3;

    private bool buttonsInteractable = false;
    private bool firstChoiceMade = false;

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
        newSkillPanel = GameObject.Find("New Skill Popup");
        levelUpSkillPanel = GameObject.Find("Level Up Skill Popup");
        statsPanel = GameObject.Find("Stats Popup");

        newSkillPanel.SetActive(false);
        levelUpSkillPanel.SetActive(false);
        statsPanel.SetActive(false);

        crosshair = GameObject.Find("Crosshair").GetComponent<Crosshair>();
        setPanelVisible(visible);

        buttonNewSkill.onClick.AddListener(OnNewSkillClicked);
        buttonLevelUpSkill.onClick.AddListener(OnLevelUpSkillClicked);
        buttonLevelUpStats.onClick.AddListener(OnLevelUpStatsClicked);

        buttonNewSkill1.onClick.AddListener(OnNewSkill1Clicked);
        buttonNewSkill2.onClick.AddListener(OnNewSkill2Clicked);
        buttonNewSkill3.onClick.AddListener(OnNewSkill3Clicked);

        buttonLevelUpSkill1.onClick.AddListener(OnLevelUpSkill1Clicked);
        buttonLevelUpSkill2.onClick.AddListener(OnLevelUpSkill2Clicked);
        buttonLevelUpSkill3.onClick.AddListener(OnLevelUpSkill3Clicked);

        buttonLevelUpStats1.onClick.AddListener(OnLevelUpStats1Clicked);
        buttonLevelUpStats2.onClick.AddListener(OnLevelUpStats2Clicked);
        buttonLevelUpStats3.onClick.AddListener(OnLevelUpStats3Clicked);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sfxLevelUp;
    }

    private void OnNewSkillClicked()
    {
        if (!buttonsInteractable || firstChoiceMade) return;
        newSkillPanel.SetActive(true);
        firstChoiceMade = true;
    }
    private void OnLevelUpSkillClicked()
    {
        if (!buttonsInteractable || firstChoiceMade) return;
        levelUpSkillPanel.SetActive(true);
        firstChoiceMade = true;
    }
    private void OnLevelUpStatsClicked()
    {
        if (!buttonsInteractable || firstChoiceMade) return;
        statsPanel.SetActive(true);
        firstChoiceMade = true;
    }

    private void OnNewSkill1Clicked()
    {
        // Implement logic for choosing the first new skill
        newSkillPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnNewSkill2Clicked()
    {
        // Implement logic for choosing the second new skill
        newSkillPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnNewSkill3Clicked()
    {
        // Implement logic for choosing the third new skill
        newSkillPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnLevelUpSkill1Clicked()
    {
        // Implement logic for leveling up the first skill
        levelUpSkillPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnLevelUpSkill2Clicked()
    {
        // Implement logic for leveling up the second skill
        levelUpSkillPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnLevelUpSkill3Clicked()
    {
        // Implement logic for leveling up the third skill
        levelUpSkillPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnLevelUpStats1Clicked()
    {
        // Implement logic for leveling up the first stat
        statsPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnLevelUpStats2Clicked()
    {
        // Implement logic for leveling up the second stat
        statsPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnLevelUpStats3Clicked()
    {
        // Implement logic for leveling up the third stat
        statsPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }
    void Update()
    {
        if (playerLevels.PendingLevelUp && !visible)
        {
            firstChoiceMade = false;
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
