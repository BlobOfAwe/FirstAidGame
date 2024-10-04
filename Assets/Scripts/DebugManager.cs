using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    private string[] logs = new string[9];
    [SerializeField] float updateTime = 0.1f;
    private float updateCounter = 0f;
    [SerializeField] InputActionReference debugToggle;
    [SerializeField] Text logText;
    [SerializeField] Text fps;

    private void Update()
    {
        if (debugToggle.action.triggered) { ToggleDebug(); }

        // Update the FPS every updateTime seconds, based on the framerate on that Frame
        updateCounter += Time.deltaTime;
        if (updateCounter > updateTime)
        {
            fps.text = "FPS: " + Mathf.Round(1 / Time.deltaTime).ToString();
            updateCounter -= updateTime;
        }
    }
    void OnEnable()
    {
        Application.logMessageReceived += ConsoleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= ConsoleLog;
    }

    void ToggleDebug()
    {
        logText.gameObject.SetActive(!logText.gameObject.activeSelf);
        logText.transform.parent.gameObject.SetActive(!logText.transform.parent.gameObject.activeSelf);
        fps.gameObject.SetActive(!fps.gameObject.activeSelf);
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
