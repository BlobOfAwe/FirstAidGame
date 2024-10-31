using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientBehaviour : NPCDialogue
{
    // To be called by other scripts when certain conditions are met so the level can end.
    public void FixPatient()
    {
        levelManager.sceneResolved = true;
        levelManager.EndLevel();
        Debug.Log("Patient Resolved!");
    }
}
