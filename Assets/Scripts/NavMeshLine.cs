using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

[RequireComponent(typeof(LineRenderer))]

public class NavMeshLine : MonoBehaviour
{
    private DataFromReact DR;                  // used DR as name for DataFromReact (allows access to variables from DataFromReact.cs)
    public static GameObject origin;
    public static GameObject destination;      // origin and destination are used as game objects

    private string originRoom = "N259";
    private string destinationRoom = "N253";   // originRoom and destinationRoom are strings
    public string building = "N";              // Meanwhile, originR and destinationR will be taken as strings from DataFromReact.cs

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

        DR = DataFromReact.Instance; //define instance of DR for use later
        //DR = GetComponent<DataFromReact>();
    }

    void Update()
    {
        //DR.GetDatas("TN290");
        Debug.Log(DR.messageText.text);
        // set originRoom and destinationRoom strings to values from DataFromReact
        if (DR.originR != null && DR.destinationR != null)
        {
            originRoom = DR.originR.Substring(1); //take a substring to get rid of the T ex: TN270 -> N270
            destinationRoom = DR.destinationR.Substring(1);
        }

        origin = GameObject.Find(originRoom);  // "convert" string into game object

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

        NavMeshPath navMeshPath = new NavMeshPath();

        // Use the current origin and destination to update the line
        calculatePath(origin, destination, navMeshPath, lineRendererBig, 60.0f);
        calculatePath(origin, destination, navMeshPath, lineRendererSmall, 1.0f);

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
