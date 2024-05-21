using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class FMODController : MonoBehaviour
{
    // Method to create and start an FMOD event instance
    public EventInstance CreateAndStartEvent(FMODUnity.EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        eventInstance.start();
        return eventInstance;
    }

    // Method to create and start an FMOD event instance
    public EventInstance CreateEvent(FMODUnity.EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        return eventInstance;
    }

    // Method to stop an FMOD event instance
    public void StopEvent(EventInstance eventInstance, bool immediate = false)
    {
        if (immediate)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        else
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        eventInstance.release();
    }

    // Method to set a parameter of an FMOD event instance
    public void SetParameter(EventInstance eventInstance, string parameterName, float value)
    {
        eventInstance.setParameterByName(parameterName, value);
    }

    // Method to play a one-shot sound
    public void PlayOneShotSound(EventReference eventReference)
    {
        RuntimeManager.PlayOneShot(eventReference, transform.position);
    }
}
