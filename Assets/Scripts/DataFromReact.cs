using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataFromReact : MonoBehaviour
{
    public string originR = "TN270"; //originR because orgin comes from React Native (there is a different origin used later)
    public string destinationR = "TN288";
    public string beaconID1R;
    public string distance1R;

    private static DataFromReact instance;

    public static DataFromReact Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataFromReact>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    //obj.name = typeof(DataFromReact).Name;   // This line will name the object
                    instance = obj.AddComponent<DataFromReact>();
                }
            }
            return instance;
        }
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
            JsonObject obj = JsonUtility.FromJson<JsonObject>(json);
            originR = obj.originR;
            destinationR = obj.destinationR;
            beaconID1R = obj.beaconID1R;
            distance1R = obj.distance1R;
        }
}