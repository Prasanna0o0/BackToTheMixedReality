using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheelUpdater : MonoBehaviour
{
    [Header("References")]
    public CarInputProvider inputProvider;
    [Tooltip("Set ref in order of FL, FR, RL, RR")]
    public WheelCollider[] WheelColliders;
    [Tooltip("Set ref of wheel meshes in order of FL, FR, RL, RR")]
    public Transform[] Wheels;


    public Vector3 positionOffset; // Position offset
    public Vector3 rotationOffset; // Rotation offset



    #region Unity Methods
  
    void Update()
    {
        UpdateWheelMovements();
    }

    #endregion


    #region Wheel Update Methods
    

    private void UpdateWheelMovements()
    {
        for (int i = 0; i < Wheels.Length; i++)
        {
            Vector3 pos;
            Quaternion rot;
            WheelColliders[i].GetWorldPose(out pos, out rot);
            Wheels[i].transform.position = pos;
            Wheels[i].transform.rotation = rot;
        }
        Wheels[1].transform.rotation *= Quaternion.Euler(rotationOffset);
        Wheels[3].transform.rotation *= Quaternion.Euler(rotationOffset);
    }

    #endregion
}
