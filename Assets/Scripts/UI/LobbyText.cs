using System.Linq;
using TMPro;
using UnityEngine;

public class LobbyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textGunType;
    [SerializeField] private TextMeshProUGUI textModBarrel;
    [SerializeField] private TextMeshProUGUI textModMagazine;
    [SerializeField] private TextMeshProUGUI textModGrip;
    [SerializeField] private TextMeshProUGUI textModStock;
    [SerializeField] private TextMeshProUGUI textModMuzzle;

    void Update()
    {
        textGunType.text = "Gun: ";
        textModBarrel.text = "Barrel: ";
        textModMagazine.text = "Magazine: ";
        textModGrip.text = "Grip: ";
        textModStock.text = "Stock: ";
        textModMuzzle.text = "Muzzle: ";
        string gunNameText = "None";
        string modBarrelText = "None";
        string modMagazineText = "None";
        string modGripText = "None";
        string modStockText = "None";
        string modMuzzleText = "None";
        if (LoadoutManager.Settings != null)
        {
            gunNameText = LoadoutManager.Settings.GunName;
            modBarrelText = LoadoutManager.GunMods.FirstOrDefault(gm => gm.Category == GunModCategory.Barrel).Name;
            modMagazineText = LoadoutManager.GunMods.FirstOrDefault(gm => gm.Category == GunModCategory.Magazine).Name;
            modGripText = LoadoutManager.GunMods.FirstOrDefault(gm => gm.Category == GunModCategory.Grip).Name;
            modStockText = LoadoutManager.GunMods.FirstOrDefault(gm => gm.Category == GunModCategory.Stock).Name;
            modMuzzleText = LoadoutManager.GunMods.FirstOrDefault(gm => gm.Category == GunModCategory.Muzzle).Name;
        }
        textGunType.text += gunNameText;
        textModBarrel.text += modBarrelText;
        textModMagazine.text += modMagazineText;
        textModGrip.text += modGripText;
        textModStock.text += modStockText;
        textModMuzzle.text += modMuzzleText;
    }
}