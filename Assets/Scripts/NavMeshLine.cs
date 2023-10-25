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

    private static NavMeshLine instance;

    private static string lastScene;

    // private void Awake()
    // {
    //     // Check if an instance already exists
    //     if (instance != null)
    //     {
    //         // If an instance already exists, destroy this new instance
    //         Destroy(gameObject);
    //         return;
    //     }

    //     // Set the instance to this object
    //     instance = this;

    //     // Ensure that this instance persists between scenes
    //     DontDestroyOnLoad(gameObject);

    //     // Your initialization code goes here
    // }


    void Start()
    {
        //initialize navmesh agent and line renderer
        GameObject lb = GameObject.Find("LineRendererBig");
        GameObject ls = GameObject.Find("LineRendererSmall");
        lineRendererBig = lb.GetComponent<LineRenderer>();
        lineRendererSmall = ls.GetComponent<LineRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        destinationScene = building + destinationRoom.Substring(2,1); //if input is TN270, the substring will take the 2 (end result: building + 2 = N2)

        DR = DataFromReact.Instance; //define instance of DR for use later
        //DR = GetComponent<DataFromReact>();

        DontDestroyOnLoad(DR.gameObject); //make sure ReactToUnity doesn't get destroyed when loading a new scene


        //SceneManager.LoadScene("Scenes/N2"); //start script on second floor (contains ReactToUnity game object)
        //Debug.Log("scene 2 loaded");

        // // Find all objects of type YourScript in the scene
        // NavMeshLine[] scriptInstances = FindObjectsOfType<NavMeshLine>();

        // // Check if there is more than one instance
        // if (scriptInstances.Length > 1)
        // {
        //     Debug.Log("Multiple instances of YourScript found in the scene.");
        // }
    }

    void Update()
    {
        Debug.Log(DR.messageText.text);
        string sceneToLoad = "";
        Scene currentScene = SceneManager.GetActiveScene();

        // set originRoom and destinationRoom strings to values from DataFromReact
        if (DR.destinationR != null && DR.beaconID1R != null && DR.beaconID1R != "") // checks if the closest beacon is valid
        {
            if (DR.beaconID1R == "19" || DR.beaconID1R == "20" || DR.beaconID1R == "21" || DR.beaconID1R == "22"){ //beacons on 1st floor stairs
                if(currentScene.name == "N1" && destinationScene != "N1"){ // going up stairs
                    sceneToLoad = "N2";
                }
                else{ // going down stairs
                    sceneToLoad = "N1";
                }
            }
            else if (DR.beaconID1R == "15" || DR.beaconID1R == "16" || DR.beaconID1R == "17" ||DR.beaconID1R == "18"){ //beacons on 3rd floor stairs
                if(currentScene.name == "N3" && destinationScene != "N3"){ // going down stairs
                    sceneToLoad = "N2";
                }
                else{ // going up stairs
                    sceneToLoad = "N3";
                }
            }
            //rest of beacons on second floor (currently using these when on 1st and 3rd, don't change scene)
            // else{
            //     sceneToLoad = "N2"; 
            // }
            originRoom = "Beacon " + DR.beaconID1R;
            destinationRoom = DR.destinationR;
        }
        else if (DR.originR != null && DR.destinationR != null) // uses the closest room entered by the user in React Native if no beacons found
        {
            if (DR.originR[0] == 'T')
            {
                originRoom = DR.originR.Substring(1); //take a substring to get rid of the T ex: TN270 -> N270
                sceneToLoad = originRoom.Substring(0,2); // ex: N270 -> Scenes/N2
            }
            else{
                originRoom = DR.originR;
                sceneToLoad = originRoom.Substring(0,2); // ex: N270 -> Scenes/N2
            }
            // if (DR.destinationR[0] == 'T')
            // {
            //     Debug.Log("removed T: ");
            //     destinationRoom = DR.destinationR.Substring(1); //take a substring to get rid of the T ex: TN270 -> N270
            // }
            // else{
            //     Debug.Log("didn't remove T: ");
            //     destinationRoom = DR.destinationR;
            // }
        }

        if (DR.destinationR[0] == 'T')
            {
                destinationRoom = DR.destinationR.Substring(1); //take a substring to get rid of the T ex: TN270 -> N270
            }
            else{
                destinationRoom = DR.destinationR;
            }




        if ( (sceneToLoad != currentScene.name) && (sceneToLoad != lastScene)) { //switch scenes if needed
            lastScene = currentScene.name; // Update what the lastScene was
            SceneManager.LoadScene(sceneToLoad);
        }

        origin = GameObject.Find(originRoom);  // "convert" string into game object

        if (GameObject.Find(destinationRoom) == null){
            Debug.Log("destinationRoom: " + destinationRoom);
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

        //Switches scene when user clicks enter (testing purposes)
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
