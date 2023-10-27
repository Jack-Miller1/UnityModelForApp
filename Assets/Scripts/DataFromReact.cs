using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;


public class DataFromReact : MonoBehaviour
{
    public Text messageText;
    public string origin = "TN241"; //orgin that comes from React Native (there is a different origin used later)
    public string destination = "TN246";
    public string beacon1;
    public bool accessibility;
    public string floor;
    //public TextMeshProUGUI textMeshProUGUI;
    // public string Hello;

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

                    GameObject obj = GameObject.Find("ReactToUnity"); // Find the existing object named "ReactToUnity" in the scene
                    instance = obj.AddComponent<DataFromReact>();
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
        messageText.text = "Goodbye";

        //InvokeRepeating("GetDatas", 0f, 0.5f); // runs GetDatas every 500 ms  (not needed as setInterval in UnityApp.tsx accomplishes the same thing)
        
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
    }
    // As you can see here is the name of the function that we get the data.
    // it should have the same name in RN function postMessage.
    public void GetData(string json)
    {
        //textMeshProUGUI.text = json;
        JsonObject obj = JsonUtility.FromJson<JsonObject>(json);
        origin = obj.origin;
        destination = obj.destination;
        beacon1 = obj.beacon1;
        if (obj.accessibility == "true"){
            accessibility = true;
        }
        else{
            accessibility = false;
        }
        floor = obj.floor;

        // Update the Unity UI Text component
        messageText.text  = "Origin: " + origin + ", Destination: " + destination +
                            ", Beacon ID1: " + beacon1 + ", Accessibility: " + accessibility + ", Floor: " + floor;
        Debug.Log("Message received: " + messageText.text);

        // if (textMeshProUGUI != null)
        // {
        //     textMeshProUGUI.text  = "Origin: " + originR + "\nDestination: " + destinationR +
        //                     "\nBeacon ID: " + beaconID1R + "\nDistance: " + distance1R;
        // }
        // Debug.Log("Message received: " + textMeshProUGUI.text);
    }
}