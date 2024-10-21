using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneAssessment : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform buttonContainer;

    private LevelManager _m;

    private void Start()
    {
        try { _m = FindFirstObjectByType<LevelManager>(); }
        catch { Debug.LogError("ERROR: No Level Manager Detected in scene."); return; }
    }
    public void AddHazard(string objName)
    {
        var option = Instantiate(buttonPrefab, buttonContainer);
        option.GetComponentInChildren<TextMeshProUGUI>().text = objName;
        option.GetComponent<Button>().onClick.AddListener(() => Destroy(option));
    }

    public void SubmitForm()
    {
        Button[] buttons = buttonContainer.GetComponentsInChildren<Button>();
        string[] names = new string[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            names[i] = buttons[i].GetComponentInChildren<TextMeshProUGUI>().text;
            Debug.Log("Hazard " + names[i]);
        }
        _m.SubmitSceneAssessment(names);
    }
}
