using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongLine : MonoBehaviour
{
    // Start is called before the first frame update
    //public LineRenderer lineRenderer = NavMeshLine.lineRendererSmall;
    public static LineRenderer lineRenderer;

    // public LineRenderer lineRenderer = NavMeshLine.Instance.lineRendererSmall; //now uses specific instance from NavMeshLine
    //public float speed; 

    private float distanceAlongLine = 0;

    Vector3 lineDirection;
    float angle;
    Vector3 rotationAxis;
    Vector3 targetPosition;

    // private Vector3[] midpoints;

    private DataFromReact DR; // used DR as name for DataFromReact (allows access to variables from DataFromReact.cs)

    private void Start()
    {
        lineRenderer = GameObject.Find("LineRendererBig").GetComponent<LineRenderer>();
        DR = DataFromReact.Instance; //define instance of DataFromReact script to make sure all data comes from same script
    }

    void Update() {
    
        distanceAlongLine += DR.speed * Time.deltaTime;

        if (distanceAlongLine > lineRenderer.positionCount - 1) {
            distanceAlongLine = 0;
        }
        transform.position = Vector3.Lerp(lineRenderer.GetPosition(Mathf.FloorToInt(distanceAlongLine)), lineRenderer.GetPosition(Mathf.CeilToInt(distanceAlongLine)), distanceAlongLine % 1);
        transform.rotation = Quaternion.AngleAxis(angle, rotationAxis);
    }
    
}

