using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerLook : MonoBehaviour
{
    public int mouseSensitivity = 30;
    public Transform cameraObject;
    float xRotation;
    float yRotation;
    public float mouseX;
    public float mouseY;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX *= Time.deltaTime * mouseSensitivity;
        mouseY *= Time.deltaTime * mouseSensitivity;

        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -35f, 90f);

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        cameraObject.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraObject.position = transform.position;



    }

    void OnLook(InputValue input)
    {
        mouseX = input.Get<Vector2>().x;
        mouseY = input.Get<Vector2>().y;
    }
}
