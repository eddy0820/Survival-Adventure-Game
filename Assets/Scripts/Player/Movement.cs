using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using SurvivalAdventureGame.Stats;

// Class that handles basic movement for the player.
public class Movement : MonoBehaviour
{
    public bool startedStaminaReductionCoroutine;
    public bool startedStaminaRegenCoroutine;
    public bool isSprinting;
    [SerializeField] Transform groundCheck;
    Vector2 horizontalInput;
    Rigidbody playerRigidbody;
    LayerMask groundLayerMask;
    CinemachineFreeLook thirdPersonCamera;
    CinemachineVirtualCamera firstPersonCamera;
    CharacterStat playerStats;
    StatModifier sprintingModifier;
    bool jump;
    bool isGrounded;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        groundLayerMask = LayerMask.GetMask("Ground");
        thirdPersonCamera = GetComponentInChildren<CinemachineFreeLook>();
        firstPersonCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        playerStats = GetComponent<CharacterStat>();
    }

    private void Start()
    {
        // Caches the sprinting modifier after its initialized in PlayerStat
        sprintingModifier = new StatModifier(playerStats.CharacterStats["SprintMult"].value, StatModifierTypes.PercentMult);
    }

    private void Update()
    {
        CheckSurroundings();
        MovePlayer();
        Jump();
        ReduceStamina();
        RegenStamina();
    }

    private void CheckSurroundings()
    {
        // Checks to see if the player is on the ground according to the LayerMask.
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
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * playerStats.CharacterStats["WalkSpeed"].value;
        
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
        Vector3 horizontalVelocity = (cameraRight * horizontalInput.x + cameraForward * horizontalInput.y) * playerStats.CharacterStats["WalkSpeed"].value;
        
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
            if(isGrounded && playerStats.CurrentStamina >= playerStats.CharacterStats["JumpStaminaCost"].value)
            {   
                // If  the jump button has been pressed & 
                // the player is on the ground & has enough stamina, makes the player jump.
                playerRigidbody.AddForce(Vector3.up * playerStats.CharacterStats["JumpForce"].value, ForceMode.Impulse);

                // Reduces the stamina by x amount.
                playerStats.ReduceStaminaJumping(); 
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
        isSprinting = sprint;

        if(sprint && playerStats.CurrentStamina > 0)
        {
            playerStats.CharacterStats["WalkSpeed"].AddModifier(sprintingModifier);
        }
        else
        {
            playerStats.CharacterStats["WalkSpeed"].RemoveModifier(sprintingModifier);
        }
    }

    // Reduces stamina while sprinting by x amount every second.
    private void ReduceStamina()
    {
        if(!startedStaminaReductionCoroutine)
        {
            StartCoroutine(playerStats.ReduceStaminaSprinting());
        }

        if(playerStats.CurrentStamina <= 0)
        {
            // Takes away modifier even if you run out of stamina and are still holding the sprint key.
            playerStats.CharacterStats["WalkSpeed"].RemoveModifier(sprintingModifier);
        }
    }
    
    // Regens stamina x amount every second.
    private void RegenStamina()
    {
        if(!startedStaminaRegenCoroutine)
        {
            StartCoroutine(playerStats.RegenStamina());
        }
    }

    // Used by other classes to see if the player is moving.
    public Vector2 GetHorizontalInput()
    {
        return horizontalInput;
    }

    // Receives the input for horizontal movement from the InputManager.
    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }
}
