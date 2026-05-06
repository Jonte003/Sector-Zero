using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LobbyAbilityManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI abilityCountText;
    [SerializeField] TextMeshProUGUI abilityListTextLeft;
    [SerializeField] TextMeshProUGUI abilityListTextRight;
    [SerializeField] int requiredAbilityCount = 10;

    private List<AbilityWallButton> allButtons = new();
    private HashSet<Ability> enabledAbilities = new();

    void Start()
    {
        GameObject wallNodeGroup = GameObject.Find("WallNodeGroup");
        if (wallNodeGroup != null)
        {
            allButtons.AddRange(wallNodeGroup.GetComponentsInChildren<AbilityWallButton>());
        }
        UpdateUI();
    }
    public void ToggleAbility(Ability ability)
    {
        ability.Enabled = !ability.Enabled;
        
        if (ability.Enabled)
        {
            enabledAbilities.Add(ability);
        }
        else
        {
            enabledAbilities.Remove(ability);
        }

        UpdateUI();
        SyncLoadoutManager();
    }

    void UpdateUI()
    {
        int count = enabledAbilities.Count;
        abilityCountText.text = $"{count}/{requiredAbilityCount} Abilities";
        abilityCountText.color = count == requiredAbilityCount ? Color.green : Color.white;

        var sorted = enabledAbilities.OrderBy(a => a.Name).Select(a => a.Name).ToList();

        int half = Mathf.CeilToInt(sorted.Count / 2f);
        var leftColumn = sorted.Take(half);
        var rightColumn = sorted.Skip(half);

        abilityListTextLeft.text = string.Join("\n", leftColumn);
        abilityListTextRight.text = string.Join("\n", rightColumn);
    }
    void SyncLoadoutManager()
    {
        LoadoutManager.AbilityTypesInBag = enabledAbilities.Select(a => a.GetType()).ToArray();
    }
}
