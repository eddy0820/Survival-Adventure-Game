using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gets and manages inputs from the new unity input system
// and then sends them to other classes that require those inputs.
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
        // Gets movement and camera control components.
        movement = GetComponent<Movement>();
        cameraLook = GetComponent<CameraLook>();

        // Gets the controls from the new unity input system.
        controls = new PlayerControls();
        groundMovement = controls.GroundMovement;

        // Gets the value from Horizontal Movement.
        groundMovement.HorizontalMovement.performed += ctx => 
            horizontalInput = ctx.ReadValue<Vector2>();
        
        // Gets the value of Jump and calls a function to enable a jump.
        groundMovement.Jump.performed += _ => 
            movement.OnJumpPressed();

        // Gets the value of Mouse movements x and y.
        groundMovement.MouseX.performed += ctx =>
            mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx =>
            mouseInput.y = ctx.ReadValue<float>();

        // Gets the value of Sprint and calls a function to enable or disable sprint.
        groundMovement.Sprint.performed += _ =>
            movement.OnSprintPressed(true);
        groundMovement.Sprint.canceled += _ =>
            movement.OnSprintPressed(false);
    }

    private void Update()
    {
        // Sends the input to their respective classes.
        movement.ReceiveInput(horizontalInput);
        cameraLook.ReceiveInput(mouseInput);
    }
    
    //Enables controls.
    private void OnEnable()
    {
        controls.Enable();
    }

    //Disables controls.
    private void OnDisable()
    {
        controls.Disable();
    }
}
