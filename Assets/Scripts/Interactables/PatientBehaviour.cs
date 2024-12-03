using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatientBehaviour : NPCDialogue
{
    [Header("ConsentCanvas")]
    [SerializeField] private Canvas consentCanvas;
    [SerializeField] private TextMeshProUGUI consentText;
    public bool consentGiven;
    private DialogueButton[] consentButtons;

    // To be called by other scripts when certain conditions are met so the level can end.
    public void FixPatient()
    {
        levelManager.sceneResolved = true;
        levelManager.EndLevel();
        Debug.Log("Patient Resolved!");
    }

    public override void OnInteract()
    {
        if (consentGiven) { base.OnInteract(); }
        else
        {
            consentButtons = consentCanvas.GetComponentsInChildren<DialogueButton>();
            consentCanvas.gameObject.SetActive(true); // Enable the canvas
            foreach (DialogueButton button in consentButtons)
            {
                button.gameObject.SetActive(button.available);
            } // Enable each of the dialogue buttons under the canvas
            consentText.text = ""; // Delete any text from the responseText object
            consentText.gameObject.SetActive(false); // Hide the responseText object
        }
    }

    public void GiveConsent(bool consenting)
    { consentGiven = consenting; }
}
