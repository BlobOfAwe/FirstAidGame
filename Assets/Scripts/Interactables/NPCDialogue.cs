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
    [SerializeField] private TextMeshProUGUI responseText;
    [SerializeField] private int maxButtonsPerPage = 4;
    public int page;
    public LevelManager levelManager;
    [SerializeField] private Button backPage;
    [SerializeField] private Button nextPage;

    [SerializeField] float space; // Space between UI elements
    private DialogueButton[] buttons; // A list used at runtime to contain all instantiated dialogue buttons

    // Populate the list buttons by instantiating a button for each question.
    void Awake()
    {
        // Determine how many buttons there should be
        buttons = menuBase.GetComponentsInChildren<DialogueButton>();
        foreach (DialogueButton button in buttons)
        {
            button.npc = this ;
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
        dialogueCanvas.gameObject.SetActive(true);
        nextPage.gameObject.SetActive(true);
        backPage.gameObject.SetActive(true);
        page = 0;
        ChangePage(0);
    }


    // When a dialogue option is selected, hide the buttons and display the appropriate response text
    public void ChooseMenuOption(string answer)
    {
        foreach (DialogueButton button in buttons) { button.gameObject.SetActive(false); } // Hide all dialogue option buttons
        nextPage.gameObject.SetActive(false);
        backPage.gameObject.SetActive(false);
        responseText.gameObject.SetActive(true); // Display the response text
        try { responseText.text = answer; } // Change the response text to match the corresponding answer
        catch { 
            responseText.text = "Null Response";
        }
    }

    public void ChangePage(int changePageBy)
    {
        int counter = 0;
        page += changePageBy;
        
        foreach (DialogueButton button in buttons)
        {
            // Disable the backPage button if on the first page, otherwise enable it
            if (page == 0) 
            { backPage.interactable = false; }
            else { backPage.interactable = true;}

            nextPage.interactable = false; // Disable the nextPage button, it will be enabled later if this is not the last page of buttons

            if (page < 0) { Debug.LogError("Page index out of range"); } // If the page is negative, throw an error
            
            // If the button is available, and the counter is on the specified page
            if (button.available && counter >= maxButtonsPerPage * page  && counter < (maxButtonsPerPage * (page + 1)))
            {
                button.gameObject.SetActive(true);
                counter++;
            }
            // If the counter is not yet at the specified page
            else if (button.available && counter < maxButtonsPerPage * page)
            {
                counter++;
                button.gameObject.SetActive(false);
            }
            // Otherwise, the counter is beyond the specified page,
            // and there are more buttons that have not yet been rendered, so enable the nextPage button
            else if (button.available)
            {
                nextPage.interactable = true;
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        } // Enable each of the dialogue buttons under the canvas
        responseText.text = ""; // Delete any text from the responseText object
        responseText.gameObject.SetActive(false); // Hide the responseText object
    }
}
