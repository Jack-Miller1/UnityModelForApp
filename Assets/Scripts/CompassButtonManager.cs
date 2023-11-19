using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassButtonManager : MonoBehaviour
{
   public bool rotateOn;
   private bool isRed = false; //button will either be green or red
   private Button button;
   Color redColor = new Color(0.545f, 0.0f, 0.0f, 0.99f); //R,G,B,Opacity
   Color greenColor = new Color(0.0f, 0.545f, 0.0f, 0.99f);

   void Start()
   {
      button = GameObject.Find("Compass Rotation Button").GetComponent<Button>();
      rotateOn = true;
   }

   public void OnButtonPress() {
      rotateOn = !rotateOn;

      ColorBlock colors = button.colors;
      isRed = !isRed; // Button was pressed, alternate between green and red

      if (isRed) {
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