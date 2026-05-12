using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] RarityColors rarityColors;

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

    [SerializeField] private GameObject buttonNewSkillBorders;
    [SerializeField] private GameObject buttonLevelUpSkillBorders;
    [SerializeField] private GameObject buttonLevelUpStatsBorders;

    [SerializeField] private Image imageAbilityBar1;
    [SerializeField] private Image imageAbilityBar2;
    [SerializeField] private Image imageAbilityBar3;

    [SerializeField] private Image imageAbilityBar1Overlay;
    [SerializeField] private Image imageAbilityBar2Overlay;
    [SerializeField] private Image imageAbilityBar3Overlay;

    private bool buttonsInteractable = false;
    private bool firstChoiceMade = false;

    [SerializeField] private Sprite iconStatHealth;
    [SerializeField] private Sprite iconStatRegen;
    [SerializeField] private Sprite iconStatDamage;
    [SerializeField] private Sprite iconStatAbilityHaste;
    [SerializeField] private Sprite iconStatDefense;
    [SerializeField] private Sprite iconStatJumpHeight;
    [SerializeField] private Sprite iconStatMovementSpeed;

    [SerializeField] private AudioClip sfxLevelUp;
    private AudioSource audioSource;

    GameObject player;

    private Stat[] statChoices;
    private Ability[] abilityChoices;

    private int abilitySlot;

    private void setPanelVisible(bool visible)
    {
        levelUpUIPanel.SetActive(visible);
    }

    private void PauseTime()
    {
        Time.timeScale = 0f;
        Pause.IsPaused = true;
    }

    private void UnpauseTime()
    {
        Time.timeScale = 1f;
        Pause.IsPaused = false;
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerLevels = player.GetComponent<PlayerLevels>();
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

        if (player.GetComponent<Loadout>().Ability1 == null)
        {
            abilitySlot = 1;
        }
        else if (player.GetComponent<Loadout>().Ability2 == null)
        {
            abilitySlot = 2;
        }
        else if (player.GetComponent<Loadout>().Ability3 == null)
        {
            abilitySlot = 3;
        }
        else
        {
            return;
        }

        abilityChoices = player.GetComponent<Loadout>().GetRandomAbilities(3);

        newSkillPanel.transform.Find("Choices").Find("Button New Skill 1").GetComponent<TooltipTrigger>().tooltipText = $"{abilityChoices[0].Name}";
        newSkillPanel.transform.Find("Choices").Find("Button New Skill 2").GetComponent<TooltipTrigger>().tooltipText = $"{abilityChoices[1].Name}";
        newSkillPanel.transform.Find("Choices").Find("Button New Skill 3").GetComponent<TooltipTrigger>().tooltipText = $"{abilityChoices[2].Name}";

        Image[] buttonImages =
        {
            buttonNewSkill1.GetComponent<Image>(),
            buttonNewSkill2.GetComponent<Image>(),
            buttonNewSkill3.GetComponent<Image>()
        };

        for (int i = 0; i < 3; i++)
        {
            buttonImages[i].sprite = abilityChoices[i].Icon;
        }

        newSkillPanel.SetActive(true);
        firstChoiceMade = true;
    }
    private void OnLevelUpSkillClicked()
    {
        if (!buttonsInteractable || firstChoiceMade) return;

        Loadout playerLoadout = player.GetComponent<Loadout>();

        if ((playerLoadout.Ability1 == null || playerLoadout.Ability1.Level == 5) && (playerLoadout.Ability2 == null || playerLoadout.Ability2.Level == 5) && (playerLoadout.Ability3 == null || playerLoadout.Ability3.Level == 5))
        {
            return;
        }

        levelUpSkillPanel.transform.Find("Choices").Find("Button Level Up Skill 1").GetComponent<TooltipTrigger>().tooltipText = playerLoadout.Ability1 != null ? $"{playerLoadout.Ability1.Name} (Level {playerLoadout.Ability1.Level}/5)" : "Empty Slot";
        levelUpSkillPanel.transform.Find("Choices").Find("Button Level Up Skill 2").GetComponent<TooltipTrigger>().tooltipText = playerLoadout.Ability2 != null ? $"{playerLoadout.Ability2.Name} (Level {playerLoadout.Ability2.Level}/5)" : "Empty Slot";
        levelUpSkillPanel.transform.Find("Choices").Find("Button Level Up Skill 3").GetComponent<TooltipTrigger>().tooltipText = playerLoadout.Ability3 != null ? $"{playerLoadout.Ability3.Name} (Level {playerLoadout.Ability3.Level}/5)" : "Empty Slot";

        Image[] buttonImages =
        {
            buttonLevelUpSkill1.GetComponent<Image>(),
            buttonLevelUpSkill2.GetComponent<Image>(),
            buttonLevelUpSkill3.GetComponent<Image>()
        };

        for (int i = 0; i < 3; i++)
        {
            if (i == 0 && playerLoadout.Ability1 != null) buttonImages[i].sprite = playerLoadout.Ability1.Icon;
            if (i == 1 && playerLoadout.Ability2 != null) buttonImages[i].sprite = playerLoadout.Ability2.Icon;
            if (i == 2 && playerLoadout.Ability3 != null) buttonImages[i].sprite = playerLoadout.Ability3.Icon;
        }

        levelUpSkillPanel.SetActive(true);
        firstChoiceMade = true;
    }
    private void OnLevelUpStatsClicked()
    {
        if (!buttonsInteractable || firstChoiceMade) return;

        statChoices = Stats.GetRandomStats(3);

        statsPanel.transform.Find("Choices").Find("Button Stats 1").GetComponent<TooltipTrigger>().tooltipText = $"{statChoices[0].StatType} +{statChoices[0].Value}";
        statsPanel.transform.Find("Choices").Find("Button Stats 2").GetComponent<TooltipTrigger>().tooltipText = $"{statChoices[1].StatType} +{statChoices[1].Value}";
        statsPanel.transform.Find("Choices").Find("Button Stats 3").GetComponent<TooltipTrigger>().tooltipText = $"{statChoices[2].StatType} +{statChoices[2].Value}";

        Image[] buttonImages =
        {
            buttonLevelUpStats1.GetComponent<Image>(),
            buttonLevelUpStats2.GetComponent<Image>(),
            buttonLevelUpStats3.GetComponent<Image>()
        };

        for (int i = 0; i < 3; i++)
        {
            buttonImages[i].sprite = GetStatIcon(statChoices[i]);
            //buttonImages[i].color = Color.red;
            buttonImages[i].color = rarityColors.GetColor(statChoices[i].Rarity);
        }

        statsPanel.SetActive(true);
        firstChoiceMade = true;
    }

    private Sprite GetStatIcon(Stat stat)
    {
        return stat.StatType switch
        {
            PossibleLevelUpStats.Hp => iconStatHealth,
            PossibleLevelUpStats.Regen => iconStatRegen,
            PossibleLevelUpStats.Damage => iconStatDamage,
            PossibleLevelUpStats.AbilityHaste => iconStatAbilityHaste,
            PossibleLevelUpStats.Defense => iconStatDefense,
            PossibleLevelUpStats.JumpHeight => iconStatJumpHeight,
            PossibleLevelUpStats.MovementSpeed => iconStatMovementSpeed,
            _ => null
        };
    }

    private void OnNewSkill1Clicked()
    {
        if (abilitySlot == 1)
        {
            player.GetComponent<Loadout>().Ability1 = abilityChoices[0];
            imageAbilityBar1.GetComponent<Image>().sprite = abilityChoices[0].Icon;
        }
        else if (abilitySlot == 2)
        {
            player.GetComponent<Loadout>().Ability2 = abilityChoices[0];
            imageAbilityBar2.GetComponent<Image>().sprite = abilityChoices[0].Icon;
        }
        else if (abilitySlot == 3)
        {
            player.GetComponent<Loadout>().Ability3 = abilityChoices[0];
            imageAbilityBar3.GetComponent<Image>().sprite = abilityChoices[0].Icon;
        }

        newSkillPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnNewSkill2Clicked()
    {
        if (abilitySlot == 1)
        {
            player.GetComponent<Loadout>().Ability1 = abilityChoices[1];
            imageAbilityBar1.GetComponent<Image>().sprite = abilityChoices[1].Icon;
        }
        else if (abilitySlot == 2)
        {
            player.GetComponent<Loadout>().Ability2 = abilityChoices[1];
            imageAbilityBar2.GetComponent<Image>().sprite = abilityChoices[1].Icon;
        }
        else if (abilitySlot == 3)
        {
            player.GetComponent<Loadout>().Ability3 = abilityChoices[1];
            imageAbilityBar3.GetComponent<Image>().sprite = abilityChoices[1].Icon;
        }

        newSkillPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnNewSkill3Clicked()
    {
        if (abilitySlot == 1)
        {
            player.GetComponent<Loadout>().Ability1 = abilityChoices[2];
            imageAbilityBar1.GetComponent<Image>().sprite = abilityChoices[2].Icon;
        }
        else if (abilitySlot == 2)
        {
            player.GetComponent<Loadout>().Ability2 = abilityChoices[2];
            imageAbilityBar2.GetComponent<Image>().sprite = abilityChoices[2].Icon;
        }
        else if (abilitySlot == 3)
        {
            player.GetComponent<Loadout>().Ability3 = abilityChoices[2];
            imageAbilityBar3.GetComponent<Image>().sprite = abilityChoices[2].Icon;
        }

        newSkillPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnLevelUpSkill1Clicked()
    {
        Loadout playerLoadout = player.GetComponent<Loadout>();

        if (playerLoadout.Ability1 == null || playerLoadout.Ability1.Level >= 5)
        {
            return;
        }
        
        playerLoadout.Ability1.Level++;

        levelUpSkillPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnLevelUpSkill2Clicked()
    {
        Loadout playerLoadout = player.GetComponent<Loadout>();

        if (playerLoadout.Ability2 == null || playerLoadout.Ability2.Level >= 5)
        {
            return;
        }

        playerLoadout.Ability2.Level++;

        levelUpSkillPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnLevelUpSkill3Clicked()
    {
        Loadout playerLoadout = player.GetComponent<Loadout>();

        if (playerLoadout.Ability3 == null || playerLoadout.Ability3.Level >= 5)
        {
            return;
        }

        playerLoadout.Ability3.Level++;

        levelUpSkillPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnLevelUpStats1Clicked()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerStats>().ApplyStatBuff(statChoices[0]);

        statsPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnLevelUpStats2Clicked()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerStats>().ApplyStatBuff(statChoices[1]);

        statsPanel.SetActive(false);
        playerLevels.ConfirmLevelUp();
    }

    private void OnLevelUpStats3Clicked()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerStats>().ApplyStatBuff(statChoices[2]);

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

        Ability playerAbility1 = player.GetComponent<Loadout>().Ability1;
        Ability playerAbility2 = player.GetComponent<Loadout>().Ability2;
        Ability playerAbility3 = player.GetComponent<Loadout>().Ability3;
        if (playerAbility1 != null)
        {
            imageAbilityBar1Overlay.fillAmount = playerAbility1.CurrentCD / playerAbility1.CD;
        }
        if (playerAbility2 != null)
        {
            imageAbilityBar2Overlay.fillAmount = playerAbility2.CurrentCD / playerAbility2.CD;
        }
        if (playerAbility3 != null)
        {
            imageAbilityBar3Overlay.fillAmount = playerAbility3.CurrentCD / playerAbility3.CD;
        }
    }

    private IEnumerator PlaySoundThenEnableButtons()
    {
        buttonsInteractable = false;

        var fadeList = new List<Image>();
        fadeList.Add(buttonNewSkill.GetComponent<Image>());
        fadeList.Add(buttonLevelUpSkill.GetComponent<Image>());
        fadeList.Add(buttonLevelUpStats.GetComponent<Image>());

        fadeList.AddRange(buttonNewSkillBorders.GetComponentsInChildren<Image>());
        fadeList.AddRange(buttonLevelUpSkillBorders.GetComponentsInChildren<Image>());
        fadeList.AddRange(buttonLevelUpStatsBorders.GetComponentsInChildren<Image>());

        Image[] fadeTargets = fadeList.ToArray();

        audioSource.PlayOneShot(sfxLevelUp);

        FadeImages(fadeTargets, 0f);

        float elapsed = 0f;
        float duration = sfxLevelUp.length;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            FadeImages(fadeTargets, alpha);
            yield return null;
        }

        FadeImages(fadeTargets, 1f);
        buttonsInteractable = true;
    }

    private void FadeImages(Image[] fadeTargets, float alpha)
    {
        foreach (Image img in fadeTargets)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }
}
