using UnityEngine;
using UnityEngine.UI;

public class ShootBar : MonoBehaviour
{
    [SerializeField] Transform barSize;
    [SerializeField] Transform barFill;
    [SerializeField] Color colorOn;
    [SerializeField] Color colorOff;
    Gun gun;
    void Start()
    {
        gun = GameObject.FindWithTag("Player").GetComponentInChildren<Gun>();
    }

    void Update()
    {
        barFill.localScale = new Vector3(gun.FireCooldownPercent, 1f, 1f);
        if (gun.CanShoot) barFill.localScale = new Vector3(1f, 1f, 1f);
        if (gun.CanShoot)
        {
            barFill.GetComponent<Image>().color = colorOn;
        }
        else
        {
            barFill.GetComponent<Image>().color = colorOff;
            
        }
    }
}
