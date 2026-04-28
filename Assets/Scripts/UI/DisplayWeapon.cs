using TMPro;
using UnityEngine;

public class DisplayWeapon : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] string weaponName;
    [SerializeField] TextMeshPro displayText;
    GunSettings thisGun;
    void Start()
    {
        if (weaponName == "Assault Rifle") { thisGun = GunSettings.AssaultRifle; }
        else if (weaponName == "Burst Rifle") { thisGun = GunSettings.BurstRifle; }
        else if (weaponName == "Shotgun") { thisGun = GunSettings.Shotgun; }
        else if (weaponName == "Pistol") { thisGun = GunSettings.Pistol; }
        else if (weaponName == "Smg") { thisGun = GunSettings.Smg; }
        else if (weaponName == "Revolver") { thisGun = GunSettings.Revolver; }
        else Debug.LogError("Invalid weapon name for display weapon: " + weaponName);


        displayText.text = thisGun.GunName;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
