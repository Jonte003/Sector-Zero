using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerrMovement : MonoBehaviour
{
    private Rigidbody characterRB;
    private Vector3 movementInput;
    public Vector3 movementVector;
    [SerializeField] float movementSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        characterRB = GetComponent<Rigidbody>();
    }
    private void OnMovement(InputValue value)
    {
        movementInput = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }
    private void OnMovementStop(InputValue input)
    {
        movementVector = Vector3.zero;

    }
    void FixedUpdate()
    {
        if (movementInput != Vector3.zero)
        {
            movementVector = movementInput.x * transform.right + movementInput.z * transform.forward;
            movementVector.y = 0;


        }

        characterRB.AddForce(movementVector * Time.fixedDeltaTime * movementSpeed);
    }
}
