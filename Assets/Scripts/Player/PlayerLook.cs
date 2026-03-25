using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Mouse Look")]
    public int mouseSensitivity = 30;
    public Transform cameraObject;

    [HideInInspector] public float xRotation;
    [HideInInspector] public float yRotation;
    private float mouseX;
    private float mouseY;

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
        float mx = mouseX * Time.deltaTime * mouseSensitivity;
        float my = mouseY * Time.deltaTime * mouseSensitivity;

        xRotation -= my;
        yRotation += mx;

        xRotation = Mathf.Clamp(xRotation, -35f, 90f);

        recoilCurrent = Vector2.Lerp(recoilCurrent, recoilTarget, recoilSnapSpeed * Time.deltaTime);

        recoilTarget = Vector2.Lerp(recoilTarget, Vector2.zero, recoilReturnSpeed * Time.deltaTime);

        float finalPitch = xRotation - recoilCurrent.y;
        float finalYaw = yRotation + recoilCurrent.x;

        transform.rotation = Quaternion.Euler(0f, finalYaw, 0f);
        cameraObject.SetPositionAndRotation(transform.position, Quaternion.Euler(finalPitch, finalYaw, 0f));
    }

    void OnLook(InputValue input)
    {
        Vector2 delta = input.Get<Vector2>();
        mouseX = delta.x;
        mouseY = delta.y;
    }

    public void AddRecoil(float recoilX, float recoilY)
    {
        recoilTarget += new Vector2(recoilX, recoilY);
    }
}