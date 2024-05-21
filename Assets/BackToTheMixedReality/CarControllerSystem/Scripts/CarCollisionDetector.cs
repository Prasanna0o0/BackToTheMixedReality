using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionDetector : MonoBehaviour
{

   

    #region Unity Events
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.WallsAndVolumes)
        {
            Debug.Log("Hit a wall!");

            GlobalEventsManager.Instance.CarCollidedWithWalls();
        }
    }

   
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Tags.WallsAndVolumes)
        {
            Debug.Log("Hit a wall!");

            GlobalEventsManager.Instance.CarCollidedWithWalls();
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == Tags.WallsAndVolumes)
        {
            Debug.Log("Hit a wall!");

            GlobalEventsManager.Instance.CarCollidedWithWalls();
        }
    }



    #endregion



}
