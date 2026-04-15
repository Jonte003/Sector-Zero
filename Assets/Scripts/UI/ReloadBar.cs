using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    [SerializeField] private Image reloadBar;

    private PlayerShoot playerShoot;
    private Gun currentGun;
    private float reloadSpeed;
    private float reloadProgress;
    void Start()
    {
        playerShoot = GameObject.FindWithTag("Player").GetComponent<PlayerShoot>();
    }

    void Update()
    {
        currentGun = playerShoot.CurrentGun;
        reloadSpeed = currentGun.ReloadSpeed;
        reloadProgress = currentGun.ReloadProgress;
        UpdateReloadFill();
    }
    void UpdateReloadFill()
    {
        if (!currentGun.IsReloading)
        {
            reloadBar.fillAmount = 0;
            return;
        }
        reloadBar.fillAmount = reloadProgress / reloadSpeed;
    }
}