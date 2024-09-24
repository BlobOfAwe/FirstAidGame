using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class FPSPlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Vector3 camOffset;
    [SerializeField] float camSensitivity;
    [SerializeField] float camXClamp = 85;
    private Camera cam;
    private float camLocalXRotation;
    private int RANDOMVAR;

    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintModifier = 2; // speed moving forward is multiplied by this value when sprinting
    [SerializeField] InputActionReference movement, jump, sprint, lookX, lookY;
    private Rigidbody rb;
    


    // Start is called before the first frame update
    void Start()
    {
        // Set camera and viewport settings
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;

        // Set movement variables
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    // LateUpdate is called every frame after Update
    // Any values that may have changed during Update should be evaluated here
    private void LateUpdate()
    {
        LookAtCursor();
    }

    // Update the player and camera rotations based on mouse movement
    void LookAtCursor()
    {
        // Move the Camera to the player's location
        cam.transform.position = transform.position + camOffset;
        
        // Rotate the player around the Y axis based on the mouse movement along the X axis, adjusted by camera sensitivity
        transform.eulerAngles += Vector3.up * lookX.action.ReadValue<float>() * camSensitivity;

        // Rotate the camera around the X axis without rotating the player.
        camLocalXRotation += -lookY.action.ReadValue<float>() * camSensitivity;
        camLocalXRotation = Mathf.Clamp(camLocalXRotation, -camXClamp, camXClamp); // Clamp the rotation to avoid issues when looking straight up or down
        cam.transform.eulerAngles = new Vector3 (camLocalXRotation, transform.eulerAngles.y);
    }

    void Walk()
    {
        // Read the player's input to the Movement action
        Vector2 walkInput = movement.action.ReadValue<Vector2>();
        float GetSprintMod() => sprint.action.ReadValue<float>() != 0 ? sprintModifier : 1; // If the sprint key has any input, apply the modifier value

        // Convert the walkInput vector to a Vector3 based in local space
        Vector3 walkLocalVector = transform.forward * walkInput.y * GetSprintMod() + transform.right * walkInput.x;
        
        // Move in the local space direction specified based on the input strength and walkSpeed
        rb.velocity = (walkLocalVector * walkSpeed + Vector3.up * rb.velocity.y);
    }
}
