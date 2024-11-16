using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Item : Interactable
{
    new public string name;
    protected FPSPlayerController player;
    protected Camera cam;
    protected LevelManager levelManager;

    private void Start()
    {
        player = FindAnyObjectByType<FPSPlayerController>();
        levelManager = FindAnyObjectByType<LevelManager>();
        cam = Camera.main;
    }

    public abstract void ItemEffect(GameObject target);

    public void UseItem()
    {
        RaycastHit hit;
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, player.interactRange))
        {
            ItemEffect(hit.collider.gameObject);
        }
        else { ItemEffect(null); }

        
    }
    
    // Pick up a dropped item when it is interacted with
    public override void OnInteract()
    {
        try 
        { 
            Item tempItem = Camera.main.GetComponentInChildren<Item>();
            tempItem.transform.localPosition += new Vector3(-0.45f, 0, 1);
            tempItem.transform.parent = null;
            tempItem.GetComponent<Rigidbody>().isKinematic = false;
        } 
        catch { }
        
        transform.parent = Camera.main.transform.Find("HeldItem");
        GetComponent<Rigidbody>().isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
