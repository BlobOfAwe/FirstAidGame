using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]

public class InteractionTest : MonoBehaviour
{
    public void LogInteraction()
    {
        Debug.Log(gameObject.name + " was interacted with.");
    }
}
