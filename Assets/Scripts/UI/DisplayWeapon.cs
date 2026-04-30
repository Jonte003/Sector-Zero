using TMPro;
using UnityEngine;

public class DisplayWeapon : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] string weaponName;
    [SerializeField] TextMeshPro displayText;
    public GunSettings ThisGun;
    void Start()
    {
        if (weaponName == "Assault Rifle") { ThisGun = GunSettings.AssaultRifle; }
        else if (weaponName == "Burst Rifle") { ThisGun = GunSettings.BurstRifle; }
        else if (weaponName == "Shotgun") { ThisGun = GunSettings.Shotgun; }
        else if (weaponName == "Pistol") { ThisGun = GunSettings.Pistol; }
        else if (weaponName == "Smg") { ThisGun = GunSettings.Smg; }
        else if (weaponName == "Revolver") { ThisGun = GunSettings.Revolver; }
        else Debug.LogError("Invalid weapon name for display weapon: " + weaponName);


        displayText.text = ThisGun.GunName;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
