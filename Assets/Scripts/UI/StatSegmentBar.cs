using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StatSegmentBar : MonoBehaviour
{
    [SerializeField] string statName;
    [SerializeField] int statValue;
    [SerializeField] SpriteRenderer segment1;
    [SerializeField] SpriteRenderer segment2;
    [SerializeField] SpriteRenderer segment3;
    [SerializeField] SpriteRenderer segment4;
    [SerializeField] SpriteRenderer segment5;
    [SerializeField] SpriteRenderer segment6;
    [SerializeField] SpriteRenderer segment7;
    [SerializeField] SpriteRenderer segment8;
    [SerializeField] SpriteRenderer segment9;
    [SerializeField] SpriteRenderer segment10;
    GunSettings gunSettings;
    [SerializeField] DisplayWeapon displayWeapon;

    [SerializeField] Color shownColor;
    [SerializeField] Color hiddenColor;

    void Start()
    {
        if (displayWeapon != null)
            gunSettings = displayWeapon.ThisGun;
        else
            Debug.LogError("StatSegmentBar: No DisplayWeapon found in parent hierarchy.");
    }
    void GetStat()
    {
        if (displayWeapon != null)
            gunSettings = displayWeapon.ThisGun;
        else
            Debug.LogError("StatSegmentBar: No DisplayWeapon found in parent hierarchy.");
        if (gunSettings == null)
        {
            Debug.LogError("StatSegmentBar: GunSettings is null. Ensure DisplayWeapon has a valid gun assigned.");
            return;
        }
        GunStats stats = new GunStats(gunSettings);
        if (statName == "Damage") statValue = stats.CalculateDamage();
        else if (statName == "Fire Rate") statValue = stats.CalculateFireRate();
        else if (statName == "Reload Speed") statValue = stats.CalculateReloadSpeed();
        else if (statName == "Magazine Size") statValue = stats.CalculateMagazineSize();
        else if (statName == "Accuracy") statValue = stats.CalculateAccuracy();
        else if (statName == "Range") statValue = stats.CalculateRange();
        else if (statName == "Weight") statValue = stats.CalculateWeight();
    }

    void Update()
    {
        GetStat();
        if (statValue >= 1) segment1.color = shownColor; else segment1.color = hiddenColor;
        if (statValue >= 2) segment2.color = shownColor; else segment2.color = hiddenColor;
        if (statValue >= 3) segment3.color = shownColor; else segment3.color = hiddenColor;
        if (statValue >= 4) segment4.color = shownColor; else segment4.color = hiddenColor;
        if (statValue >= 5) segment5.color = shownColor; else segment5.color = hiddenColor;
        if (statValue >= 6) segment6.color = shownColor; else segment6.color = hiddenColor;
        if (statValue >= 7) segment7.color = shownColor; else segment7.color = hiddenColor;
        if (statValue >= 8) segment8.color = shownColor; else segment8.color = hiddenColor;
        if (statValue >= 9) segment9.color = shownColor; else segment9.color = hiddenColor;
        if (statValue >= 10) segment10.color = shownColor; else segment10.color = hiddenColor;
    }
}
