using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfo : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI currentAmmoText;
    //[SerializeField] TextMeshProUGUI maxAmmoText;
    [SerializeField] Image Ones1;
    [SerializeField] Image Ones2;
    [SerializeField] Image Ones3;
    [SerializeField] Image Ones4;
    [SerializeField] Image Ones5;
    [SerializeField] Image Ones6;
    [SerializeField] Image Ones7;
    [SerializeField] Image Ones8;
    [SerializeField] Image Ones9;
    [SerializeField] Image Ones10;
    [SerializeField] Image Tens1;
    [SerializeField] Image Tens2;
    [SerializeField] Image Tens3;
    [SerializeField] Image Tens4;
    [SerializeField] Image Tens5;
    [SerializeField] Image Tens6;
    [SerializeField] Image Tens7;
    [SerializeField] Image Tens8;
    [SerializeField] Image Tens9;
    [SerializeField] Image Tens10;

    [SerializeField] Color OnesColor;
    [SerializeField] Color TensColor;
    [SerializeField] Color EmptyColor;
    [SerializeField] Color OverCapColor;

    private PlayerShoot playerShoot;
    private Gun currentGun;
    private int currentAmmo;
    private int maxAmmo;
    void Start()
    {
        playerShoot = GameObject.FindWithTag("Player").GetComponent<PlayerShoot>();
    }

    void Update()
    {
        currentGun = playerShoot.CurrentGun;
        currentAmmo = currentGun.CurrentAmmoInt();
        maxAmmo = currentGun.MaxAmmoInt();

        //currentAmmoText.text = currentAmmo.ToString();
        //maxAmmoText.text = maxAmmo.ToString();

        UpdateColors();
    }
    void UpdateColors()
    {
        Image[] ones = { Ones1, Ones2, Ones3, Ones4, Ones5, Ones6, Ones7, Ones8, Ones9, Ones10 };
        Image[] tens = { Tens1, Tens2, Tens3, Tens4, Tens5, Tens6, Tens7, Tens8, Tens9, Tens10 };

        int onesCount = currentAmmo % 10;
        int tensCount = Mathf.Min(currentAmmo / 10, 10);
        int maxOnes = Mathf.Min(maxAmmo, 10);
        int maxTens = maxAmmo / 10;

        for (int i = 0; i < 10; i++)
        {
            if (i < onesCount)
            {
                ones[i].color = OnesColor;
            }
            else if (i < maxOnes)
            {
                ones[i].color = EmptyColor;
            }
            else
            {
                ones[i].color = OverCapColor;
            }

            if (i < tensCount)
            {
                tens[i].color = TensColor;
            }
            else if (i < maxTens)
            {
                tens[i].color = EmptyColor;
            }
            else
            {
                tens[i].color = OverCapColor;
            }
        }
    }
}