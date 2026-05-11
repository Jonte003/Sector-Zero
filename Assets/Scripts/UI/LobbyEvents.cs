using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyEvents : MonoBehaviour
{
    [SerializeField] GameObject player;
    void Update()
    {
        if (player == null) return;
        if (player.transform.position.x >= 14)
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
