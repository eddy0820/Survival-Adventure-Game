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
    FirstPersonCamera firstPersonCamera;
    Vector2 horizontalInput;
    Vector2 mouseInput;
    CameraSwitch cameraSwitch;
    PlayerUIController playerUI;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        firstPersonCamera = GetComponentInChildren<FirstPersonCamera>();
        cameraSwitch = GetComponent<CameraSwitch>();
        playerUI = GetComponentInChildren<PlayerUIController>();

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

        // Gets the value of SwitchCamera and calls a function to toggle the camera perspective.
        groundMovement.SwitchCamera.performed += _ =>
            cameraSwitch.OnSwitchCameraPressed();

        //Gets the value of ToggleHUD and calls a function to toggle the HUD.
        groundMovement.ToggleHUD.performed += _ =>
            playerUI.OnToggleHUDPressed();

    }

    private void Update()
    {
        // Sends the input to their respective classes.
        movement.ReceiveInput(horizontalInput);
        firstPersonCamera.ReceiveInput(mouseInput);
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
