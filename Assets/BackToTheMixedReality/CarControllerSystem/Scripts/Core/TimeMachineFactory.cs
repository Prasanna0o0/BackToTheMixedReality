using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMachineFactory : MonoBehaviour
{

    #region Fields

    public CarController carControllerScript;
    public CarInputProvider inputProvider;
    public CarControlEnabler carControlEnabler;
    public UIManager uiManager;
    public TimeTravelManager timeTravelManager;
    public TimeTravelSoundManager timeTravelSoundManager;
    public TimeTravelProximityDetector timeTravelProximityDetector;
    public CarSoundManager carEngineSoundManager;
    public CarControllerParameters carControllerParameters;
    public GeneralFMODParameters fMODParameters;

    #endregion


    #region Unity Methods

    private void Start()
    {
        if (carControllerScript == null)
        {
            carControllerScript = FindObjectOfType<CarController>();
        }

        if (inputProvider == null)
        {
            inputProvider = FindObjectOfType<CarInputProvider>();
        }
    }
    #endregion


    #region Singleton Implementation

    public static TimeMachineFactory Instance { get; private set; }

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
}
