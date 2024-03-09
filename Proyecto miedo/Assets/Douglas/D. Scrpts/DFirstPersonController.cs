using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;
    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;

    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float loweLookLimit = 80.0f;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    private static string AxisVertical = "Vertical";
    private static string AxisHorizontal = "Horizontal";
    private static string MouseY = "Mouse Y";
    private static string MouseX = "Mouse X";

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovementInput();
        HandleMouseInput();
        ApplyFinalMovement();
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2(walkSpeed * Input.GetAxis(AxisVertical), walkSpeed * Input.GetAxis(AxisHorizontal));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }
    private void HandleMouseInput()
    {
        rotationX -= Input.GetAxis(MouseY) * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit,loweLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0,Input.GetAxis(MouseX) * lookSpeedX, 0);

    }
    private void ApplyFinalMovement()
    {
        if(!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
