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

    private DataFromReact DR; // used DR as name for DataFromReact (allows access to variables from DataFromReact.cs)
    private GameObject endpoint;
    private float distanceThreshold = 600f; //used to check if the user is close to their destination

    private Vector3 velocity;

    private void Start()
    {
        lineRenderer = GameObject.Find("LineRendererSmall").GetComponent<LineRenderer>();
        DR = DataFromReact.Instance; //define instance of DataFromReact script to make sure all data comes from same script
        if(DR.beacon1 != null && DR.beacon1 != ""){
            previousBeacon = DR.beacon1;
        }
    }

    void Update() {
        //check if user object is close to the endpoint
        if(endpoint == null && GameObject.Find(DR.destination) != null){
            endpoint = GameObject.Find(DR.destination);
        }
        if (endpoint != null)
        {
            float distance = Vector3.Distance(endpoint.transform.position, transform.position);

            if (distance < distanceThreshold)
            {
                DR.messageText.text = "You are close to your destination.";
            }
            if (distance < distanceThreshold/3)
            {
                DR.messageText.text = "You have reached your destination. \nThank you for using MNSU Wayfinder!";
            }
        }

        //move user object along the line
        distanceAlongLine += DR.speed * Time.deltaTime;

        if (distanceAlongLine > lineRenderer.positionCount - 1) { //when distance is too large, wrap around
            distanceAlongLine = 0;
        }
        if(DR.beacon1 != null && DR.beacon1 != "" && previousBeacon != DR.beacon1){ //reset the user position on the line when a new beacon is in range
            previousBeacon = DR.beacon1;
            distanceAlongLine = 0;
        }
        
        //use linear interpolation to smoothly move the user object along the line.
        transform.position = Vector3.Lerp(lineRenderer.GetPosition(Mathf.FloorToInt(distanceAlongLine)), lineRenderer.GetPosition(Mathf.CeilToInt(distanceAlongLine)), distanceAlongLine %1);
        transform.rotation = Quaternion.AngleAxis(angle, rotationAxis); 
    }
    
}

