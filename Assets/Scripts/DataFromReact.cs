using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataFromReact : MonoBehaviour
{
    public string name;
    public string age;

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
                    obj.name = typeof(DataFromReact).Name;
                    instance = obj.AddComponent<DataFromReact>();
                }
            }
            return instance;
        }
    }

    public  class JsonObject
    {
        public string name;
        public string age;
    }
    // As you can see here is the name of the function that we get the data.
    // it should have the same name in RN function postMessage.
        public void GetDatas(string json)
        {
            JsonObject obj = JsonUtility.FromJson<JsonObject>(json);
            name = obj.name;
            age = obj.age;
        }
}