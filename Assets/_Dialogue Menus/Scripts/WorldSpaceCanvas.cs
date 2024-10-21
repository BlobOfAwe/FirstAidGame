using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvas : MonoBehaviour
{
    private Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        try { player = GameObject.FindGameObjectsWithTag("Player")[0].transform; } catch { Debug.LogError("No Player Object found in scene"); }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 dir = new Vector3(player.position.x - transform.position.x, player.position.y - transform.position.y, player.position.z - transform.position.z);
        transform.forward = -dir;
    }
}
