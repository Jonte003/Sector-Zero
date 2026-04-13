using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput input;

    private bool running = false;
    [SerializeField] private bool grounded = false;
    [SerializeField, Tooltip("How much faster you are when sprinting (for example 1.1 = 10% faster)")] private float runningMod = 1.25f;
    [SerializeField, Tooltip("How high you jump")] private float jumpForce = 300f;

    private Rigidbody characterRB;
    private Vector3 movementInput;
    public Vector3 movementVector;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float deceleration = 10f;
    [SerializeField] float gravity = 1;

    [SerializeField, Tooltip("The distance the player has to move to recalculate the paths of all enemies")] float distanceToRecalculateEnemies;
    [SerializeField] Controller enemyController;
    Vector3 lastCalculatedPosition;
    private float RealGravity => 9.81f * gravity;

    private float gunMoveSpeedMod = 1;

    private float FinalSpeed => movementSpeed * (running && grounded ? runningMod : 1) * (1 + playerStats.movementSpeedBuffs / 100) * gunMoveSpeedMod;


    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private PlayerStats playerStats;

    private bool CheckGrounded()
    {
        CapsuleCollider col = GetComponent<CapsuleCollider>();

        Vector3 bottom = transform.position + col.center - new Vector3(0, col.height / 2f, 0);

        return Physics.CheckSphere(bottom, groundCheckRadius, groundMask);
    }

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        characterRB = GetComponent<Rigidbody>();
        input = new();
        input.Enable();

        input.PlayerMovement.Run.performed += ctx =>
        {
            running = true;
        };

        input.PlayerMovement.Run.canceled += ctx =>
        {
            running = false;
        };

        input.PlayerMovement.Jump.performed += ctx => Jump();
        lastCalculatedPosition = Vector3.zero;
    }

    private void OnDisable()
    {
        input.Disable();


    }

    public void CalcMoveSpeed()
    {
        gunMoveSpeedMod = GetComponent<Loadout>().Gun.FinalMoveSpeed;
    }

    private void OnMovement(InputValue value)
    {
        movementInput = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }
    private void OnMovementStop(InputValue input)
    {
        movementVector = Vector3.zero;
    }

    private void Jump()
    {
        if (grounded)
        {
            characterRB.AddForce(new Vector3(0, jumpForce * (1 + playerStats.jumpHeightBuffs / 100), 0));
        }
    }

    private void Update()
    {
        grounded = CheckGrounded();
    }



    private static bool CheckIfPositionsInRange(Vector3 position1, Vector3 position2, float distance)
    {
        return (position1 - position2).sqrMagnitude < distance * distance;
    }
    void FixedUpdate()
    {
        if (movementInput != Vector3.zero)
        {
            movementVector = movementInput.x * transform.right + movementInput.z * transform.forward;
            movementVector.y = 0;
        }


        Debug.Log(characterRB == null);

        Vector3 vel = characterRB.linearVelocity;
        Vector3 horizontalVel = new Vector3(vel.x, 0, vel.z);
        Vector3 targetVel = movementVector * FinalSpeed;
        Vector3 newHorizontalVel;

        if (movementVector.sqrMagnitude > 0.01f)
            newHorizontalVel = targetVel;
        else
            newHorizontalVel = Vector3.MoveTowards(horizontalVel, Vector3.zero, deceleration * Time.fixedDeltaTime);

        float newY = vel.y - RealGravity * Time.fixedDeltaTime;

        characterRB.linearVelocity = new Vector3(newHorizontalVel.x, newY, newHorizontalVel.z);
    }
}
