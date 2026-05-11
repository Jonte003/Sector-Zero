using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] Color colorClosed;
    [SerializeField] Color colorOpen;
    bool isClosed = true;
    Renderer barrierRenderer;
    Collider barrierCollider;
    [SerializeField] int doorID;
    [SerializeField] LobbyAbilityManager lobbyAbilityManager;
    void Start()
    {
        barrierRenderer = GetComponent<Renderer>();
        barrierCollider = GetComponent<Collider>();
        UpdateState();
    }

    void Update()
    {
        #region Door 1
        if (doorID == 1)
        {
            if (LoadoutManager.Settings == null) return;
            if (LoadoutManager.Settings.GunName != "None")
            {
                isClosed = false;
            }
            else
            {
                isClosed = true;
            }
            UpdateState();
        }
        #endregion
        #region Door 2
        if (doorID == 2)
        {
            if (LoadoutManager.Settings == null || lobbyAbilityManager == null) return;
            if (LoadoutManager.Settings.GunName != "None" && lobbyAbilityManager.CorrectAbilityCount())
            {
                isClosed = false;
            }
            else
            {
                isClosed = true;
            }
            UpdateState();
        }
        #endregion
    }

    void UpdateState()
    {
        if (isClosed)
        {
            barrierRenderer.material.color = colorClosed;
            barrierCollider.enabled = true;
        }
        else
        {
            barrierRenderer.material.color = colorOpen;
            barrierCollider.enabled = false;
        }
    }
}
