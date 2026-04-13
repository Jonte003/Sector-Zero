using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private Gun gun;
    public Gun CurrentGun => gun;

    private PlayerInput playerInput;

    private bool isShooting = false;

    private void Start()
    {
        playerInput = new();
        playerInput.PlayerActions.Enable();


        playerInput.PlayerActions.Reload.performed += ctx => Reload();

        playerInput.PlayerActions.Shoot.performed += ctx =>
        {
            if (gun.settings.FullAuto)
                isShooting = true;
            else
                Shoot();
        };

        playerInput.PlayerActions.Shoot.canceled += ctx =>
        {
            isShooting = false;
        };
    }

    public void SetGun()
    {
        gun = GetComponent<Loadout>().Gun;
    }

    private void OnDisable()
    {
        playerInput.PlayerActions.Disable();
    }

    private void Update()
    {
        if (isShooting && gun.CanShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Debug.Log("Shoot, Ammo: "  + gun.CurrentAmmo());
        gun.TryShoot();
    }

    private void Reload()
    {
        Debug.Log(gun.settings.ReloadSpeed);
        gun.TryReload();
    }
}