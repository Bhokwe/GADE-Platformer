using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // reference for the player's inptu action and movement
    PlayerInput playerInput;
    InputAction lookAction;

    [Header("Look Settings")]
    public float mouseSens = 20f; //mouse sensitivity for looking around
    public Transform cameraTarget; //slot for the cameraTarget empty in the player
    float xRotation = 0f; //up&down tilt of the camera
    Vector2 lookInput;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
             

    }

    // Update is called once per frame
    void Update()
    {
       float mouseX = lookInput.x * mouseSens * Time.deltaTime;
       float mouseY = lookInput.y * mouseSens * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20f, 20f);
        cameraTarget.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
    void PlayerLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        transform.Rotate(Vector3.up * lookInput.x * mouseSens * Time.deltaTime);

        xRotation -= lookInput.y * mouseSens * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -20f, 20f);
        cameraTarget.localRotation = Quaternion.Euler(xRotation, 0, 0);

        /*if (lookInput.sqrMagnitude > 0)
        {
            
        }*/   
    }
}
