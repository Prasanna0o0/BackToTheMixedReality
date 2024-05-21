using Cinemachine;
using DG.Tweening;
using DG.Tweening.CustomPlugins;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class TimeTravelManager : MonoBehaviour
{
    #region Fields

    [Header("Time Travel Timelines & Events")]
    public GameObject Timeline_TimeTravelPrep;
    public GameObject Timeline_TimeTravelExplosion;
    public GameObject Timeline_AfterTimeTravel;

    public GameObject TimeMachineMesh;
    public GameObject TimeMachineParent;


    public UnityEvent onTimeTravelPrepSpeedReached;
    public bool isTimeTravelPrepEventTriggered = false;


    public UnityEvent onTimeTravelCancelled;
    public bool isTimeTravelCancelledEventTriggered = false;

    public UnityEvent onTimeTravelSpeedReached;
    public bool isTimeTravelSpeedReachedEventTriggered = false;
    public UnityEvent onTimeTravelComplete;

    [Header("Time Travel Parameters")]
    public float travelTimeInSeconds = 45f;
    public float speedForTimeTravelCancel = 30f;
    public float speedForTimeTravelPrepare = 44f;
    public float speedForTimeTravelMoment = 88f;
   

    [Header("After Time Travel Movement Params")]
    public float speedMilesPerHour;
    public bool canTimeTravel = false;

    #endregion

    #region Unity 
    private void Awake()
    {
        DeActivateTimelines();
    }

    private void OnEnable()
    {
        GlobalEventsManager.Instance.onTimeTravelPrepCancelled += CancelTimeTravel_Prep;

    }

    private void OnDisable()
    {
        GlobalEventsManager.Instance.onTimeTravelPrepCancelled -= CancelTimeTravel_Prep;
    }

    private void Update()
    {
        speedMilesPerHour = TimeMachineFactory.Instance.carControllerScript.GetSpeedMilesPerHour();
        if (canTimeTravel)
        {
            ListenForTimeTravel();
        }
    }

    #endregion


    #region Time Travel Methods

    void ListenForTimeTravel()
    {
        if (!TimeMachineFactory.Instance.carControllerScript.isCarControlEnabled) return;

        // Trigger the event when the car reaches the required speed
        if (speedMilesPerHour >= speedForTimeTravelPrepare && onTimeTravelPrepSpeedReached != null && !isTimeTravelPrepEventTriggered
            && !TimeMachineFactory.Instance.carControllerScript.IsMovingBackward())
        {
            InitiateTimeTravel_Prep();
        }
        if (speedMilesPerHour <= speedForTimeTravelCancel && onTimeTravelCancelled != null && !isTimeTravelCancelledEventTriggered
            && !TimeMachineFactory.Instance.carControllerScript.IsMovingBackward())
        {

            CancelTimeTravel_Prep();
        }
    }

    public void InitiateTimeTravel_Prep()
    {
        //This will trigger TimeTravel manager to activate some crazy particles etc.
        onTimeTravelPrepSpeedReached.Invoke();


        Debug.Log("Time travel initiated");
        //this is for triggering time travel prep event only once
        isTimeTravelPrepEventTriggered = true;

        //Since time travel is initiated, it can be cancelled again
        isTimeTravelCancelledEventTriggered = false;

        Timeline_TimeTravelPrep.SetActive(true);
        Timeline_TimeTravelPrep.GetComponent<PlayableDirector>().Play();
    }

    public void CancelTimeTravel_Prep()
    {
        //This will trigger TimeTravel manager to cancel time travel
        onTimeTravelCancelled.Invoke();

        Timeline_TimeTravelPrep.GetComponent<PlayableDirector>().Stop();
        Timeline_TimeTravelPrep.SetActive(false);

        //this is for triggering time travel cancel event only once
        isTimeTravelCancelledEventTriggered = true;

        //since time travel is cancelled' it can be initiated again
        isTimeTravelPrepEventTriggered = false;
    }

    public void DoTimeTravel()
    {
        Debug.Log("Time travel started. ");

        if (!TimeMachineFactory.Instance.carControllerScript.IsMovingBackward())
        {
            TimeMachineFactory.Instance.inputProvider.enabled = false;
            
            //Disable Car Sound
            TimeMachineFactory.Instance.carEngineSoundManager.DisableCarEngineSound();

            Timeline_TimeTravelPrep.GetComponent<PlayableDirector>().Stop();
            Timeline_TimeTravelPrep.SetActive(false);

            Timeline_TimeTravelExplosion.SetActive(true);
            Timeline_TimeTravelExplosion.GetComponent<PlayableDirector>().Play();
            onTimeTravelSpeedReached.Invoke();

            GlobalEventsManager.Instance.TimeTravelStarted();
        }
    }


    /// <summary>
    /// This is the exact momement when the Time machine do dissapear
    /// </summary>

    public void OnTimeTravelOccured()
    {
        canTimeTravel = false;
        GlobalEventsManager.Instance.TimeTravelOccured();
        StartCoroutine(InitiateAfterTimeTravel());

    }


    private IEnumerator InitiateAfterTimeTravel()
    {
        // Wait for the time to pass
        yield return new WaitForSeconds(travelTimeInSeconds);

        Timeline_AfterTimeTravel.SetActive(true);
        Timeline_AfterTimeTravel.GetComponent<PlayableDirector>().Play();

        TimeMachineFactory.Instance.timeTravelSoundManager.PlayAfterTimeTravelSound();
    }

    /// <summary>
    /// Called from the timeline, Timeline_AfterTimeTravel
    /// </summary>
    public void OnAfterTimeTravelSignalFired()
    {
        DeActivateTimelines();
        TimeMachineMesh.SetActive(true);
        PerformTimeTravelMovement();
    }


    public void PerformTimeTravelMovement()
    {
        if (!TimeMachineFactory.Instance.timeTravelProximityDetector.obstacleExistsInRunway)
        {
            Rigidbody rb = TimeMachineParent.GetComponent<Rigidbody>();

            Vector3 forwardVelocity = transform.forward * 2f;
            rb.velocity = forwardVelocity;

            //Play Drift Sound
            TimeMachineFactory.Instance.timeTravelSoundManager.PlayDriftSound();
        }
       

        onTimeTravelComplete.Invoke();
        OnTimeTravelComplete();
        GlobalEventsManager.Instance.TimeTravelCompleted();

        TimeMachineFactory.Instance.inputProvider.enabled = true;
        TimeMachineFactory.Instance.carEngineSoundManager.EnableCarEngineSound();

        DOVirtual.DelayedCall(1.5f, () => canTimeTravel = true);
    }



    public void OnTimeTravelComplete()
    {
        TimeMachineFactory.Instance.carControllerScript.isCarControlEnabled = true;
        isTimeTravelPrepEventTriggered = false;
        isTimeTravelSpeedReachedEventTriggered = false;
    }

    public void DeActivateTimelines()
    {
        Timeline_TimeTravelPrep.SetActive(false);
        Timeline_AfterTimeTravel.SetActive(false);
        Timeline_TimeTravelExplosion.SetActive(false);
    }


    #endregion
}
