using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutUI : MonoBehaviour
{
    [SerializeField] private Button assaultRifle;
    [SerializeField] private Button burstRifle;
    [SerializeField] private Button laserRifle;
    [SerializeField] private Button pistol;
    [SerializeField] private Button shotgun;
    [SerializeField] private Button submachineGun;
    private Button[] gunTypeButtons;

    [SerializeField] private Button basicBarrel;
    [SerializeField] private Button shortBarrel;
    [SerializeField] private Button longBarrel;
    [SerializeField] private Button portedBarrel;
    private Button[] barrelButtons;

    [SerializeField] private Button basicGrip;
    [SerializeField] private Button verticalGrip;
    [SerializeField] private Button angledGrip;
    [SerializeField] private Button ergonomicGrip;
    private Button[] gripButtons;

    [SerializeField] private Button basicStock;
    [SerializeField] private Button heavyStock;
    [SerializeField] private Button lightStock;
    [SerializeField] private Button balancedStock;
    private Button[] stockButtons;


    [SerializeField] private Button basicMagazine;
    [SerializeField] private Button extendedMagazine;
    [SerializeField] private Button drumMagazine;
    [SerializeField] private Button fastMagazine;
    private Button[] magazineButtons;

    [SerializeField] private Button basicMuzzle;
    [SerializeField] private Button suppressor;
    [SerializeField] private Button muzzleBreak;
    [SerializeField] private Button compensator;
    private Button[] muzzleButtons;

    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.darkGray;

    [SerializeField] private TextMeshProUGUI baseWeaponDamageText;
    [SerializeField] private TextMeshProUGUI barrelMultiplierWeaponDamageText;
    [SerializeField] private TextMeshProUGUI gripMultiplierWeaponDamageText;
    [SerializeField] private TextMeshProUGUI stockMultiplierWeaponDamageText;
    [SerializeField] private TextMeshProUGUI magazineMultiplierWeaponDamageText;
    [SerializeField] private TextMeshProUGUI muzzleMultiplierWeaponDamageText;
    [SerializeField] private TextMeshProUGUI totalMultiplierWeaponDamageText;
    [SerializeField] private TextMeshProUGUI resultWeaponDamageText;

    [SerializeField] private TextMeshProUGUI baseFireRateText;
    [SerializeField] private TextMeshProUGUI barrelMultiplierFireRateText;
    [SerializeField] private TextMeshProUGUI gripMultiplierFireRateText;
    [SerializeField] private TextMeshProUGUI stockMultiplierFireRateText;
    [SerializeField] private TextMeshProUGUI magazineMultiplierFireRateText;
    [SerializeField] private TextMeshProUGUI muzzleMultiplierFireRateText;
    [SerializeField] private TextMeshProUGUI totalMultiplierFireRateText;
    [SerializeField] private TextMeshProUGUI resultFireRateText;

    [SerializeField] private TextMeshProUGUI baseSpreadText;
    [SerializeField] private TextMeshProUGUI barrelMultiplierSpreadText;
    [SerializeField] private TextMeshProUGUI gripMultiplierSpreadText;
    [SerializeField] private TextMeshProUGUI stockMultiplierSpreadText;
    [SerializeField] private TextMeshProUGUI magazineMultiplierSpreadText;
    [SerializeField] private TextMeshProUGUI muzzleMultiplierSpreadText;
    [SerializeField] private TextMeshProUGUI totalMultiplierSpreadText;
    [SerializeField] private TextMeshProUGUI resultSpreadText;

    [SerializeField] private TextMeshProUGUI baseRecoilText;
    [SerializeField] private TextMeshProUGUI barrelMultiplierRecoilText;
    [SerializeField] private TextMeshProUGUI gripMultiplierRecoilText;
    [SerializeField] private TextMeshProUGUI stockMultiplierRecoilText;
    [SerializeField] private TextMeshProUGUI magazineMultiplierRecoilText;
    [SerializeField] private TextMeshProUGUI muzzleMultiplierRecoilText;
    [SerializeField] private TextMeshProUGUI totalMultiplierRecoilText;
    [SerializeField] private TextMeshProUGUI resultRecoilText;

    [SerializeField] private TextMeshProUGUI baseMovementSpeedText;
    [SerializeField] private TextMeshProUGUI barrelMultiplierMovementSpeedText;
    [SerializeField] private TextMeshProUGUI gripMultiplierMovementSpeedText;
    [SerializeField] private TextMeshProUGUI stockMultiplierMovementSpeedText;
    [SerializeField] private TextMeshProUGUI magazineMultiplierMovementSpeedText;
    [SerializeField] private TextMeshProUGUI muzzleMultiplierMovementSpeedText;
    [SerializeField] private TextMeshProUGUI totalMultiplierMovementSpeedText;
    [SerializeField] private TextMeshProUGUI resultMovementSpeedText;

    [SerializeField] private TextMeshProUGUI baseMagazineSizeText;
    [SerializeField] private TextMeshProUGUI barrelMultiplierMagazineSizeText;
    [SerializeField] private TextMeshProUGUI gripMultiplierMagazineSizeText;
    [SerializeField] private TextMeshProUGUI stockMultiplierMagazineSizeText;
    [SerializeField] private TextMeshProUGUI magazineMultiplierMagazineSizeText;
    [SerializeField] private TextMeshProUGUI muzzleMultiplierMagazineSizeText;
    [SerializeField] private TextMeshProUGUI totalMultiplierMagazineSizeText;
    [SerializeField] private TextMeshProUGUI resultMagazineSizeText;

    [SerializeField] private TextMeshProUGUI baseReloadSpeedText;
    [SerializeField] private TextMeshProUGUI barrelMultiplierReloadSpeedText;
    [SerializeField] private TextMeshProUGUI gripMultiplierReloadSpeedText;
    [SerializeField] private TextMeshProUGUI stockMultiplierReloadSpeedText;
    [SerializeField] private TextMeshProUGUI magazineMultiplierReloadSpeedText;
    [SerializeField] private TextMeshProUGUI muzzleMultiplierReloadSpeedText;
    [SerializeField] private TextMeshProUGUI totalMultiplierReloadSpeedText;
    [SerializeField] private TextMeshProUGUI resultReloadSpeedText;

    [SerializeField] private TextMeshProUGUI gunTypeHeaderText;
    [SerializeField] private TextMeshProUGUI barrelHeaderText;
    [SerializeField] private TextMeshProUGUI gripHeaderText;
    [SerializeField] private TextMeshProUGUI stockHeaderText;
    [SerializeField] private TextMeshProUGUI magazineHeaderText;
    [SerializeField] private TextMeshProUGUI muzzleHeaderText;

    private GunSettings activeGun;
    private GunMod activeBarrel;
    private GunMod activeGrip;
    private GunMod activeStock;
    private GunMod activeMagazine;
    private GunMod activeMuzzle;

    private List<Ability> abilitiesList;
    private int activeAbilityCount = 0;

    [SerializeField] private Button abilityButtonPrefab;
    [SerializeField] private Transform abilityButtonGridParent;
    [SerializeField] private TextMeshProUGUI activeAbilityCountText;

    void Start()
    {
        gunTypeButtons = new Button[] { assaultRifle, burstRifle, pistol, shotgun, submachineGun };
        barrelButtons = new Button[] { basicBarrel, shortBarrel, longBarrel, portedBarrel };
        gripButtons = new Button[] { basicGrip, verticalGrip, angledGrip, ergonomicGrip };
        stockButtons = new Button[] { basicStock, heavyStock, lightStock, balancedStock };
        magazineButtons = new Button[] { basicMagazine, extendedMagazine, drumMagazine, fastMagazine };
        muzzleButtons = new Button[] { basicMuzzle, suppressor, muzzleBreak, compensator };

        OnClickedAssaultRifle();
        OnClickedBasicBarrel();
        OnClickedBasicGrip();
        OnClickedBasicStock();
        OnClickedBasicMagazine();
        OnClickedBasicMuzzle();
        Debug.Log("Initialized gun and mods");
        InitializeAbilityList();

        foreach (Ability ability in abilitiesList)
        {
            Button button = Instantiate(abilityButtonPrefab, abilityButtonGridParent);
            Image buttonImage = button.GetComponent<Image>();
            buttonImage.color = inactiveColor;
            //button.GetComponentInChildren<TextMeshProUGUI>().text = ability.Name;
            buttonImage.sprite = ability.Icon;

            button.onClick.AddListener(() =>
            {
                if (ability.Enabled)
                {
                    buttonImage.color = inactiveColor;
                }
                else
                {
                    buttonImage.color = activeColor;
                }
                ability.Enabled = !ability.Enabled;
            });
        }
    }
    void InitializeAbilityList()
    {
        abilitiesList = new List<Ability>();
        abilitiesList.Add(new Backstep());
        abilitiesList.Add(new Blink());
        abilitiesList.Add(new ChainLightning());
        abilitiesList.Add(new Charge());
        abilitiesList.Add(new Dash());
        abilitiesList.Add(new Eruption());
        abilitiesList.Add(new Explosion());
        abilitiesList.Add(new Fortify());
        abilitiesList.Add(new GroundSlam());
        abilitiesList.Add(new Invincible());
        abilitiesList.Add(new Jump());
        abilitiesList.Add(new Knockback());
        abilitiesList.Add(new Leap());
        abilitiesList.Add(new VitalSurge());
    }

    Ability[] MakeActiveAbilityLoadout()
    {
        Ability[] activeAbilityLoadout = new Ability[10];
        int index = 0;
        foreach (Ability ability in abilitiesList)
        {
            if (ability.Enabled)
            {
                activeAbilityLoadout[index] = ability;
                index++;
                if (index >= 10)
                {
                    break;
                }
            }
        }
        return activeAbilityLoadout;
    }

    #region Buttons Gun Types
    public void OnClickedAssaultRifle()
    {
        activeGun = GunSettings.AssaultRifle;
        foreach (Button button in gunTypeButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        assaultRifle.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedBurstRifle()
    {
        activeGun = GunSettings.BurstRifle;
        foreach (Button button in gunTypeButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        burstRifle.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedPistol()
    {
        activeGun = GunSettings.Pistol;
        foreach (Button button in gunTypeButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        pistol.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedShotgun()
    {
        activeGun = GunSettings.Shotgun;
        foreach (Button button in gunTypeButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        shotgun.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedSubmachineGun()
    {
        activeGun = GunSettings.Smg;
        foreach (Button button in gunTypeButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        submachineGun.GetComponent<Image>().color = activeColor;
    }
    #endregion
    #region Buttons Barrels
    public void OnClickedBasicBarrel()
    {
        activeBarrel = new BasicBarrel();
        foreach (Button button in barrelButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        basicBarrel.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedShortBarrel()
    {
        activeBarrel = new ShortBarrel();
        foreach (Button button in barrelButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        shortBarrel.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedLongBarrel()
    {
        activeBarrel = new LongBarrel();
        foreach (Button button in barrelButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        longBarrel.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedPortedBarrel()
    {
        activeBarrel = new PortedBarrel();
        foreach (Button button in barrelButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        portedBarrel.GetComponent<Image>().color = activeColor;
    }
    #endregion
    #region Buttons Grips
    public void OnClickedBasicGrip()
    {
        activeGrip = new BasicGrip();
        foreach (Button button in gripButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        basicGrip.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedVerticalGrip()
    {
        activeGrip = new VerticalGrip();
        foreach (Button button in gripButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        verticalGrip.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedAngledGrip()
    {
        activeGrip = new AngledGrip();
        foreach (Button button in gripButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        angledGrip.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedErgonomicGrip()
    {
        activeGrip = new ErgonomicGrip();
        foreach (Button button in gripButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        ergonomicGrip.GetComponent<Image>().color = activeColor;
    }
    #endregion
    #region Buttons Stocks
    public void OnClickedBasicStock()
    {
        activeStock = new BasicStock();
        foreach (Button button in stockButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        basicStock.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedHeavyStock()
    {
        activeStock = new HeavyStock();
        foreach (Button button in stockButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        heavyStock.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedLightStock()
    {
        activeStock = new LightStock();
        foreach (Button button in stockButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        lightStock.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedBalancedStock()
    {
        activeStock = new BalancedStock();
        foreach (Button button in stockButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        balancedStock.GetComponent<Image>().color = activeColor;
    }
    #endregion
    #region Buttons Magazines
    public void OnClickedBasicMagazine()
    {
        activeMagazine = new BasicMagazine();
        foreach (Button button in magazineButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        basicMagazine.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedExtendedMagazine()
    {
        activeMagazine = new ExtendedMagazine();
        foreach (Button button in magazineButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        extendedMagazine.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedDrumMagazine()
    {
        activeMagazine = new DrumMagazine();
        foreach (Button button in magazineButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        drumMagazine.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedFastMagazine()
    {
        activeMagazine = new FastMagazine();
        foreach (Button button in magazineButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        fastMagazine.GetComponent<Image>().color = activeColor;
    }
    #endregion
    #region Buttons Muzzles
    public void OnClickedBasicMuzzle()
    {
        activeMuzzle = new BasicMuzzle();
        foreach (Button button in muzzleButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        basicMuzzle.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedSuppressor()
    {
        activeMuzzle = new Suppressor();
        foreach (Button button in muzzleButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        suppressor.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedMuzzleBreak()
    {
        activeMuzzle = new MuzzleBreak();
        foreach (Button button in muzzleButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        muzzleBreak.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedCompensator()
    {
        activeMuzzle = new Compensator();
        foreach (Button button in muzzleButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        compensator.GetComponent<Image>().color = activeColor;
    }
    #endregion

    void Update()
    {
        float baseWeaponDamage = activeGun.Damage;
        float baseFireRate = activeGun.FireRate;
        Vector2 baseSpread = activeGun.MaxSpread;
        Vector2 baseRecoil = activeGun.RecoilMagnitude;
        float baseMovementSpeed = activeGun.MoveSpeed;
        float baseMagazineSize = activeGun.MaxAmmo;
        float baseReloadSpeed = activeGun.ReloadSpeed;

        float totalMultiplierWeaponDamage = activeBarrel.WeaponDamage + activeGrip.WeaponDamage + activeStock.WeaponDamage + activeMagazine.WeaponDamage + activeMuzzle.WeaponDamage;
        float totalMultiplierFireRate = activeBarrel.FireRate     + activeGrip.FireRate     + activeStock.FireRate     + activeMagazine.FireRate     + activeMuzzle.FireRate;
        float totalMultiplierSpread = activeBarrel.Spread       + activeGrip.Spread       + activeStock.Spread       + activeMagazine.Spread       + activeMuzzle.Spread;
        float totalMultiplierRecoil = activeBarrel.Recoil       + activeGrip.Recoil       + activeStock.Recoil       + activeMagazine.Recoil       + activeMuzzle.Recoil;
        float totalMultiplierMovementSpeed = activeBarrel.MoveSpeed    + activeGrip.MoveSpeed    + activeStock.MoveSpeed    + activeMagazine.MoveSpeed    + activeMuzzle.MoveSpeed;
        float totalMultiplierMagazineSize = activeBarrel.MagSize      + activeGrip.MagSize      + activeStock.MagSize      + activeMagazine.MagSize      + activeMuzzle.MagSize;
        float totalMultiplierReloadSpeed = activeBarrel.ReloadSpeed  + activeGrip.ReloadSpeed  + activeStock.ReloadSpeed  + activeMagazine.ReloadSpeed  + activeMuzzle.ReloadSpeed;

        baseWeaponDamageText.text = baseWeaponDamage.ToString();
        barrelMultiplierWeaponDamageText.text = activeBarrel.WeaponDamage.ToString("F2");
        gripMultiplierWeaponDamageText.text = activeGrip.WeaponDamage.ToString("F2");
        stockMultiplierWeaponDamageText.text = activeStock.WeaponDamage.ToString("F2");
        magazineMultiplierWeaponDamageText.text = activeMagazine.WeaponDamage.ToString("F2");
        muzzleMultiplierWeaponDamageText.text = activeMuzzle.WeaponDamage.ToString("F2");

        baseFireRateText.text = baseFireRate.ToString();
        barrelMultiplierFireRateText.text = activeBarrel.FireRate.ToString("F2");
        gripMultiplierFireRateText.text = activeGrip.FireRate.ToString("F2");
        stockMultiplierFireRateText.text = activeStock.FireRate.ToString("F2");
        magazineMultiplierFireRateText.text = activeMagazine.FireRate.ToString("F2");
        muzzleMultiplierFireRateText.text = activeMuzzle.FireRate.ToString("F2");
        
        baseSpreadText.text = baseSpread.ToString();
        barrelMultiplierSpreadText.text = activeBarrel.Spread.ToString("F2");
        gripMultiplierSpreadText.text = activeGrip.Spread.ToString("F2");
        stockMultiplierSpreadText.text = activeStock.Spread.ToString("F2");
        magazineMultiplierSpreadText.text = activeMagazine.Spread.ToString("F2");
        muzzleMultiplierSpreadText.text = activeMuzzle.Spread.ToString("F2");

        baseRecoilText.text = baseRecoil.ToString();
        barrelMultiplierRecoilText.text = activeBarrel.Recoil.ToString("F2");
        gripMultiplierRecoilText.text = activeGrip.Recoil.ToString("F2");
        stockMultiplierRecoilText.text = activeStock.Recoil.ToString("F2");
        magazineMultiplierRecoilText.text = activeMagazine.Recoil.ToString("F2");
        muzzleMultiplierRecoilText.text = activeMuzzle.Recoil.ToString("F2");

        baseMovementSpeedText.text = baseMovementSpeed.ToString();
        barrelMultiplierMovementSpeedText.text = activeBarrel.MoveSpeed.ToString("F2");
        gripMultiplierMovementSpeedText.text = activeGrip.MoveSpeed.ToString("F2");
        stockMultiplierMovementSpeedText.text = activeStock.MoveSpeed.ToString("F2");
        magazineMultiplierMovementSpeedText.text = activeMagazine.MoveSpeed.ToString("F2");
        muzzleMultiplierMovementSpeedText.text = activeMuzzle.MoveSpeed.ToString("F2");

        baseMagazineSizeText.text = baseMagazineSize.ToString();
        barrelMultiplierMagazineSizeText.text = activeBarrel.MagSize.ToString("F2");
        gripMultiplierMagazineSizeText.text = activeGrip.MagSize.ToString("F2");
        stockMultiplierMagazineSizeText.text = activeStock.MagSize.ToString("F2");
        magazineMultiplierMagazineSizeText.text = activeMagazine.MagSize.ToString("F2");
        muzzleMultiplierMagazineSizeText.text = activeMuzzle.MagSize.ToString("F2");

        baseReloadSpeedText.text = baseReloadSpeed.ToString();
        barrelMultiplierReloadSpeedText.text = activeBarrel.ReloadSpeed.ToString("F2");
        gripMultiplierReloadSpeedText.text = activeGrip.ReloadSpeed.ToString("F2");
        stockMultiplierReloadSpeedText.text = activeStock.ReloadSpeed.ToString("F2");
        magazineMultiplierReloadSpeedText.text = activeMagazine.ReloadSpeed.ToString("F2");
        muzzleMultiplierReloadSpeedText.text = activeMuzzle.ReloadSpeed.ToString("F2");

        totalMultiplierWeaponDamageText.text = (totalMultiplierWeaponDamage * 100).ToString("F1") + "%";
        totalMultiplierFireRateText.text = (totalMultiplierFireRate * 100).ToString("F1") + "%";
        totalMultiplierSpreadText.text = (totalMultiplierSpread * 100).ToString("F1") + "%";
        totalMultiplierRecoilText.text = (totalMultiplierRecoil * 100).ToString("F1") + "%";
        totalMultiplierMovementSpeedText.text = (totalMultiplierMovementSpeed * 100).ToString("F1") + "%";
        totalMultiplierMagazineSizeText.text = (totalMultiplierMagazineSize * 100).ToString("F1") + "%";
        totalMultiplierReloadSpeedText.text = (totalMultiplierReloadSpeed * 100).ToString("F1") + "%";

        resultWeaponDamageText.text = (baseWeaponDamage * (1 + totalMultiplierWeaponDamage)) < 0 ? (baseWeaponDamage * (1 + totalMultiplierWeaponDamage)).ToString() : ((baseWeaponDamage * (1 + totalMultiplierWeaponDamage)) == 0 ? "0.00" : "+" + (baseWeaponDamage * (1 + totalMultiplierWeaponDamage)).ToString());
        resultFireRateText.text = (baseFireRate * (1 + totalMultiplierFireRate)).ToString();
        resultSpreadText.text = (baseSpread * (1 + totalMultiplierSpread)).ToString();
        resultRecoilText.text = (baseRecoil * (1 + totalMultiplierRecoil)).ToString();
        resultMovementSpeedText.text = (baseMovementSpeed * (1 + totalMultiplierMovementSpeed)).ToString();
        resultMagazineSizeText.text = (baseMagazineSize * (1 + totalMultiplierMagazineSize)).ToString();
        resultReloadSpeedText.text = (baseReloadSpeed * (1 + totalMultiplierReloadSpeed)).ToString();

        activeAbilityCount = 0;
        foreach (Ability ability in abilitiesList)
        {
            if (ability.Enabled)
            {
                activeAbilityCount++;
            }
        }
        activeAbilityCountText.text = activeAbilityCount.ToString() + "/10 abilities";
    }
}