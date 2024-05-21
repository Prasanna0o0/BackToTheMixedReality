using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //When the user first opens the application, here are the main steps
    //1. User is forced to placed the Time Machine using a Raycast system from the VR controllers
    //2. As soon as the user spawns the Time Machine, Remote Controller is activated allowing the user to drive the Time Machine
    //3. As soon as the Time Travel happens, a Stopwatch is activated allowing the user to track when the Time Machine will back to the future ( pun intended )
    //4. When the Time Machine completes its final return drift, Remote Controller is activated again


    #region Fiels

    [SerializeField] List<GameObject> managerGameobjects;
    private FMODController fmodController;
    public EventReference eventPath_IntroSong;
    EventInstance eventInstance_IntroSong;


    public EventReference eventPath_Lightning;
    EventInstance eventInstance_Lightning;

    public bool isTesting =false;

    #endregion

    #region Unity Events
    private void Awake()
    {
        fmodController = gameObject.AddComponent<FMODController>();

    }

    private void Start()
    {
        StartGameSession();

    }

    #endregion


    async void StartGameSession()
    {
        SetManagers(false);
        InitiateGameIntroEffect();
        await LoadGameobjectsIntoMemory();
        await UniTask.Delay(4000);
      
    }

    void SetManagers(bool doActivate)
    {
        foreach (var gameObject in managerGameobjects)
        {
            gameObject.SetActive(doActivate);
        }
    }

    async UniTask InitiateGameIntroEffect()
    {
        if (!isTesting)
        {
            GameScriptsFactory.Instance.passthroughBrightnessManager.AdjustBrightnessGradually(0, 20f);
            GameScriptsFactory.Instance.guiManager.ToggleAppSplashScreen(12);

            //Trigger into sound
            eventInstance_IntroSong = fmodController.CreateAndStartEvent(eventPath_IntroSong);
           
            await UniTask.Delay(22000);

            fmodController.StopEvent(eventInstance_IntroSong);

        }

        //Finish the cinematic effect 
        SetManagers(true);
        GameScriptsFactory.Instance.gameTutorialManager.ActivateTutorial_SpawnObject();
    }


    //Load Gameobjects into Memory at the beginning of the game
    async UniTask LoadGameobjectsIntoMemory()
    {
        GameScriptsFactory.Instance.timeMachineSpawnerScript.enabled = true;
        await GameScriptsFactory.Instance.timeMachineSpawnerScript.LoadIntoMemory();

        GameScriptsFactory.Instance.remoteControllerManager.enabled =true;
        await GameScriptsFactory.Instance.remoteControllerManager.LoadIntoMemory();

        GameScriptsFactory.Instance.stopWatchManager.enabled = true;
        await GameScriptsFactory.Instance.stopWatchManager.LoadIntoMemory();


    }


}
