using UnityEngine;

public class EruptionAbility : MonoBehaviour
{
    float timer = 1.5f;
    GameObject player;
    float size;
    GameObject sphere;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            player.GetComponent<ParticleManager>().PlayExplosionEffect(transform.position, size);
            Destroy(sphere);
            Destroy(gameObject);
            
            
        }
    }

    public void StartCountdown(GameObject player, float time, float explosionScale,GameObject growingSphere)
    {
        sphere = Instantiate(growingSphere, transform.position, Quaternion.identity);
        timer = time;
        this.player = player;
        size = explosionScale;
    }
}
