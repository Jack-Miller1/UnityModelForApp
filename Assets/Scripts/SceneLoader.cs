using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class SceneLoader
{

    public List<Scene> scenes = new List<Scene>();
    public Endpoint[] endpoints;

    void Start()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            scenes.Add(SceneManager.GetSceneAt(i));
            Debug.Log("Scene " + i + ": " + scenes[i].name);
        }
        // foreach (var sceneName in scenes)
        // {
        //     SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //     foreach(var endpoint in FindObjectsOfType<Endpoint>()){
        //         endpoints.append(endpoint);
        //     }
        // }
    }
}