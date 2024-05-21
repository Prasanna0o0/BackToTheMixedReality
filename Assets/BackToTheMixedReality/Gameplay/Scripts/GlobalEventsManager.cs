using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalEventsManager : MonoBehaviour
{

    #region Singleton Implementation
    public static GlobalEventsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    // Define events
    public event Action onTimeMachinePlaced;
    public event Action onTimeTravelStarted;
    public event Action onTimeTravelOccured;
    public event Action onTimeTravelCompleted;
    public event Action onRemoteControllerActivated;
    public event Action onRemoteControllerDeactivated;
    public event Action onTimeTravelPrepCancelled;
    public event Action onCarCollidesWithWalls;
    public event Action onCarStopped;
    public event Action onTimeDestinationAdjustmentActived;
    public event Action onTimeDestinationAdjustmentDeActived;





    // Methods to trigger events
    public void TimeMachinePlaced() => onTimeMachinePlaced?.Invoke();
    public void TimeTravelStarted() => onTimeTravelStarted?.Invoke();

    public void TimeTravelOccured() => onTimeTravelOccured?.Invoke();

    public void TimeTravelCompleted() => onTimeTravelCompleted?.Invoke();
    public void RemoteControllerActivated() => onRemoteControllerActivated?.Invoke();
    public void RemoteControllerDeactivated() => onRemoteControllerDeactivated?.Invoke();
    public void CancelTimeTravelPrep() => onTimeTravelPrepCancelled?.Invoke();

    public void CarCollidedWithWalls() => onCarCollidesWithWalls?.Invoke();


    public void CarStopped() => onCarStopped?.Invoke();


    public void TimeDestinationAdjustmentActivated() => onTimeDestinationAdjustmentActived?.Invoke();

    public void TimeDestinationAdjustmentDeActivated() => onTimeDestinationAdjustmentDeActived?.Invoke();


}
