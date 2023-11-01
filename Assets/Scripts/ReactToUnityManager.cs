using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class ReactToUnityManager : MonoBehaviour
{
    static ReactToUnityManager instance; //make sure there is only one instance of the ReactToUnity GameObject and the child GameObjects

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}