using UnityEngine;

public class AbilityWallButton : MonoBehaviour, IInteractable
{
    [SerializeField] private string abilityName;
    private Renderer iconRenderer;

    [SerializeField] Color colorActive;
    [SerializeField] Color colorInactive;
    [SerializeField] Color colorUnimplemented;
    bool firstInteraction;
    public Ability Ability { get; private set; }
    [SerializeField] private LobbyAbilityManager manager;

    private void Start()
    {
        firstInteraction = true;
        iconRenderer = GetComponent<Renderer>();
        if (AbilityRegistry.All.TryGetValue(abilityName, out Ability ability))
        {
            //Debug.Log($"Found ability for {abilityName} button: {ability.Name}");
            Ability = ability;
            iconRenderer.material.mainTexture = ability.Icon != null ? ability.Icon.texture : null;
            iconRenderer.material.color = ability.NotYetImplemented ? colorUnimplemented : colorInactive;
        }
    }

    public void OnLookAt()
    {
        //Debug.Log($"Looking at {abilityName} button");
        TooltipManager.Instance.ShowTooltip(abilityName, Ability.Description);
    }

    public void OnLookAway()
    {
        TooltipManager.Instance.ClearTooltip();
    }

    public void OnInteract()
    {
        //Debug.Log($"Interacted with {abilityName} button");
        if (Ability == null || Ability.NotYetImplemented) return;
        manager.ToggleAbility(Ability, firstInteraction);
        iconRenderer.material.color = Ability.Enabled ? colorActive : colorInactive;
        firstInteraction = false;
    }
}