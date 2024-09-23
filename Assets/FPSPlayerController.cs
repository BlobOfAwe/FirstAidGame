using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // Used to manage player's movement

public class FPSPlayerController : MonoBehaviour
{
    [Header("Camera")]
    private Camera cam;
    [SerializeField] float camSensitivity;
    [SerializeField] float camXClamp = 85;
    private float camLocalXRotation;

    [Header("Movement")]
    private Rigidbody rb;
    private CharacterController characterController;
    [SerializeField] float walkSpeed;


    // Start is called before the first frame update
    void Start()
    {
        // Set camera and viewport settings
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;

        // Set movement variables
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();

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
        transform.eulerAngles += Vector3.up * Input.GetAxis("Mouse X") * camSensitivity;

        // Rotate the camera around the X axis without rotating the player.
        camLocalXRotation += -Input.GetAxis("Mouse Y") * camSensitivity;
        camLocalXRotation = Mathf.Clamp(camLocalXRotation, -camXClamp, camXClamp); // Clamp the rotation to avoid issues when looking straight up or down
        cam.transform.localEulerAngles = Vector3.right * camLocalXRotation;
    }

    void Walk()
    {
        
        characterController.SimpleMove(Vector3.forward * walkSpeed);
        //rb.velocity = new Vector3(Input.GetAxis("Horizontal") * walkSpeed, rb.velocity.y, Input.GetAxis("Vertical") * walkSpeed);
    }
}
