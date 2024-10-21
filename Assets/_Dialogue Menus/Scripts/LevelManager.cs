using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    [Header("Points")]
    public int points;

    [Header("Scene Assessment Key")]
    public string[] sceneAssessment;

    [Header("Primary Assessment Answer Key")]
    public  bool[] airway = new bool[3];
    public  bool[] breathing = new bool[7];
    public  bool[] circulation = new bool[3];

    // Start is called before the first frame update
    void Awake()
    {
        points = 0;

        if (airway.Length > 3 || breathing.Length > 7 || circulation.Length > 3) 
            { Debug.LogError("Primary Assessment Answer Key values mismatch expected range"); }

    }

    public void SubmitPrimaryAssessment(bool[] a, bool[] b, bool[] c)
    { 
        if (Enumerable.SequenceEqual(a, airway)) 
        { points++; }
        if (Enumerable.SequenceEqual(b, breathing)) 
        { points++; }
        if (Enumerable.SequenceEqual(c, circulation)) 
        { points++; }
        Debug.Log("Submitted Primary Assessment with " + points + " points");
    }

    // Add points for each item both in hazards and in the sceneAssessmentKey
    public void SubmitSceneAssessment(string[] hazards)
    {
        // For each item in the answer key
        for (int i = 0; i < sceneAssessment.Length; i++)
        {
            // If there are no further items in the answer key, break the loop
            if (sceneAssessment[i] == null) { break; }

            // For each item in the hazard list, check to see if the hazard matches with an item in the scene key
            for (int j = 0; j < hazards.Length; j++)
            {
                if (sceneAssessment[i] == hazards[j]) { points++; }
                Debug.Log("Compared: " + sceneAssessment[i] + " + " + hazards[j]);
            }
        }

        Debug.Log("Completed Analyzing Scene Assessment submission of length " + hazards.Length + " compared to key of length " + sceneAssessment.Length);
    }

    
}


