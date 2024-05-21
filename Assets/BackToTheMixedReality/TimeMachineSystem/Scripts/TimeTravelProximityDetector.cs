using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using NaughtyAttributes;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;
using FMODUnity;
using FMOD.Studio;

public class TimeTravelProximityDetector : MonoBehaviour
{
    [SerializeField] private VisualEffect linearFlame1;
    [SerializeField] private VisualEffect linearFlame2;
    public EventReference eventPath_FlamingTires;
    EventInstance eventInstance_FlamingTires;

    public float maxDistance = 2000f; 

    public LayerMask wallLayerMask; 

    FMODController fmodController;

    public bool obstacleExistsInRunway = false;
    #region Unity Events

    private void Start()
    {
        fmodController = gameObject.AddComponent<FMODController>();
    }

    #endregion

    // Method to be called to calculate the distance to the nearest wall
    public void CalculateWallDistanceAndToggleFlames()
    {
        linearFlame1.gameObject.SetActive(true);
        linearFlame2.gameObject.SetActive(true);
        RaycastHit hit;
        Vector3 forward = TimeMachineFactory.Instance.carControllerScript.gameObject.transform.TransformDirection(Vector3.forward)*maxDistance;
        Debug.DrawRay(TimeMachineFactory.Instance.carControllerScript.gameObject.transform.position, forward, Color.green);

        if (Physics.Raycast(TimeMachineFactory.Instance.carControllerScript.gameObject.transform.position+new Vector3(0,0.5f,0), forward, out hit,maxDistance,wallLayerMask))
        {
            if (hit.collider.CompareTag(Tags.WallsAndVolumes)) // Using CompareTag for efficiency
            {
                float distanceToWall = hit.distance;
                if (distanceToWall<2f)
                {
                    obstacleExistsInRunway =true;

                    linearFlame1.gameObject.SetActive(false);
                    linearFlame2.gameObject.SetActive(false);
                    if (eventInstance_FlamingTires.isValid())
                    {
                        fmodController.StopEvent(eventInstance_FlamingTires);
                    }

                }
                else
                {
                    obstacleExistsInRunway = false;

                    linearFlame1.gameObject.SetActive(true);
                    linearFlame2.gameObject.SetActive(true);
                    eventInstance_FlamingTires= fmodController.CreateAndStartEvent(eventPath_FlamingTires);
                    DOVirtual.DelayedCall(10.3f, () => OnFlamingTiresStop());
                }
                  
            }
        }
        else
        {
            Debug.Log("No wall within max distance");
        }
    }

    void OnFlamingTiresStop()
    {
        fmodController.StopEvent(eventInstance_FlamingTires);
        TimeMachineFactory.Instance.timeTravelManager.DeActivateTimelines();

    }


}
