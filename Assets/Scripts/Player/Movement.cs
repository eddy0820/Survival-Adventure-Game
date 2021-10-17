using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that handles basic movement for the player.
public class Movement : MonoBehaviour
{
    Vector2 horizontalInput;
    Rigidbody playerRigidbody;
    LayerMask groundLayerMask;
    bool jump;
    bool isGrounded;
    float currentMovementSpeed;
    [SerializeField] float walkingSpeed = 5;
    [SerializeField] float sprintingSpeed = 8;
    [SerializeField] float jumpForce = 5;
    [SerializeField] Transform groundCheck;

    private void Awake()
    {
        // Gets references.
        playerRigidbody = GetComponent<Rigidbody>();
        groundLayerMask = LayerMask.GetMask("Ground");

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
        // Calculates the velocity of the player according to the input.
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * currentMovementSpeed;
        
        // Sets the valocit of the player.
        playerRigidbody.velocity = new Vector3(horizontalVelocity.x, playerRigidbody.velocity.y, horizontalVelocity.z);
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
