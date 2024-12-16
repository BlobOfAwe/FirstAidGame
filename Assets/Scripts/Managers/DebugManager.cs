using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    [Header("Console Logs")]
    [SerializeField] Text logText; // The text body that displays Debug.Logs
    private string[] logs = new string[9]; // logs.length determnes how many logs will be displayed at a time

    [Header("FPS Monitor")]
    [SerializeField] float updateTime = 0.1f; // How often (in seconds) does the game check certain values, such as FPS
    private float updateCounter = 0f; // used as a dynamic variable to time updateTime
    [SerializeField] Text fps; // Displays the current FPS. Updated every updateTime-seconds

    [Header("Other")]
    [SerializeField] InputActionReference debugToggle; // Reference to the input action that toggles the debug canvas
    private LevelManager levelManager;

    void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    // Update is called every frame
    private void Update()
    {
        // If the toggle input is triggered that frame, toggle the Debug Canvas
        //if (debugToggle.action.triggered) { ToggleDebug(); }

        // Update the FPS every updateTime seconds, based on the framerate on that Frame
        updateCounter += Time.deltaTime; // Increase the timer each frame
        // If that causes the timer to exceed the updateTime, update displayed FPS and reset the timer
        if (updateCounter > updateTime)
        {
            fps.text = "FPS: " + Mathf.Round(1 / Time.deltaTime).ToString();
            updateCounter -= updateTime;
        }
    }


    void OnEnable() { Application.logMessageReceived += ConsoleLog; }
    void OnDisable() { Application.logMessageReceived -= ConsoleLog; }

    // When the debug toggle input is triggered, toggle the visibility of the Debug elements
    void ToggleDebug()
    {
        levelManager.debugEnabled = !levelManager.debugEnabled;

        logText.gameObject.SetActive(levelManager.debugEnabled);
        logText.transform.parent.gameObject.SetActive(levelManager.debugEnabled);
        fps.gameObject.SetActive(levelManager.debugEnabled);
    }

    void ConsoleLog(string logString, string stackTrace, LogType type)
    {
        // If the last unit in the array has already been written to...
        if (logs[logs.Length - 1] != null)
        {
            // Move every value in the array up by one index, 
            for (int i = 0; i < logs.Length - 1; i++)
            {
                logs[i] = logs[i + 1];
            }

            // replacing the last value with the logString
            logs[logs.Length - 1] = logString;
        }
        
        // If there is at least one empty value in the array...
        else
        {
            // Add the logstring to the first null index
            for (int i = 0; i < logs.Length; i++)
            {
                {
                    if (logs[i] == null) { logs[i] = logString; break; }
                }
            }
        }

        // Update the Debugger text
        string textBodyString = "";
        foreach (string log in logs) { if (log != null) { textBodyString += "> " + log + "\n"; } }
        logText.text = textBodyString;
    }

    
}
