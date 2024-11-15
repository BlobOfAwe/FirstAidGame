using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueButton : MonoBehaviour
{
    public bool available = true; //Should this button be displayed in the menu?
    [SerializeField] string question;
    [SerializeField] string answer;
    [SerializeField] int pointValue = 0;
    [SerializeField] GameObject item;
    [HideInInspector] public NPCDialogue npc;
    private LevelManager levelManager;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        if (question != "") { GetComponentInChildren<TextMeshProUGUI>().text = question; }
        levelManager = FindAnyObjectByType<LevelManager>();
        cam = Camera.main;
    }

    // To be called by most dialogue buttons
    public void ChooseDialogueOption()
    {
        levelManager.points += pointValue;
        GiveItem();
        npc.ChooseMenuOption(answer);
    }

    public void GiveItem()
    {
        if (item != null)
        {
            if (cam.transform.GetComponentInChildren<Item>() != null)
            {
                item.transform.position = cam.transform.position + transform.forward;
                item.GetComponent<Rigidbody>().isKinematic = false;
                item.gameObject.SetActive(true);
            }
            else
            {
                item.transform.parent = cam.transform.Find("HeldItem");
                item.transform.position = item.transform.parent.position;
                item.gameObject.SetActive(true);
            }
            available = false;
        }
    }

    // Exclusively used by external button events, since Unity events can't directly access the variable "available"
    public void MakeAvailable(bool availability) { available = availability; }
}
