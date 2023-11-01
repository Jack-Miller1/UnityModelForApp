using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowUserButtonManager : MonoBehaviour
{
   public bool followOn;
   private int numPresses = 0; //keep track of the number of button presses, alternate between green and red
   private Button button;
   Color redColor = new Color(1.0f, 0.0f, 0.0f, 0.9f); //R,G,B,Opacity
   Color greenColor = new Color(0.0f, 1.0f, 0.0f, 0.9f);

   void Start()
   {
      button = GameObject.Find("Follow User Button").GetComponent<Button>();
      followOn = true;
   }

   public void OnButtonPress() {
      followOn = !followOn;

      ColorBlock colors = button.colors;
      numPresses++; // Button was pressed

      if (numPresses % 2 != 0) {
         colors.normalColor = redColor;
         colors.selectedColor = redColor;
         colors.pressedColor = greenColor;
      } else {
         colors.normalColor = greenColor;
         colors.selectedColor = greenColor;
         colors.pressedColor = redColor;
      }

    button.colors = colors; // Assign the modified ColorBlock back to the button
   }
}
