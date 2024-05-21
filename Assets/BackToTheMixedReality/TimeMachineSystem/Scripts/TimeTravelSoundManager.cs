using Cysharp.Threading.Tasks;
using FMODUnity;
using System;
using UnityEngine;

public class TimeTravelSoundManager : MonoBehaviour
{
    #region References
    private FMODController fmodController;
    private FMOD.Studio.EventInstance soundInstance;
    private FMOD.Studio.EventInstance soundInstance_TimeTravelPrep;
    private FMOD.Studio.EventInstance soundInstance_TimeTravelPrep_Additional;

    private FMOD.Studio.EventInstance soundInstance_Drift;
    public FMOD.Studio.EventInstance soundInstance_AfterTimeTravel;

    public EventReference timeTravelSound_PrepEventPath;
    public EventReference timeTravelSound_AdditionalPrepEventPath;

    public EventReference timeTravelSound_ExplosionEventPath;
    public EventReference timeTravelSound_AfterTimeTravelEventPath;
    public EventReference timeTravelSound_DriftEventPath;

    public float timeTravelSpeed_Prep = 90f;

    #endregion

    #region Unity Events

    private void Awake()
    {
        fmodController = gameObject.AddComponent<FMODController>(); // Add the FMODController component

    }

    private void OnDestroy()
    {
        fmodController.StopEvent(soundInstance_TimeTravelPrep, true);
        // Stop and release the event instance when the GameObject is destroyed
        soundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        soundInstance.release();
    }
    #endregion


    #region Time Travel Sound Methods

    #region Time Travel Prep

    public void ActivateTimeTravelPrepSound()
    {
        if (!soundInstance_TimeTravelPrep.isValid())
        {
            soundInstance_TimeTravelPrep = fmodController.CreateAndStartEvent(timeTravelSound_PrepEventPath);
            fmodController.SetParameter(soundInstance_TimeTravelPrep, "Speed", timeTravelSpeed_Prep);

            soundInstance_TimeTravelPrep_Additional = fmodController.CreateAndStartEvent(timeTravelSound_AdditionalPrepEventPath);
            fmodController.SetParameter(soundInstance_TimeTravelPrep_Additional,"Speed",45f);

        }
        else
        {
            fmodController.SetParameter(soundInstance_TimeTravelPrep, "Speed", timeTravelSpeed_Prep);
            fmodController.SetParameter(soundInstance_TimeTravelPrep_Additional, "Speed", 45f);

        }

    }


    public void DeactivateTimeTravelPrepSound()
    {
        fmodController.StopEvent(soundInstance_TimeTravelPrep, false);
        fmodController.StopEvent(soundInstance_TimeTravelPrep_Additional);
    }

    #endregion


  
    #region After Time Travel

    public async UniTaskVoid PlayAfterTimeTravelSound()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f), false);
        soundInstance_AfterTimeTravel = fmodController.CreateAndStartEvent(timeTravelSound_AfterTimeTravelEventPath);
    }

    public void StopAfterTimeTravelSound()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    }



    public void PlayDriftSound()
    {
        soundInstance_Drift = fmodController.CreateAndStartEvent(timeTravelSound_DriftEventPath);
        fmodController.SetParameter(soundInstance, "Speed", TimeMachineFactory.Instance.fMODParameters.fastSpeedThreshold_TireSqueals);
    }

    public void StopDriftSound(bool isImmediate)
    {
        fmodController.StopEvent(soundInstance_Drift,isImmediate);
    }

    #endregion

    #endregion



}
