using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Mouse Look")]
    public float mouseSensitivity;
    public Transform cameraObject;

    [HideInInspector] public float xRotation;
    [HideInInspector] public float yRotation;

    [Header("Recoil Settings")]
    [SerializeField] private float recoilSnapSpeed = 25f;
    [SerializeField] private float recoilReturnSpeed = 10f;

    private Vector2 recoilCurrent;
    private Vector2 recoilTarget;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 delta = Pause.IsPaused ? Vector2.zero : Mouse.current.delta.ReadValue();

        float mx = delta.x * mouseSensitivity;
        float my = delta.y * mouseSensitivity;

        xRotation -= my;
        yRotation += mx;

        xRotation = Mathf.Clamp(xRotation, -85f, 90f);

        recoilCurrent = Vector2.Lerp(recoilCurrent, recoilTarget, recoilSnapSpeed * Time.deltaTime);
        recoilTarget = Vector2.Lerp(recoilTarget, Vector2.zero, recoilReturnSpeed * Time.deltaTime);

        float finalPitch = xRotation - recoilCurrent.y;
        float finalYaw = yRotation + recoilCurrent.x;

        transform.rotation = Quaternion.Euler(0f, finalYaw, 0f);
        cameraObject.localRotation = Quaternion.Euler(finalPitch, 0f, 0f);
    }

    public void AddRecoil(float recoilX, float recoilY)
    {
        recoilTarget += new Vector2(recoilX, recoilY);
    }
}