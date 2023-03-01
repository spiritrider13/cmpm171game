using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{

    public Camera cam;
    Vector3 cameraDir;
    // Start is called before the first frame update
    void Start()
    {

        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraDir = cam.transform.forward;
        cameraDir.y = 0;
        transform.rotation = Quaternion.LookRotation(cameraDir);
    }
}
