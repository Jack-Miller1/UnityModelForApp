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

    private string originRoom;
    private string destinationRoom;            // originRoom and destinationRoom are strings. Meanwhile, originR and destinationR will be taken as strings from DataFromReact.cs
    public string building = "N";

    public static LineRenderer lineRendererBig;
    public static LineRenderer lineRendererSmall;
    private NavMeshAgent navMeshAgent;

    public static bool accessibileRoute = false;
    
    private string destinationScene;
    private Scene currentScene;
    
    private GameObject stairs;
    private GameObject elevator;

    private static NavMeshLine instance;

    private static string lastScene;
    private string sceneToLoad;
    private string lastFloor; //different than lastScene, used to track if the user enters a new floor on start over page

    void Start()
    {
        //initialize navmesh agent and line renderer
        GameObject lb = GameObject.Find("LineRendererBig");
        GameObject ls = GameObject.Find("LineRendererSmall");
        lineRendererBig = lb.GetComponent<LineRenderer>();
        lineRendererSmall = ls.GetComponent<LineRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        // destinationScene = building + destinationRoom.Substring(2,1); //if input is TN270, the substring will take the 2 (end result: building + 2 = N2)

        DR = DataFromReact.Instance; //define instance of DataFromReact script to make sure all data comes from same script
        //DR = GetComponent<DataFromReact>();

        DontDestroyOnLoad(DR.gameObject); //make sure ReactToUnity and its child objects don't get destroyed when loading a new scene
        DontDestroyOnLoad(lb); //make sure the line renderers don't get destroyed when loading a new scene
        DontDestroyOnLoad(ls);

        //load the floor entered by the user. This will then be kept track of by Scene currentScene = SceneManager.GetActiveScene(); in the Update()
        sceneToLoad = DR.floor; //DR.floor will only be used when beacons are found
        // DR.currentFloor = DR.floor;
        DR.floorChangeVisible = false;
        //lastFloor = DR.floor;
    }

    void Update()
    {
        //Debug.Log(DR.messageText.text);
        //string sceneToLoad = DR.floor;
        currentScene = SceneManager.GetActiveScene();
        accessibileRoute = DR.accessibility;
        destinationScene = building + DR.destination.Substring(1,1); //if input is N270, the substring will take the 2 (end result: building + 2 = N2)

        // set originRoom and destinationRoom strings to values from DataFromReact
        if (DR.destination != null && DR.beacon1 != null && DR.beacon1 != "") // checks if the closest beacon is valid
        {
            if (DR.beacon1 == "19" || DR.beacon1 == "20" || DR.beacon1 == "21" || DR.beacon1 == "22") { //beacons on 1st floor stairs
                if(currentScene.name == "N1" && destinationScene != "N1" && "N2" != lastScene){ // going up stairs
                    sceneToLoad = "N2";
                }
                else if ("N1" != lastScene){ // going down stairs
                    sceneToLoad = "N1";
                }
            }
            else if (DR.beacon1 == "15" || DR.beacon1 == "16" || DR.beacon1 == "17" || DR.beacon1 == "18") { //beacons on 3rd floor stairs
                if(currentScene.name == "N3" && destinationScene != "N3" && "N2" != lastScene){ // going down stairs
                    sceneToLoad = "N2";
                }
                else if ("N3" != lastScene){ // going up stairs
                    sceneToLoad = "N3";
                }
            }
            //rest of beacons on second floor (currently using these when on 1st and 3rd, don't change scene)

            originRoom = "Beacon " + DR.beacon1;
            //destinationRoom = DR.destination;
        }
        else if (DR.origin != null && DR.destination != null) // uses the closest room entered by the user in React Native if no beacons found
        {
            // if (DR.origin[0] == 'T')
            // {
            //     originRoom = DR.origin.Substring(1); //take a substring to get rid of the T ex: TN270 -> N270
            //     sceneToLoad = originRoom.Substring(0,2); // ex: N270 -> Scenes/N2
            // }
            // else{
            //     originRoom = DR.origin;
            //     sceneToLoad = originRoom.Substring(0,2); // ex: N270 -> Scenes/N2
            // }
            originRoom = DR.origin;
            sceneToLoad = originRoom.Substring(0,2); // ex: N270 -> Scenes/N2
        }

        destinationRoom = DR.destination;
        // if (DR.destination[0] == 'T')
        // {
        //     destinationRoom = DR.destination.Substring(1); //take a substring to get rid of the T ex: TN270 -> N270
        // }
        // else{
        //     destinationRoom = DR.destination;
        // }

        if(DR.changeFloorClicked == true){
            sceneToLoad = destinationScene;
        }

        if ( (sceneToLoad != currentScene.name) && sceneToLoad != "") { //switch scenes if needed           //&& (sceneToLoad != lastScene)
            lastScene = currentScene.name; // Update what the lastScene was
            SceneManager.LoadScene(sceneToLoad);
            //Debug.Log("scene loaded: " + sceneToLoad);
        }

        origin = GameObject.Find(originRoom);  // "convert" string into game object

        if (GameObject.Find(destinationRoom) == null){
            if (accessibileRoute){
                destination = elevator = FindNearest(origin.transform.position, "Elevator");
                if (!DR.changeFloorClicked){
                    DR.messageText.text = "Please use the elevator to go to floor " + destinationScene + ". Then, press update.";
                    DR.floorChangeVisible = true;
                }
            }
            else
            {
                destination = stairs = FindNearest(origin.transform.position, "Stairs");
                DR.messageText.text = "Please use the stairs to go to floor " + destinationScene + ".";
            }
        }else
        {
            destination = GameObject.Find(destinationRoom);
            DR.messageText.text = "";
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
