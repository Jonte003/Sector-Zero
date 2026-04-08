using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public void Run(GameObject player, List<GameObject> enemies)
    {
        if (CurrentCD > 0)
            return;

        StartCoroutine(AbilityRoutine(player, enemies));

        CurrentCD = CD;
    }

    protected virtual IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        yield return null;
    }
    public float CurrentCD { get; protected set; }
    protected abstract float CD { get; }
    public int Level { get; set; }
}

public class Explosion : Ability
{
    protected override float CD => 5f;

    private float baseRange = 0;
    private float rangePerLevel = 0;

    private float baseDamage = 0;
    private float damagePerLevel = 0;

    protected override IEnumerator AbilityRoutine(GameObject player, List<GameObject> enemies)
    {
        for (int i = 0;  i < enemies.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, enemies[i].transform.position) <= baseRange + rangePerLevel * (Level - 1))
            {
                enemies[i].GetComponent<EnemyStats>().DoDamageToEnemy(baseDamage + damagePerLevel * (Level - 1));
                // 
            }
        }

        yield return null;
    }
}