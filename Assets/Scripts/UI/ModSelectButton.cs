using System.Linq;
using TMPro;
using UnityEngine;

public class ModSelectButton : MonoBehaviour, IInteractable
{
    [SerializeField] private string gunModName;
    private GunModButton parent;
    [SerializeField] private TextMeshPro buttonText;
    [SerializeField] private float inactiveIntensity;
    private Color activeColor;
    private void Awake()
    {
        parent = GetComponentInParent<GunModButton>();
        activeColor = buttonText.color;
    }

    public void OnLookAt(){}
    public void OnLookAway(){}
    public void OnInteract()
    {
        Debug.Log("OnInteract called on " + gameObject.name, this);

        parent.SelectMod(gunModName);
    }

    void Update()
    {
        bool isActive = LoadoutManager.GunMods != null && LoadoutManager.GunMods.Any(gmod => gmod.Name == gunModName);
        buttonText.color = isActive ? activeColor : activeColor * inactiveIntensity;
    }
}
