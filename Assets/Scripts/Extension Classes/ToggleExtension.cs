using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Extends the Toggle component class. Makes toggles display as their "Selected" colour while toggled on,
/// even when not selected using controller navigation and their "Normal" colour otherwise.
/// </summary>

[RequireComponent(typeof(Toggle))]

public class ToggleExtension : MonoBehaviour
{
    private Toggle toggle;
    private ColorBlock normalBlock;
    private ColorBlock selectedBlock;

    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        normalBlock = toggle.colors;
        selectedBlock = toggle.colors;
        selectedBlock.normalColor = toggle.colors.selectedColor;
        selectedBlock.highlightedColor = toggle.colors.selectedColor;
    }

    // Update is called once per frame
    void Update()
    {
        toggle.colors = toggle.isOn ? selectedBlock : normalBlock;
    }
}
