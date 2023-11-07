using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DataFromReact : MonoBehaviour
{
    public Text messageText;
    public string origin; //orgin that comes from React Native (there is a different origin used later)
    public string destination;
    public string beacon1;
    public bool accessibility;
    public string floor;
    public float speed;
    
    public bool floorChangeVisible = false;
    public bool changeFloorClicked = false;

    private static DataFromReact instance;

    public static DataFromReact Instance
    {
        get
        {
            if (instance == null)
            {
                //instance = new DataFromReact();
                // instance.originR;
                // instance.destinationR;
                instance = FindObjectOfType<DataFromReact>();
                if (instance == null)
                {
                    //GameObject obj = new GameObject("ReactToUnity"); // previously just GameObject()
                    //obj.name = typeof(DataFromReact).Name;   // This line will name the object

                    GameObject GameObj = GameObject.Find("ReactToUnity"); // Find the existing object named "ReactToUnity" in the scene
                    instance = GameObj.AddComponent<DataFromReact>();
                }
            }
            return instance;

    //         // if (instance == null)
    //         // {
    //         //     instance = FindObjectOfType<DataFromReact>();
    //         //     if (instance == null)
    //         //     {
    //         //         GameObject obj = new GameObject("DataFromReact");
    //         //         instance = obj.AddComponent<DataFromReact>();
    //         //     }
    //         //     // Initialize textMeshProUGUI if found on the object
    //         //     textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    //         //     if (textMeshProUGUI != null)
    //         //     {
    //         //         textMeshProUGUI.text = "TN246";
    //         //     }
    //         // }
    //         // return instance;
        }
    }

    // private void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //     }
    // }

    void Start(){
        messageText.text = "";
        //InvokeRepeating("GetData", 0f, 0.5f); // runs GetDatas every 500 ms  (not needed as setInterval in UnityApp.tsx accomplishes the same thing)
        
        // textMeshProUGUI = new TextMeshProUGUI();
        // if (textMeshProUGUI != null) {
        //     textMeshProUGUI.text = "TN246";
        // }
        // textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        // if (textMeshProUGUI != null)
        // {
        //     textMeshProUGUI.text = "TN246";
        // }
    }

    public  class JsonObject
    {
        public string origin;
        public string destination;
        public string beacon1;
        public string accessibility;
        public string floor;
        public string speed;
    }

    // Here is the name of the function that gets the data.
    // It should have the same name in RN function postMessage.
    public void GetData(string json)
    {
        JsonObject obj = JsonUtility.FromJson<JsonObject>(json);
        //string originScene;
        //string destinationScene;

        // origin and destination are sent with T in front if they are a room (ex: TN270) by React Native side
        if (obj.origin != null && obj.origin != "" && obj.origin[0] == 'T')
        {
            origin = obj.origin.Substring(1); //take a substring to get rid of the T ex: TN270 -> N270
            //originScene = origin.Substring(0,2); // ex: N270 -> N2
        }
        else{
            origin = obj.origin;
            //originScene = origin.Substring(0,2); // ex: N270 -> N2
        }

        if (obj.destination != null && obj.destination != "" && obj.destination[0] == 'T')
        {
            destination = obj.destination.Substring(1); //take a substring to get rid of the T ex: TN270 -> N270
            //destinationScene = destination.Substring(0,2); // ex: N270 -> N2
        }
        else{
            destination = obj.destination;
            //destinationScene = destination.Substring(0,2); // ex: N270 -> N2
        }

        //origin = obj.origin;
        //destination = obj.destination;
        beacon1 = obj.beacon1;

        if (obj.accessibility == "true"){
            accessibility = true;
            //messageText.text = "Please use the elevator to go to floor " + floor + ". \nPress ok after reaching the correct floor.";
            // floorChangeVisible = true;
        }
        // else if (originScene != null && originScene != destinationScene ) {
        //     accessibility = false;
        //     messageText.text = "Please use the stairs to go to floor " + floor + ".";
        // }
        // else if (originScene == null && obj.floor != destinationScene) {
        //     accessibility = false;
        //     messageText.text = "Please use the stairs to go to floor " + floor + ".";
        // }
        else{
            accessibility = false;
            // floorChangeVisible = false;
        }
        floor = obj.floor;
        speed = float.Parse(obj.speed);

        // // Update the Unity UI Text component
        //messageText.text  = "Origin: " + origin + ", Destination: " + destination +
        //                     ", Beacon ID1: " + beacon1 + ", Accessibility: " + accessibility + ", Floor: " + floor;
        //Debug.Log("Message received: " + messageText.text);
    }
}