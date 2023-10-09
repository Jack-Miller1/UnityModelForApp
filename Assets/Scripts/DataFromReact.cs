using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;


public class DataFromReact : MonoBehaviour
{
    public Text messageText;
    public string originR = "TN241"; //originR because orgin comes from React Native (there is a different origin used later)
    public string destinationR = "TN246";
    public string beaconID1R;
    public string distance1R;
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
                    GameObject obj = new GameObject("ReactToUnity"); // previously just GameObject()
                    //obj.name = typeof(DataFromReact).Name;   // This line will name the object
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
        public string originR;
        public string destinationR;
        public string beaconID1R;
        public string distance1R;
    }
    // As you can see here is the name of the function that we get the data.
    // it should have the same name in RN function postMessage.
    public void GetDatas(string json)
    {
        //textMeshProUGUI.text = json;
        JsonObject obj = JsonUtility.FromJson<JsonObject>(json);
        originR = obj.originR;
        destinationR = obj.destinationR;
        beaconID1R = obj.beaconID1R;
        distance1R = obj.distance1R;
        

        // Update the Unity UI Text component
        messageText.text  = "Origin: " + originR + ", Destination: " + destinationR +
                            ", Beacon ID: " + beaconID1R + ", Distance: " + distance1R;
        Debug.Log("Message received: " + messageText.text);

        // if (textMeshProUGUI != null)
        // {
        //     textMeshProUGUI.text  = "Origin: " + originR + "\nDestination: " + destinationR +
        //                     "\nBeacon ID: " + beaconID1R + "\nDistance: " + distance1R;
        // }
        // Debug.Log("Message received: " + textMeshProUGUI.text);
    }
}