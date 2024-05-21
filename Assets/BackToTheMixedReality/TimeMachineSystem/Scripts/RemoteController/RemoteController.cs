using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemoteController : MonoBehaviour
{
    public Transform rightStick;

    public Transform leftStick;

    public TextMeshPro text_SpeedIndicator;
    // Update is called once per frame
    void Update()
    {
        MapInputToRightStick(TimeMachineFactory.Instance.inputProvider.Vertical, rightStick);

        MapInputToLeftStick(TimeMachineFactory.Instance.inputProvider.Horizontal, leftStick);

        if (text_SpeedIndicator!=null)
        {
            UpdateSpeedIndicator();

        }
    }




    void MapInputToRightStick(float input, Transform stick)
    {
        // Calculate rotation angle based on input
        float targetRotation = -45f * input;

        // Clamp the rotation angle to be within the -135 to -45 degree range
        targetRotation = Mathf.Clamp(targetRotation, -45f, 45f);
        stick.localEulerAngles = new Vector3(targetRotation, stick.localEulerAngles.y, stick.localEulerAngles.z);
    }


  

    void MapInputToLeftStick(float input, Transform stick)
    {
        // Calculate rotation angle
        float targetRotation = -20f * input;
        //targetRotation = Mathf.Clamp(targetRotation, 30, -30);
        stick.localEulerAngles = new Vector3(stick.localEulerAngles.x, targetRotation, targetRotation);
    }

    void UpdateSpeedIndicator()
    {
         
        text_SpeedIndicator.text = ClampBasedOnRange(TimeMachineFactory.Instance.carControllerScript.speedMilesPerHour, 0, 4, 0, 88);

    }

    public string ClampBasedOnRange(float x, float xMin, float xMax, float yMin, float yMax)
    {
        // Normalize x to a 0-1 range
        float normalizedX = (x - xMin) / (xMax - xMin);

        // Interpolate the y value based on normalized x
        float y = normalizedX * (yMax - yMin) + yMin;

        // Clamp y to ensure it stays within the desired range
        float yClamped = Mathf.Clamp(y, yMin, yMax);

        // Format the clamped value as an integer string
        string formattedString = yClamped.ToString("F0"); // F0 format specifier for no decimal places

        
        return formattedString;
    }


}
