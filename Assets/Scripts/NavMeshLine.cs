using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

[RequireComponent(typeof(LineRenderer))]

public class NavMeshLine : MonoBehaviour
{
    public static GameObject origin;
    public static GameObject destination;

    private string originRoom = "N342";
    private string destinationRoom = "N396";
    public string building = "N";

    public static LineRenderer lineRendererBig;
    public static LineRenderer lineRendererSmall;
    private NavMeshAgent navMeshAgent;

    public static bool accessibileRoute = false;
    
    private string destinationScene;
    
    private GameObject stairs;
    private GameObject elevator;

    void Start()
    {
        //initialize navmesh agent and line renderer

        GameObject lb = GameObject.Find("LineRendererBig");
        GameObject ls = GameObject.Find("LineRendererSmall");
        lineRendererBig = lb.GetComponent<LineRenderer>();
        lineRendererSmall = ls.GetComponent<LineRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        destinationScene = building + destinationRoom.Substring(1,1);
        
        origin = GameObject.Find(originRoom);

        if (GameObject.Find(destinationRoom) == null){
            if (accessibileRoute){
                destination = elevator = FindNearest(origin.transform.position, "Elevator");
            }
            else
            {
                destination = stairs = FindNearest(origin.transform.position, "Stairs");
            }
        }else
        {
            destination = GameObject.Find(destinationRoom);
        }

        //initialize navmesh
        NavMeshPath navMeshPath = new NavMeshPath();

        //Creates the illusion that the user is going down the center of the line
        calculatePath(origin, destination, navMeshPath, lineRendererBig, 60.0f);
        calculatePath(origin, destination, navMeshPath, lineRendererSmall, 1.0f);
    }

    void Update()
    {

        //Switches scene when user clicks enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (accessibileRoute){
                originRoom = elevator.name;
            }else{
                originRoom = stairs.name;
            }
            
            SceneManager.LoadScene(destinationScene);
        }
    }

    static void calculatePath(GameObject origin, GameObject destination, NavMeshPath navMeshPath, LineRenderer lineRenderer, float width)
    {
        if (NavMesh.CalculatePath(origin.transform.position, destination.transform.position, UnityEngine.AI.NavMesh.AllAreas, navMeshPath))
        {
            Vector3[] corners = navMeshPath.corners;  //vector list of all navmesh corners

            //directions for line renderer
            lineRenderer.positionCount = corners.Length;
            formatLine(lineRenderer, width, width);
            lineRenderer.SetPositions(corners);
            
        }
    }

    static void formatLine(LineRenderer line, float startWidth, float endWidth)
    {
        line.startWidth = startWidth;
        line.endWidth = endWidth;
        line.startColor = Color.white;
        line.endColor = Color.white;
    }

    //position is origin room and tag is either stairs or elevator
    static GameObject FindNearest(Vector3 position, string tag)
    {
        GameObject[] tagObjects = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearestTag = null;
        float minDistance = float.MaxValue;

        foreach (GameObject x in tagObjects)
        {
            float distance = Vector3.Distance(position, x.transform.position);
            if (distance < minDistance)
            {
            nearestTag = x;
            minDistance = distance;
            }
        }

        return nearestTag;
    }
}

    



    
