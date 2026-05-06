using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GunModButton : MonoBehaviour
{
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private TextMeshPro descriptionText;
    [SerializeField] private GunModCategory gunModCategory;
    [SerializeField] private string gunModName;
    [SerializeField] private bool isInteractable;

    private GunMod modFromName(string name)
    {
        if (name == "Basic Barrel") return new BasicBarrel();
        if (name == "Long Barrel") return new LongBarrel();
        if (name == "Short Barrel") return new ShortBarrel();
        if (name == "Ported Barrel") return new PortedBarrel();

        if (name == "Basic Grip") return new BasicGrip();
        if (name == "Vertical Grip") return new VerticalGrip();
        if (name == "Angled Grip") return new AngledGrip();
        if (name == "Ergonomic Grip") return new ErgonomicGrip();

        if (name == "Basic Stock") return new BasicStock();
        if (name == "Heavy Stock") return new HeavyStock();
        if (name == "Light Stock") return new LightStock();
        if (name == "Balanced Stock") return new BalancedStock();

        if (name == "Basic Magazine") return new BasicMagazine();
        if (name == "Extended Magazine") return new ExtendedMagazine();
        if (name == "Drum Magazine") return new DrumMagazine();
        if (name == "Fast Magazine") return new FastMagazine();

        if (name == "Basic Muzzle") return new BasicMuzzle();
        if (name == "Suppressor") return new Suppressor();
        if (name == "Muzzle Break") return new MuzzleBreak();
        if (name == "Compensator") return new Compensator();

        return null;
    }

    public void SelectMod(string name)
    {
        GunMod newMod = modFromName(name);
        Debug.LogError($"No mod found for name: '{name}'", this);

        if (newMod == null) return;

        for (int i = 0; i < LoadoutManager.GunMods.Length; i++)
        {
            if (LoadoutManager.GunMods[i].Category == newMod.Category)
            {
                LoadoutManager.GunMods[i] = newMod;
                return;
            }
        }
    }

    void Update()
    {
        if (LoadoutManager.GunMods == null)
        {
            LoadoutManager.GunMods = new GunMod[] { new BasicBarrel(), new BasicGrip(), new BasicStock(), new BasicMagazine(), new BasicMuzzle() };
        }

        GunMod currentMod = LoadoutManager.GunMods.FirstOrDefault(gm => gm.Category == gunModCategory);

        nameText.text = currentMod.Name;
        descriptionText.text = currentMod.Description;
    }
}
