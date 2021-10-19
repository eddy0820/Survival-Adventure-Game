using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// Class that handles basic movement for the player.
public class Movement : MonoBehaviour
{
    [SerializeField] Transform groundCheck;
    [SerializeField] float walkingSpeed = 5;
    [SerializeField] float sprintingSpeed = 8;
    [SerializeField] float jumpForce = 5;
    [SerializeField] float turnSmoothTime = 0.1f;
    Vector2 horizontalInput;
    Rigidbody playerRigidbody;
    LayerMask groundLayerMask;
    CinemachineFreeLook thirdPersonCamera;
    CinemachineVirtualCamera firstPersonCamera;
    bool jump;
    bool isGrounded;
    float currentMovementSpeed;
    float turnSmoothVelocity;
    
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        groundLayerMask = LayerMask.GetMask("Ground");
        thirdPersonCamera = GetComponentInChildren<CinemachineFreeLook>();
        firstPersonCamera = GetComponentInChildren<CinemachineVirtualCamera>();

        // Assigns the walk speed as the current speed by default;
        currentMovementSpeed = walkingSpeed;
    }

    private void Update()
    {
        CheckSurroundings();
        MovePlayer();
        Jump();
    }

    private void CheckSurroundings()
    {
        //Checks to see if the player is on the ground according to the LayerMask.
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayerMask);
    }

    private void MovePlayer()
    {
        // If using the third person camera, use third person movement.
        if(thirdPersonCamera.Priority == 1)
        {
            ThirdPersonMovement();
        }
        //If using the first person camera, use first person movement.
        else if(firstPersonCamera.Priority == 1)
        {
            FirstPersonMovement();
        }
    }

    private void FirstPersonMovement()
    {
        // Calculates the velocity of the player according to the input.
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * currentMovementSpeed;
        
        // Sets the velocity of the player.
        playerRigidbody.velocity = new Vector3(horizontalVelocity.x, playerRigidbody.velocity.y, horizontalVelocity.z);
    }

    private void ThirdPersonMovement()
    {
        // Gets the transforms from the camera and changes the y to 0 and normalizes them.
        Vector3 cameraRight = Camera.main.transform.right;
        Vector3 cameraForward = Camera.main.transform.forward;

        cameraRight.y = 0;
        cameraForward.y = 0;

        cameraRight.Normalize();
        cameraForward.Normalize();

        // Calculates the velocity of the player according to the input and camera transforms.
        Vector3 horizontalVelocity = (cameraRight * horizontalInput.x + cameraForward * horizontalInput.y) * currentMovementSpeed;
        
        // Sets the velocity of the player.
        playerRigidbody.velocity = new Vector3(horizontalVelocity.x, playerRigidbody.velocity.y, horizontalVelocity.z);
        
        // If the player is moving calculate the turn angles and set them.
        if(horizontalVelocity != new Vector3(0, 0, 0))
        {
            // Gets the angle.
            float targetAngle = Mathf.Atan2(horizontalVelocity.x, horizontalVelocity.z) * Mathf.Rad2Deg;
            // Smooths the angle.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            // Sets the angle.
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void Jump()
    {
        if(jump)
        {
            if(isGrounded)
            {   
                // If both the jump button has been pressed & 
                // the player is on the ground, makes the player jump.
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            // Disables the ability to jump.
            jump = false;
        }
    }

    // Sets the ability to jump to be true of the button was pressed in the InputManager.
    public void OnJumpPressed()
    {
        jump = true;
    }

    // Changes the player's movement speed according to whether the button was pressed in the InputManager.
    public void OnSprintPressed(bool sprint)
    {
        if(sprint)
        {
            currentMovementSpeed = sprintingSpeed;
        }
        else
        {
            currentMovementSpeed = walkingSpeed;
        }
    }

    // Receives the input for horizontal movement from the InputManager.
    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }
}
