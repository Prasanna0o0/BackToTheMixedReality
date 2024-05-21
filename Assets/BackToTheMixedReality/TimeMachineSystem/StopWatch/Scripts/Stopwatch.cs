using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace BacktoMR
{
    public class Stopwatch : MonoBehaviour
    {

        #region Fields
        public GameObject timeScreen;
        public GameObject musicScreen;
        public TextMeshPro music_Text;

        public enum MusicState
        {
            Off,
            Music1,
            Music2,
            Music3,
            Music4
        }
        public MusicState currentMusicState;
        private int musicStateIndex = 0;

        public TextMeshPro tutorialInfo_Default_Text;
        public TextMeshPro tutorialInfo_Adjusting_Text;
        public TextMeshPro countdown_Text;
        public float timeIncrement = 5f;

        private float destinationTime = 20;
        private bool isCountingDown = false;
        GameInputActions inputActions;

        private bool isStopwatchSetupModeActive = false;

        private FMODController fmodController;
        public EventReference eventPath_AdjustTimeDestionationBeep;
        public EventReference eventPath_SetTimeDestionationBeep;
        public EventReference eventPath_StopwatchTicking;

        public EventReference eventPath_BackgroundMusic1;
        public EventReference eventPath_BackgroundMusic2;
        public EventReference eventPath_BackgroundMusic3;
        public EventReference eventPath_BackgroundMusic4;
        private EventInstance eventInstance_BackgroundMusic;



        #endregion


        #region Unity Events

        private void Awake()
        {
            fmodController = gameObject.AddComponent<FMODController>();

            currentMusicState = MusicState.Off;

        }

        private void Start()
        {
            isCountingDown = false;
            ActivateDefaultTutorialText();
            destinationTime = TimeMachineFactory.Instance.timeTravelManager.travelTimeInSeconds;
            countdown_Text.text = ConvertTime(destinationTime);


            //Listen for controller inputs
            inputActions.Stopwatch.ToggleDestinationTimeSetup.performed += _ => SetupStopWatch();
            inputActions.Stopwatch.IncrementTime.performed += _ => IncrementDestinationTime();
            inputActions.Stopwatch.DecrementTime.performed += _ => DecrementDestinationTime();
            inputActions.Stopwatch.ChangeBackgroundMusic.performed += _ => ChangeBackgroundMusic();

            isStopwatchSetupModeActive = false;
            ActivateScreen(timeScreen.name);

        }


        private void OnEnable()
        {
            GlobalEventsManager.Instance.onTimeTravelOccured += StartCountdown;

            inputActions = new GameInputActions();
            inputActions.Enable();
        }


        private void OnDisable()
        {
            GlobalEventsManager.Instance.onTimeTravelOccured -= StartCountdown;
            inputActions.Disable();

        }


        #endregion


        #region ChangeBackgroundMusic

        void ChangeBackgroundMusic()
        {
            if (!TimeMachineFactory.Instance.timeTravelManager.canTimeTravel)
            {
                return;
            }

                if (timeScreen.gameObject.activeSelf)
            {
                ActivateScreen(musicScreen.name);
            }

            
            musicStateIndex += 1;
            if (musicStateIndex>4)
            {
                musicStateIndex = 0;
            }
            

            switch (musicStateIndex)
            {
                case 0:
                    currentMusicState = MusicState.Off;
                    break;
                case 1:
                    currentMusicState = MusicState.Music1;
                    break;
                case 2:
                    currentMusicState = MusicState.Music2;
                    break;
                case 3:
                    currentMusicState = MusicState.Music3;
                    break;
                case 4:
                    currentMusicState = MusicState.Music4;
                    break;

            }
            PlayBackgroundMusic(currentMusicState);
            fmodController.PlayOneShotSound(eventPath_AdjustTimeDestionationBeep);

            DOVirtual.DelayedCall(2.5f, () =>

                ActivateScreen(timeScreen.name)
            );


        }



        void ActivateScreen(string screenName)
        {
            timeScreen.SetActive(timeScreen.gameObject.name.Equals(screenName));
            musicScreen.SetActive(musicScreen.gameObject.name.Equals(screenName));

        }

        void PlayBackgroundMusic(MusicState musicState)
        {
            if (eventInstance_BackgroundMusic.isValid())
            {
                fmodController.StopEvent(eventInstance_BackgroundMusic);
            }

            if (musicState == MusicState.Music1)
            {
                music_Text.text = "MUSIC 1";

                eventInstance_BackgroundMusic= fmodController.CreateAndStartEvent(eventPath_BackgroundMusic1);

            }
            else if (musicState == MusicState.Music2)
            {
                eventInstance_BackgroundMusic = fmodController.CreateAndStartEvent(eventPath_BackgroundMusic2);
                music_Text.text = "MUSIC 2";

            }
            else if (musicState == MusicState.Music3)
            {
                eventInstance_BackgroundMusic = fmodController.CreateAndStartEvent(eventPath_BackgroundMusic3);
                music_Text.text = "MUSIC 3";

            }
            else if (musicState == MusicState.Off)
            {
                fmodController.StopEvent(eventInstance_BackgroundMusic);
                music_Text.text = "OFF";
            }
            else if (musicState == MusicState.Music4)
            {
                eventInstance_BackgroundMusic = fmodController.CreateAndStartEvent(eventPath_BackgroundMusic4);
                music_Text.text = "MUSIC 4";
            }
        }

        #endregion

        #region Stopwatch Countdown Events

        public void StartCountdown()
        {
            StartCoroutine(CountdownCoroutine(destinationTime));
        }

        private IEnumerator CountdownCoroutine(float timeInSeconds)
        {
            while (timeInSeconds > 0)
            {

                countdown_Text.text = ConvertTime(timeInSeconds);
                fmodController.PlayOneShotSound(eventPath_StopwatchTicking);
                yield return new WaitForSeconds(1f);
                timeInSeconds--;
            }

            countdown_Text.text = ConvertTime(0);
            // Additional actions when countdown reaches 0
            yield return new WaitForSeconds(1f);
            countdown_Text.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);

            countdown_Text.text = ConvertTime(destinationTime);
            countdown_Text.gameObject.SetActive(true);
            fmodController.PlayOneShotSound(eventPath_SetTimeDestionationBeep);

            OnCountdownComplete();
        }

        private void OnCountdownComplete()
        {
            // Actions to perform when the countdown completes
            Debug.Log("Countdown Complete!");
        }


        string ConvertTime(float timeInSeconds)
        {
            float minutes = Mathf.FloorToInt(timeInSeconds / 60);
            float seconds = Mathf.FloorToInt(timeInSeconds % 60);

            string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);
            return timeText;
        }


        #endregion



        #region Time Destination Adjust Events

        void ActivateDefaultTutorialText()
        {
            tutorialInfo_Default_Text.gameObject.SetActive(true);
            tutorialInfo_Adjusting_Text.gameObject.SetActive(false);

        }

        void ActivateAdjustingTutorialText()
        {
            tutorialInfo_Default_Text.gameObject.SetActive(false);
            tutorialInfo_Adjusting_Text.gameObject.SetActive(true);

        }

        void SetupStopWatch()
        {

            if (TimeMachineFactory.Instance.timeTravelManager.canTimeTravel)
            {
                if (!isStopwatchSetupModeActive)
                {
                    isStopwatchSetupModeActive = true;
                    ActivateAdjustingTutorialText();
                    GlobalEventsManager.Instance.TimeDestinationAdjustmentActivated();

                }
                else
                {
                    isStopwatchSetupModeActive = false;
                    ActivateDefaultTutorialText();
                    GlobalEventsManager.Instance.TimeDestinationAdjustmentDeActivated();

                }
                //SFX
                fmodController.PlayOneShotSound(eventPath_SetTimeDestionationBeep);
            }

        }

        public void IncrementDestinationTime()
        {

            if (isStopwatchSetupModeActive)
            {
                destinationTime = TimeMachineFactory.Instance.timeTravelManager.travelTimeInSeconds;
                destinationTime += timeIncrement;
                if (destinationTime > 60)
                {
                    //Max wait time is 60 seconds and I think it is a good waiting time.
                    destinationTime = 60;
                }
                countdown_Text.text = ConvertTime(destinationTime);
                TimeMachineFactory.Instance.timeTravelManager.travelTimeInSeconds = destinationTime;

                fmodController.PlayOneShotSound(eventPath_AdjustTimeDestionationBeep);

            }

        }

        public void DecrementDestinationTime()
        {
            if (isStopwatchSetupModeActive)
            {
                destinationTime = TimeMachineFactory.Instance.timeTravelManager.travelTimeInSeconds;
                destinationTime -= timeIncrement;
                if (destinationTime < 10)
                {
                    destinationTime = 10;
                }
                countdown_Text.text = ConvertTime(destinationTime);
                TimeMachineFactory.Instance.timeTravelManager.travelTimeInSeconds = destinationTime;

                fmodController.PlayOneShotSound(eventPath_AdjustTimeDestionationBeep);
            }
        }

        #endregion
    }
}

