using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Givable : Item
{
    [SerializeField] private GameObject giveTo;
    [SerializeField] private string response;
    [SerializeField] public int points;
    public override void ItemEffect(GameObject target)
    {
        
        // If the target is the designated NPC to give it to, add points and destroy the game object
        if (target == giveTo || giveTo == null)
        {
            NPCDialogue dialogue = target.GetComponentInChildren<NPCDialogue>();
            dialogue.OnInteract();
            dialogue.ChooseMenuOption(response);
            levelManager.points += points;
            Destroy(gameObject);
        }
    }
}
