using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPromptText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.transform.Find("HeldItem").childCount != 0) 
        {
            text.enabled = true;
        }
        else
        {
            text.enabled = false;
        }
    }
}
