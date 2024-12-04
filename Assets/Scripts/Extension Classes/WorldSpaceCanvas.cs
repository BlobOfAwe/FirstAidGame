using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvas : MonoBehaviour
{
    private Transform cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 dir = new Vector3(cam.position.x - transform.position.x, cam.position.y - transform.position.y, cam.position.z - transform.position.z);
        transform.forward = -dir;
    }
}
