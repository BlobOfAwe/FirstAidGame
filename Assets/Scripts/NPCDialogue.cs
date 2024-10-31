using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : Interactable
{
    // References must be assigned in the inspector
    [SerializeField] GameObject dialogueButtonPrefab;
    [SerializeField] Image menuBase;
    [SerializeField] Canvas dialogueCanvas;
    [SerializeField] TextMeshProUGUI responseText; 
    public LevelManager levelManager;

    [SerializeField] float space; // Space between UI elements
    private Button[] buttons; // A list used at runtime to contain all instantiated dialogue buttons

    // Populate the list buttons by instantiating a button for each question.
    void Awake()
    {
        // Determine how many buttons there should be
        buttons = menuBase.GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            button.GetComponent<DialogueButton>().npc = this ;
        }
    }

    // Hide the dialogue canvas on Start
    private void Start()
    {
        dialogueCanvas.gameObject.SetActive(false);
    }

    // When the NPC is interacted with, open the canvas with dialogue options
    public override void OnInteract()
    {
        dialogueCanvas.gameObject.SetActive(true); // Enable the canvas
        foreach (var button in buttons) { button.gameObject.SetActive(true); } // Enable each of the dialogue buttons under the canvas
        responseText.text = ""; // Delete any text from the responseText object
        responseText.gameObject.SetActive(false); // Hide the responseText object
    }

    // When a dialogue option is selected, hide the buttons and display the appropriate response text
    public void ChooseMenuOption(string answer)
    {
        foreach (var button in buttons) { button.gameObject.SetActive(false); } // Hide all dialogue option buttons
        responseText.gameObject.SetActive(true); // Display the response text
        try { responseText.text = answer; } // Change the response text to match the corresponding answer
        catch { 
            responseText.text = "Null Response"; 
        }
    }
}
