using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneAssessment : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform buttonContainer; // The parent object for the instantiated buttons

    private LevelManager _m;

    private void Start()
    {
        try { _m = FindFirstObjectByType<LevelManager>(); }
        catch { Debug.LogError("ERROR: No Level Manager Detected in scene."); return; }
    }

    // Triggered when the player is in SceneAssessment mode and clicks on an object. Creates a button with text corresponding to the object's name
    public void AddHazard(string objName)
    {
        // Create a temporary array of all child buttons of the buttonContainer
        Button[] buttons = buttonContainer.GetComponentsInChildren<Button>();

        // Check the array to see if the there is already a button with text matching parameter objName. If there is, do nothing.
        foreach (var button in buttons)
        {
            if (button.GetComponentInChildren<TextMeshProUGUI>().text == objName)
            {
                return;
            }
        }

        // If the passed parameter does not already have an instantiated button
        var option = Instantiate(buttonPrefab, buttonContainer); // Create a new button as a child of the buttonContainer
        option.GetComponentInChildren<TextMeshProUGUI>().text = objName; // Change its text to match the parameter objName
        option.GetComponent<Button>().onClick.AddListener(() => Destroy(option)); // Give the button a listener that will destroy it if the button is clicked
    }

    // Submits the text values of each button to the LevelManager to be evaluated
    public void SubmitForm()
    {
        Button[] buttons = buttonContainer.GetComponentsInChildren<Button>(); // An array containing all buttons under buttonContainer, each represents an identified hazard
        string[] names = new string[buttons.Length]; // An empty array of strings to be populated with the text of the buttons

        // For each button in the array, add the text to the corresponding index in names. 
        for (int i = 0; i < buttons.Length; i++)
        {
            names[i] = buttons[i].GetComponentInChildren<TextMeshProUGUI>().text;
        }

        // Pass the text values to the LevelManager to be evaluated and assigned a point value
        _m.SubmitSceneAssessment(names);
    }
}
