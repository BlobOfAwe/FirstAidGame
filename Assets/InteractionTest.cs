using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTest : Interactable
{
    public override void OnInteract()
    {
        Debug.Log(gameObject.name + " was interacted with.");
    }
}
