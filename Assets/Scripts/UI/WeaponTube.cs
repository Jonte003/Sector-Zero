using UnityEngine;

public class WeaponTube : MonoBehaviour, IInteractable
{
    [SerializeField] private DisplayWeapon displayWeapon;
    [SerializeField] private GameObject gunPrefab;
    private GunSettings gunSettings;

    void Start()
    {
        gunSettings = displayWeapon.ThisGun;
    }
    public void OnLookAt(){}

    public void OnLookAway(){}

    public void OnInteract()
    {
        LoadoutManager.GunPrefab = gunPrefab;
        LoadoutManager.Settings = gunSettings;
    }

    void Update()
    {
        
    }
}
