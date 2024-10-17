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

    [SerializeField] float space; // Space between UI elements
    public string[] questions;
    public string[] answers;
    private GameObject[] buttons; // A list used at runtime to contain all instantiated dialogue buttons

    // Populate the list buttons by instantiating a button for each question.
    void Awake()
    {
        // Determine how many buttons there should be
        buttons = new GameObject[questions.Length];

        // For each question...
        for(int i = 0; i < questions.Length; i++)
        {
            // Create a button with the question text
            var option = Instantiate(dialogueButtonPrefab, menuBase.transform);
            option.GetComponentInChildren<TextMeshProUGUI>().text = questions[i];

            // Tell the button to call the ChooseMenuOption function for the appropriate index
            int index = i; // This creates a local variable with value i so that the following lamda expression catches only the variable instance with the desired value
            option.GetComponent<Button>().onClick.AddListener(() => ChooseMenuOption(index));
            
            // Add the new button to the list
            buttons[i] = option;
        }
    }

    private void Start()
    {
        dialogueCanvas = GetComponentInChildren<Canvas>();
        dialogueCanvas.gameObject.SetActive(false);
    }

    // When the NPC is interacted with, open the canvas with dialogue options
    public override void OnInteract()
    {
        dialogueCanvas.gameObject.SetActive(true);
        foreach (var button in buttons) { button.SetActive(true); }
        responseText.text = "";
        responseText.gameObject.SetActive(false);
    }

    // When a dialogue option is selected, hide the buttons and display the appropriate response text
    public void ChooseMenuOption(int index)
    {
        foreach (var button in buttons) { button.SetActive(false); }
        responseText.gameObject.SetActive(true);
        try { responseText.text = answers[index]; }
        catch { 
            Debug.LogError("No corresponding answer for Object " + gameObject.name + " question" + index + ": " + questions[index]); 
            responseText.text = "Null Response"; 
        }
    }
}
