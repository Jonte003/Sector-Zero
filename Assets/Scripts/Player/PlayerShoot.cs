using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Gun Gun;

    private PlayerInput playerInput;

    private bool isShooting = false;

    private void Start()
    {
        playerInput = new();
        playerInput.PlayerActions.Enable();


        playerInput.PlayerActions.Reload.performed += ctx => Reload();

        playerInput.PlayerActions.Shoot.performed += ctx =>
        {
            if (Gun.FullAuto)
                isShooting = true;
            else
                Shoot();
        };

        playerInput.PlayerActions.Shoot.canceled += ctx =>
        {
            isShooting = false;
        };
    }

    private void OnDisable()
    {
        playerInput.PlayerActions.Disable();
    }

    private void Update()
    {
        if (isShooting && Gun.CanShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Debug.Log("Shoot, Ammo: "  + Gun.CurrentAmmo());
        Gun.TryShoot();
    }

    private void Reload()
    {
        Debug.Log("Reload");
        Gun.TryReload();
    }
}