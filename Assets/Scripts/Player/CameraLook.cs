using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] float sensitivityX = 5;
    [SerializeField] float sensitivityY= 5;
    [SerializeField] float xClamp = 85f;
    [SerializeField] Transform playerCamera;
    float mouseX, mouseY, xRotation = 0f;

    private void Update()
    {
        // Rotates the player towards where they are looking.
        transform.Rotate(Vector3.up, mouseX * Time.deltaTime);

        // Gets the y rotation for the camera from the input.
        xRotation -= mouseY;

        // Makes sure the camera's y rotation doesn't go too high or low.
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);

        // Sets the camera's y rotation.
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
    }

    // Receives the input for mouse movement from the InputManager
    // and multiplies them by the sensitivity.
    public void ReceiveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;
    }
}
