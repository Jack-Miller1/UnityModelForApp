using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFacingCamera : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Ensure the text is rotated to match the camera rotation
        //float xRotation = cam.transform.eulerAngles.x;
        transform.rotation = Quaternion.Euler(90f, cam.transform.eulerAngles.y, 0);
    }
}
