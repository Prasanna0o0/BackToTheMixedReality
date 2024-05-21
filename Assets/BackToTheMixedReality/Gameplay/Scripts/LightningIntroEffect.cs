using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningIntroEffect : MonoBehaviour
{
    private FMODController fmodController;

    public EventReference eventPath_Lightning;
    EventInstance eventInstance_Lightning;

    private void Awake()
    {
        fmodController = gameObject.AddComponent<FMODController>();

    }

    private void Start()
    {
        eventInstance_Lightning = fmodController.CreateAndStartEvent(eventPath_Lightning);
        fmodController.SetParameter(eventInstance_Lightning, "Speed", 60f);
    }

    private void OnDestroy()
    {
        fmodController.StopEvent(eventInstance_Lightning);
    }

}
