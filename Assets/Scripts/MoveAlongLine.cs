using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongLine : MonoBehaviour
{
    public static LineRenderer lineRenderer;
    
    private string previousBeacon;
    private float distanceAlongLine = 0;

    Vector3 lineDirection;
    float angle;
    Vector3 rotationAxis;
    Vector3 targetPosition;

    // private Vector3[] midpoints;

    private DataFromReact DR; // used DR as name for DataFromReact (allows access to variables from DataFromReact.cs)

    private void Start()
    {
        lineRenderer = GameObject.Find("LineRendererSmall").GetComponent<LineRenderer>();
        DR = DataFromReact.Instance; //define instance of DataFromReact script to make sure all data comes from same script
        if(DR.beacon1 != null && DR.beacon1 != ""){
            previousBeacon = DR.beacon1;
        }
    }

    void Update() {
        distanceAlongLine += DR.speed * Time.deltaTime;

        if (distanceAlongLine > lineRenderer.positionCount - 1) { //reset the user position on the line if it is before the starting point of the line
            distanceAlongLine = 0;
        }
        if(DR.beacon1 != null && DR.beacon1 != "" && previousBeacon != DR.beacon1){ //reset the user position on the line when a new beacon is found
            previousBeacon = DR.beacon1;
            distanceAlongLine = 0;
        }

        transform.position = Vector3.Lerp(lineRenderer.GetPosition(Mathf.FloorToInt(distanceAlongLine)), lineRenderer.GetPosition(Mathf.CeilToInt(distanceAlongLine)), distanceAlongLine % 1);
        transform.rotation = Quaternion.AngleAxis(angle, rotationAxis);
    }
    
}

