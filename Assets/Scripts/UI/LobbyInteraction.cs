using UnityEngine;

public class LobbyInteraction : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float interactRange = 10f;
    [SerializeField] private LayerMask interactLayer;

    private IInteractable focusedInteractable;

    void Update()
    {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactRange, interactLayer))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);

            var interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != focusedInteractable)
            {
                focusedInteractable?.OnLookAway();
                focusedInteractable = interactable;
                focusedInteractable?.OnLookAt();
            }
        }
        else if (focusedInteractable != null)
        {
            focusedInteractable.OnLookAway();
            focusedInteractable = null;
        }

        if (Input.GetKeyDown(KeyCode.Z)) //Hardcoded keybinding, can be changed to more flexible later
        {
            focusedInteractable?.OnInteract();
        }
    }
}
