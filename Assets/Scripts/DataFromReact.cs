using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DataFromReact : MonoBehaviour
{
    //values received by Unity from React Native
    public string origin; 
    public string destination;
    public string beacon1;
    public bool accessibility;
    public string floor;
    public float speed;
    
    //values used by other scripts in Unity that don't come from React Native
    public Text messageText;
    public bool floorChangeVisible = false;
    public bool changeFloorClicked = false;

    private static DataFromReact instance;

    public static DataFromReact Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataFromReact>();
                
                if (instance == null) //if the instance is still null
                {
                    GameObject GameObj = GameObject.Find("ReactToUnity"); // Find the existing object named "ReactToUnity" in the scene
                    instance = GameObj.AddComponent<DataFromReact>();
                }
            }
            return instance;
        }
    }


    void Start(){
        messageText.text = "";
    }

    //This defines the object that has the data being sent by React Native
    public class JsonObject
    {
        public string origin;
        public string destination;
        public string beacon1;
        public string accessibility;
        public string floor;
        public string speed;
    }

    // Function that gets the data. It should have the same name in React Native function postMessage.
    // Values from the JsonObject are placed into variables that other Unity scripts can use.
    public void GetData(string json)
    {
        JsonObject obj = JsonUtility.FromJson<JsonObject>(json);

        // origin and destination are sent with T in front if they are a room (ex: TN270) by React Native side
        if (obj.origin != null && obj.origin != "" && obj.origin[0] == 'T')
        {
            origin = obj.origin.Substring(1); //take a substring to get rid of the T ex: TN270 -> N270
        }
        else{
            origin = obj.origin;
        }

        if (obj.destination != null && obj.destination != "" && obj.destination[0] == 'T')
        {
            destination = obj.destination.Substring(1); //take a substring to get rid of the T ex: TN270 -> N270
        }
        else{
            destination = obj.destination;
        }

        beacon1 = obj.beacon1;

        if (obj.accessibility == "true"){
            accessibility = true;

        }
        else{
            accessibility = false;
        }
        floor = obj.floor;
        speed = float.Parse(obj.speed);
    }
}