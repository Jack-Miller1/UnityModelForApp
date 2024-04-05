using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FloorChangeButtonManager : MonoBehaviour
{
    private DataFromReact DR; // used DR as name for DataFromReact (allows access to variables from DataFromReact.cs)
    private Button button;

    void Start()
    {
        DR = DataFromReact.Instance; //define instance of DataFromReact script to make sure all data comes from same script
        button = GameObject.Find("Floor Change Button").GetComponent<Button>();
    }

    void Update()
    {
        if(!DR.accessibility){ //toggle the button's visibility based on if accessibility is enabled or not
            DR.floorChangeVisible = false;
        }
        button.gameObject.SetActive(DR.floorChangeVisible);
    }

    public void OnButtonPress() {
        DR.changeFloorClicked = true;
        DR.floorChangeVisible = !DR.floorChangeVisible;
        DR.messageText.text = "";
        button.gameObject.SetActive(DR.floorChangeVisible);
   }
}
