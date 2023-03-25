using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public GameObject target; // the object to follow

    void Start()
    {
        target = GameObject.Find("User");
    }

    void Update()
    {
        // Update the camera's position and rotation to follow the target.
        Vector3 targetPosition = target.transform.position;
        Vector3 cameraPosition = new Vector3(targetPosition.x, targetPosition.y + 800f, targetPosition.z);
        transform.position = cameraPosition;
        transform.rotation = Quaternion.LookRotation(Vector3.down, target.transform.forward);
    }
}
