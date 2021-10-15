using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls controls;
    PlayerControls.GroundMovementActions groundMovement;
    Movement movement;
    CameraLook cameraLook;
    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        cameraLook = GetComponent<CameraLook>();

        controls = new PlayerControls();
        groundMovement = controls.GroundMovement;

        groundMovement.HorizontalMovement.performed += ctx => 
            horizontalInput = ctx.ReadValue<Vector2>();
        
        groundMovement.Jump.performed += _ => 
            movement.OnJumpPressed();

        groundMovement.MouseX.performed += ctx =>
            mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx =>
            mouseInput.y = ctx.ReadValue<float>();
    }

    private void Update()
    {
        movement.ReceiveInput(horizontalInput);
        cameraLook.ReceiveInput(mouseInput);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
