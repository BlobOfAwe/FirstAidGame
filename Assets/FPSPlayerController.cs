using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class FPSPlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] float camSensitivity;
    [SerializeField] float camXClamp = 85;
    private Camera cam;
    private float camLocalXRotation;
    private int RANDOMVAR;

    [Header("Movement")]
    [SerializeField] float walkSpeed;
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
        // Rotate the player around the Y axis based on the mouse movement along the X axis, adjusted by camera sensitivity
        transform.eulerAngles += Vector3.up * lookX.action.ReadValue<float>() * camSensitivity;

        // Rotate the camera around the X axis without rotating the player.
        camLocalXRotation += -lookY.action.ReadValue<float>() * camSensitivity;
        camLocalXRotation = Mathf.Clamp(camLocalXRotation, -camXClamp, camXClamp); // Clamp the rotation to avoid issues when looking straight up or down
        cam.transform.localEulerAngles = Vector3.right * camLocalXRotation;
    }

    void Walk()
    {
        // Read the player's input to the Movement action
        Vector2 walkInput = movement.action.ReadValue<Vector2>();

        // Convert the walkInput vector to a Vector3 based in local space
        Vector3 walkLocalVector = transform.forward * walkInput.y + transform.right * walkInput.x;
        
        // Move in the local space direction specified based on the input strength and walkSpeed
        rb.velocity = (walkLocalVector * walkSpeed);
    }
}
