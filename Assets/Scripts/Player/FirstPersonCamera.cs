using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float sensitivityX = 5;
    [SerializeField] float sensitivityY= 5;
    [SerializeField] float xClamp = 85f;
    float mouseX, mouseY, xRotation = 0;
    bool toggle;
    
    private void Update()
    {
        // Gets the y rotation for the camera from the input.
        xRotation -= mouseY;

        // Makes sure the camera's y rotation doesn't go too high or low.
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);

        // Sets the camera's y rotation.
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            
        // Rotates the player towards where they are looking.
        playerTransform.Rotate(Vector3.up, mouseX * Time.deltaTime);
    }

    // Receives the input for mouse movement from the InputManager
    // and multiplies them by the sensitivity.
    public void ReceiveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;
    }
}
