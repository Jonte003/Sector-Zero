using UnityEngine;

public class Loadout : MonoBehaviour
{
    public Gun Gun { get; private set; }
    public GunMods[] GunMods { get; private set; }
    public Ability Ability1 { get; private set; }
    public Ability Ability2 { get; private set; }

    public void UpdateCooldowns()
    {
        Ability1.CurrentCD = Mathf.Max(0, Ability1.CurrentCD - Time.deltaTime);
        Ability2.CurrentCD = Mathf.Max(0, Ability1.CurrentCD - Time.deltaTime);
    }
}