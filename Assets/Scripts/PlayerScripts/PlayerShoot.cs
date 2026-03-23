using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Gun Gun;

    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = new();
        playerInput.PlayerActions.Shoot.performed += ctx => Shoot();
    }

    private void Shoot()
    {
        Debug.Log("Shoot");
        Gun.TryShoot();
    }
}