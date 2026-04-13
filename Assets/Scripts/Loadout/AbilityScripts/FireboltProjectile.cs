using UnityEngine;

public class FireboltProjectile : MonoBehaviour
{
    private float damage;

    public void Initialize(float dmg)
    {
        damage = dmg;
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Enemy"))
        {
            return;
        }

        EnemyStats enemy = collision.collider.GetComponent<EnemyStats>();
        if (enemy != null)
        {
            enemy.DoDamageToEnemy(damage);
        }

        Destroy(gameObject);
    }
}