using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldspaceUIRaycaster : GraphicRaycaster
{
    // The following code was written by u/QpaCompany as a workaround for UI elements not working with CursorLockMode.Locked
    // https://discussions.unity.com/t/world-space-canvas-cursorlockmode-locked-incompatible/717500/6 
    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        //Set middle screen pos or you can set variable on start and use it
        eventData.position = new(Screen.width / 2, Screen.height / 2);
        base.Raycast(eventData, resultAppendList);
    }
}
