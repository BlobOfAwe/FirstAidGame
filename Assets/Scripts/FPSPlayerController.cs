using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FPSPlayerController : MonoBehaviour
{
    public enum playerMode { sceneAssessment, primaryAssessment, secondaryAssessment, paused }
    private LevelManager levelManager;

    [Header("Mode")]
    public playerMode mode;

    [Header("Camera")]
    [SerializeField] Vector3 camOffset;
    [SerializeField] float camSensitivity;
    [SerializeField] float camXClamp = 85;
    public Camera cam;
    private float camLocalXRotation;
    private int RANDOMVAR;

    [Header("Interaction")]
    private SceneAssessment sceneAssessment;
    [SerializeField] float interactRange = 5f;
    [SerializeField] LayerMask interactables;
    private Transform activeHighlight = null;

    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintModifier = 2; // speed moving forward is multiplied by this value when sprinting
    [SerializeField] InputActionReference movement, jump, sprint, lookX, lookY, interact;
    private Rigidbody rb;
    


    // Start is called before the first frame update
    void Start()
    {
        try { sceneAssessment = FindAnyObjectByType<SceneAssessment>(); } 
        catch { Debug.LogError("No sceneAssessment object found in scene!"); }
        
        // Set camera and viewport settings
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;

        // Set movement variables
        rb = GetComponent<Rigidbody>();

        levelManager = FindAnyObjectByType<LevelManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (mode != playerMode.paused && mode != playerMode.sceneAssessment) { Walk(); }
        
        if (interact.action.triggered) 
        { 
            if (mode == playerMode.sceneAssessment) { IdentifyHazard(); }
            else if (mode == playerMode.secondaryAssessment) { Interact(); }
        }

        if (jump.action.triggered && levelManager.debugEnabled) { DebugFixPatient(); }

        SeekInteractable();
    }

    // LateUpdate is called every frame after Update
    // Any values that may have changed during Update should be evaluated here
    private void LateUpdate()
    {
        if (mode != playerMode.paused) { LookAtCursor(); }
        else { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; };
    }

    // Raycast for an interactable object and invoke it's onInteract Method
    void Interact()
    {
        // Raycast out to interactRange and look for interactable objects.
        // If one is found, call the OnInteract() function
        // -----**NOTE: all interactable objects MUST be on the Interactable Layer, and have the a class that inherits from Interactable.cs**-----
        RaycastHit hit;
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange, interactables))
        {
            hit.collider.GetComponent<Interactable>().OnInteract();
        }
        else { Debug.LogWarning("No Raycast Target Found."); }
    }

    void IdentifyHazard()
    {
        // Raycast out to interactRange and look for any gameObject.
        // If one is found, add it to the sceneAssessment hazard list
        RaycastHit hit;
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            sceneAssessment.AddHazard(hit.collider.gameObject.name);
            Debug.Log("hit");
        }
        else { Debug.LogWarning("No Raycast Target Found."); }
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

    void SeekInteractable()
    {
        // Raycast out to interactRange and look for interactable objects.
        // If one is found, find a child object called Highlight and enable it to highlight the object
        // -----**NOTE: all interactable objects MUST be on the Interactable Layer, and have the a class that inherits from Interactable.cs**-----
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange, interactables))
        {
            if (hit.collider.transform.Find("Highlight") != activeHighlight && activeHighlight != null)
            {
                activeHighlight.gameObject.SetActive(false);
            }

            activeHighlight = hit.collider.transform.Find("Highlight");
            activeHighlight.gameObject.SetActive(true);
        }
        else if (activeHighlight != null)
        { 
            if (activeHighlight.gameObject.activeSelf)
            {
                activeHighlight.gameObject.SetActive(false);
                activeHighlight = null;
            }
        }
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

    // A bypass function that should only be called for developer purposes while debug is enabled.
    // Automatically resolves the scene and markes the patient as fixed
    void DebugFixPatient()
    {
        // Raycast out to interactRange and look for interactable objects.
        // If one is found, check for the PatientBehaviour class and call FixPatient, throwing a warning if the class is not found
        // -----**NOTE: all interactable objects MUST be on the Interactable Layer, and have the a class that inherits from Interactable.cs**-----
        RaycastHit hit;
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange, interactables))
        {
            hit.collider.GetComponent<PatientBehaviour>().FixPatient();
            //try { hit.collider.GetComponent<PatientBehaviour>().FixPatient(); }
            //catch { Debug.LogWarning("Targeted object " + hit.collider.gameObject.name + " contains no type of PatientBehaviour"); }
        }
        else { Debug.LogWarning("No Raycast Target Found."); }
    }
}
