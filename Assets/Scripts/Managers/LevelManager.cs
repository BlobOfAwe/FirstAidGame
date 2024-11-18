using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public bool debugEnabled;

    public int points;
    public bool sceneResolved;
    private FPSPlayerController player;
    private GameObject primaryAssessmentForm;

    [Header("Scene Assessment Key")]
    public string[] sceneAssessment;

    [Header("Primary Assessment Answer Key")]
    public  bool[] airway = new bool[3];
    public  bool[] breathing = new bool[7];
    public  bool[] circulation = new bool[3];

    [Header("Level Results UI")]
    public GameObject resultsUI;

    // Start is called before the first frame update
    void Awake()
    {
        points = 0;

        if (airway.Length > 3 || breathing.Length > 7 || circulation.Length > 3) 
            { Debug.LogError("Primary Assessment Answer Key values mismatch expected range"); }

    }

    private void Start()
    {
        player = FindAnyObjectByType<FPSPlayerController>();
        primaryAssessmentForm = FindAnyObjectByType<SubmitPrimaryAssessment>().transform.parent.gameObject;
        primaryAssessmentForm.SetActive(false);
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
        player.mode = FPSPlayerController.playerMode.secondaryAssessment;
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
        player.mode = FPSPlayerController.playerMode.primaryAssessment;
        primaryAssessmentForm.SetActive(true);
        Debug.Log("Completed Analyzing Scene Assessment submission of length " + hazards.Length + " compared to key of length " + sceneAssessment.Length);
    }

    public void EndLevel()
    {
        ref Stage levelData = ref GameManager.Instance.saveData.levels[SceneManager.GetActiveScene().buildIndex - GameManager.Instance.firstSceneBuildIndex];

        player.mode = FPSPlayerController.playerMode.paused;
        resultsUI.SetActive(true);
        resultsUI.GetComponentInChildren<Text>().text = "Level complete! You scored " + points + " points in this scenario. Your previous high score was " + levelData.highScore;
        WorldspaceUIRaycaster[] worldspaceCanvases = FindObjectsOfType<WorldspaceUIRaycaster>();
        foreach (var canvas in  worldspaceCanvases) { canvas.gameObject.SetActive(false); }

        
        if (levelData.highScore < points)
        {
            levelData.highScore = points;
        }
        GameManager.Instance.saveData.levels[SceneManager.GetActiveScene().buildIndex - GameManager.Instance.firstSceneBuildIndex + 1].available = true;

        GameManager.Instance.SaveGame();
    }

    public void AddPoint() { points++; }
    public void ReloadScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}


