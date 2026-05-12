using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadout : MonoBehaviour
{
    public Gun Gun { get; set; }
    public GunMod[] GunMods { get; private set; }
    public Ability Ability1 { get;  set; }
    public Ability Ability2 { get;  set; }
    public Ability Ability3 { get;  set; }
    public Ability[] AbilitiesInBag { get; set; }

    public void UpdateCooldowns()
    {
        if (Ability1 != null)
        {
            Ability1.CurrentCD = Mathf.Max(0, Ability1.CurrentCD - Time.deltaTime);
        }
        if (Ability2 != null)
        {
            Ability2.CurrentCD = Mathf.Max(0, Ability2.CurrentCD - Time.deltaTime);
        }
        if (Ability3 != null)
        {
            Ability3.CurrentCD = Mathf.Max(0, Ability3.CurrentCD - Time.deltaTime);
        }
    }

    public Ability[] GetRandomAbilities(int amount)
    {
        AbilityManager am = transform.Find("AbilityManager").GetComponent<AbilityManager>();

        List<Ability> pool = new List<Ability>(am.abilities);

        Loadout loadout = GetComponent<Loadout>();

        pool.Remove(loadout.Ability1);
        pool.Remove(loadout.Ability2);
        pool.Remove(loadout.Ability3);

        Ability[] result = new Ability[amount];

        for (int i = 0; i < amount; i++)
        {
            if (pool.Count == 0)
            {
                Debug.LogError("Not enough abilities in pool to pick from!");
                break;
            }

            int index = Random.Range(0, pool.Count);
            result[i] = pool[index];
            pool.RemoveAt(index);
        }

        return result;
    }


    private void Awake()
    {
        Transform parent = transform.Find("Camera");

        if (SceneManager.GetActiveScene().name == "Gameplay")
        {

            GameObject gun = Instantiate(LoadoutManager.GunPrefab, parent);
            gun.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            Gun = gun.GetComponent<Gun>();
            GunMods = LoadoutManager.GunMods;

            gun.GetComponent<Gun>().gunMods = GunMods;
            gun.GetComponent<Gun>().settings = LoadoutManager.Settings;

            var abilityManager = transform.Find("AbilityManager").GetComponent<AbilityManager>();
            abilityManager.InitializeAbilities(LoadoutManager.AbilityTypesInBag);

            GetComponent<PlayerShoot>().SetGun();
        }
    }

    private void Update()
    {
        UpdateCooldowns();
    }
}