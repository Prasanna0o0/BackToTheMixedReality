using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScriptsFactory : MonoBehaviour
{

    #region References
    public MRObjectPlacer mrObjectPlacerScript;
    public TimeMachineSpawner timeMachineSpawnerScript;
    public RemoteControllerSpawner remoteControllerManager;
    public StopwatchSpawner stopWatchManager;
    public GUIManager guiManager;
    public GameTutorialManager gameTutorialManager;
    public PassthroughBrightnessManager passthroughBrightnessManager;
    #endregion


    #region Singleton Implementation

    public static GameScriptsFactory Instance { get; private set; }

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
