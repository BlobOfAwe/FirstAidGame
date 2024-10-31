using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueButton : MonoBehaviour
{
    [SerializeField] string question;
    [SerializeField] string answer;
    [SerializeField] int pointValue = 0;
    [SerializeField] GameObject item;
    public NPCDialogue npc;
    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        if (question != "") { GetComponentInChildren<TextMeshProUGUI>().text = question; }
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    // To be called by most dialogue buttons
    public void ChooseDialogueOption()
    {
        Debug.Log("Chose Option");
        npc.ChooseMenuOption(answer);
        levelManager.points += pointValue;

    }

    public void GiveItem()
    {
        if (item != null)
        {
            // Add the applicable item to the player's inventory
        }
    }
}
