using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitPrimaryAssessment : MonoBehaviour
{
    public Toggle[] airway = new Toggle[3];
    public Toggle[] breathing = new Toggle[7];
    public Toggle[] circulation = new Toggle[3];

    private bool[] airwayB = new bool[3];
    private bool[] breathingB = new bool[7];
    private bool[] circulationB = new bool[3];

    // Start is called before the first frame update
    public void SubmitForm()
    {
        LevelManager _m;
        try { _m = FindFirstObjectByType<LevelManager>(); }
        catch { Debug.LogError("ERROR: No Level Manager Detected in scene."); return; }

        for (int i = 0; i < airway.Length; i++) { airwayB[i] = airway[i].isOn; }
        for (int i = 0; i < breathing.Length; i++) { breathingB[i] = breathing[i].isOn; }
        for (int i = 0; i < circulation.Length; i++) { circulationB[i] = circulation[i].isOn; }

        _m.SubmitPrimaryAssessment(airwayB, breathingB, circulationB);
        
    }
}
