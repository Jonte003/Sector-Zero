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
    [SerializeField] private TextMeshProUGUI multiplierWeaponDamageText;
    [SerializeField] private TextMeshProUGUI totalWeaponDamageText;

    [SerializeField] private TextMeshProUGUI baseFireRateText;
    [SerializeField] private TextMeshProUGUI multiplierFireRateText;
    [SerializeField] private TextMeshProUGUI totalFireRateText;

    [SerializeField] private TextMeshProUGUI baseSpreadText;
    [SerializeField] private TextMeshProUGUI multiplierSpreadText;
    [SerializeField] private TextMeshProUGUI totalSpreadText;

    [SerializeField] private TextMeshProUGUI baseRecoilText;
    [SerializeField] private TextMeshProUGUI multiplierRecoilText;
    [SerializeField] private TextMeshProUGUI totalRecoilText;

    [SerializeField] private TextMeshProUGUI baseMovementSpeedText;
    [SerializeField] private TextMeshProUGUI multiplierMovementSpeedText;
    [SerializeField] private TextMeshProUGUI totalMovementSpeedText;

    [SerializeField] private TextMeshProUGUI baseMagazineSizeText;
    [SerializeField] private TextMeshProUGUI multiplierMagazineSizeText;
    [SerializeField] private TextMeshProUGUI totalMagazineSizeText;

    [SerializeField] private TextMeshProUGUI baseReloadSpeedText;
    [SerializeField] private TextMeshProUGUI multiplierReloadSpeedText;
    [SerializeField] private TextMeshProUGUI totalReloadSpeedText;

    private Gun activeGun;
    private GunMod activeBarrel;
    private GunMod activeGrip;
    private GunMod activeStock;
    private GunMod activeMagazine;
    private GunMod activeMuzzle;

    void Start()
    {
        gunTypeButtons = new Button[] { assaultRifle, burstRifle, laserRifle, pistol, shotgun, submachineGun };
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
    }

    #region Buttons Gun Types
    public void OnClickedAssaultRifle()
    {
        //activeGun = new Gun(); //How do we make this a gun type? Works differently than the gun mods.
        foreach (Button button in gunTypeButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        assaultRifle.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedBurstRifle()
    {
        //activeGun = new Gun(); //How do we make this a gun type? Works differently than the gun mods.
        foreach (Button button in gunTypeButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        burstRifle.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedLaserRifle()
    {
        //activeGun = new Gun(); //How do we make this a gun type? Works differently than the gun mods.
        foreach (Button button in gunTypeButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        laserRifle.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedPistol()
    {
        //activeGun = new Gun(); //How do we make this a gun type? Works differently than the gun mods.
        foreach (Button button in gunTypeButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        pistol.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedShotgun()
    {
        //activeGun = new Gun(); //How do we make this a gun type? Works differently than the gun mods.
        foreach (Button button in gunTypeButtons)
        {
            button.GetComponent<Image>().color = inactiveColor;
        }
        shotgun.GetComponent<Image>().color = activeColor;
    }
    public void OnClickedSubmachineGun()
    {
        //activeGun = new Gun(); //How do we make this a gun type? Works differently than the gun mods.
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
        activeBarrel = new BlankBarrel();
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
        activeGrip = new BlankGrip();
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
        activeStock = new BlankStock();
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
        activeMagazine = new BlankMagazine();
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
        activeMuzzle = new BlankMuzzle();
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
        float baseWeaponDamage = 100; //this should be gun type base damage
        float baseFireRate = 100; //this should be gun type base fire rate
        float baseSpread = 100; //this should be gun type base spread
        float baseRecoil = 100; //this should be gun type base recoil
        float baseMovementSpeed = 100; //this should be gun type base movement speed
        float baseMagazineSize = 100; //this should be gun type base magazine size
        float baseReloadSpeed = 100; //this should be gun type base reload speed

        float multiplierWeaponDamage = activeBarrel.WeaponDamage + activeGrip.WeaponDamage + activeStock.WeaponDamage + activeMagazine.WeaponDamage + activeMuzzle.WeaponDamage;
        float multiplierFireRate = activeBarrel.FireRate     + activeGrip.FireRate     + activeStock.FireRate     + activeMagazine.FireRate     + activeMuzzle.FireRate;
        float multiplierSpread = activeBarrel.Spread       + activeGrip.Spread       + activeStock.Spread       + activeMagazine.Spread       + activeMuzzle.Spread;
        float multiplierRecoil = activeBarrel.Recoil       + activeGrip.Recoil       + activeStock.Recoil       + activeMagazine.Recoil       + activeMuzzle.Recoil;
        float multiplierMovementSpeed = activeBarrel.MoveSpeed    + activeGrip.MoveSpeed    + activeStock.MoveSpeed    + activeMagazine.MoveSpeed    + activeMuzzle.MoveSpeed;
        float multiplierMagazineSize = activeBarrel.MagSize      + activeGrip.MagSize      + activeStock.MagSize      + activeMagazine.MagSize      + activeMuzzle.MagSize;
        float multiplierReloadSpeed = activeBarrel.ReloadSpeed  + activeGrip.ReloadSpeed  + activeStock.ReloadSpeed  + activeMagazine.ReloadSpeed  + activeMuzzle.ReloadSpeed;

        baseWeaponDamageText.text = baseWeaponDamage.ToString();
        baseFireRateText.text = baseFireRate.ToString();
        baseSpreadText.text = baseSpread.ToString();
        baseRecoilText.text = baseRecoil.ToString();
        baseMovementSpeedText.text = baseMovementSpeed.ToString();
        baseMagazineSizeText.text = baseMagazineSize.ToString();
        baseReloadSpeedText.text = baseReloadSpeed.ToString();

        multiplierWeaponDamageText.text = (multiplierWeaponDamage * 100).ToString("F1") + "%";
        multiplierFireRateText.text = (multiplierFireRate * 100).ToString("F1") + "%";
        multiplierSpreadText.text = (multiplierSpread * 100).ToString("F1") + "%";
        multiplierRecoilText.text = (multiplierRecoil * 100).ToString("F1") + "%";
        multiplierMovementSpeedText.text = (multiplierMovementSpeed * 100).ToString("F1") + "%";
        multiplierMagazineSizeText.text = (multiplierMagazineSize * 100).ToString("F1") + "%";
        multiplierReloadSpeedText.text = (multiplierReloadSpeed * 100).ToString("F1") + "%";

        totalWeaponDamageText.text = (baseWeaponDamage * (1 + multiplierWeaponDamage)).ToString();
        totalFireRateText.text = (baseFireRate * (1 + multiplierFireRate)).ToString();
        totalSpreadText.text = (baseSpread * (1 + multiplierSpread)).ToString();
        totalRecoilText.text = (baseRecoil * (1 + multiplierRecoil)).ToString();
        totalMovementSpeedText.text = (baseMovementSpeed * (1 + multiplierMovementSpeed)).ToString();
        totalMagazineSizeText.text = (baseMagazineSize * (1 + multiplierMagazineSize)).ToString();
        totalReloadSpeedText.text = (baseReloadSpeed * (1 + multiplierReloadSpeed)).ToString();
    }
}
