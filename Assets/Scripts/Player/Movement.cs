using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Vector2 horizontalInput;
    Rigidbody playerRigidbody;
    LayerMask groundLayerMask;
    bool jump;
    bool isGrounded;
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float jumpForce = 5;
    [SerializeField] Transform groundCheck;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        MovePlayer();
        Jump(); 
    }

    private void MovePlayer()
    {
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * movementSpeed;
        playerRigidbody.velocity = new Vector3(horizontalVelocity.x, playerRigidbody.velocity.y, horizontalVelocity.z);
    }

    private void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayerMask);

        if(jump)
        {
            if(isGrounded)
            {
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            jump = false;
        }
    }

    public void OnJumpPressed()
    {
        jump = true;
    }

    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }
}
