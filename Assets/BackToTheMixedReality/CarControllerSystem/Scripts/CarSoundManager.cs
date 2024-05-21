using FMODUnity;
using UnityEngine;

public class CarSoundManager : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter carEngineEmitter_Drive;
    [SerializeField] private StudioEventEmitter tiresSqueals;

    private FMODController fmodController;

    private FMOD.Studio.EventInstance soundInstance_Brake;
    public EventReference carEngineSound_BrakeEventPath;


    bool isCarEngineRunning = true;
    CarInputProvider carInputProvider;

    public GeneralFMODParameters fmodParams;
    public CarControllerParameters carControllerParameters;
    public bool isDriftActive = false;


    #region Unity Events
    private void Start()
    {
        carInputProvider = TimeMachineFactory.Instance.inputProvider;

        // Add the FMODController component
        fmodController = gameObject.AddComponent<FMODController>();

        //Update Drive Sound
        // Update Gas parameter
        float gas = carInputProvider.Vertical > 0 ? 1 : 0;
        carEngineEmitter_Drive.SetParameter("Gas", gas);
    }

   

    void Update()
    {
        if (isCarEngineRunning)
        {
            UpdateEngineSounds();
            UpdateTireSound();
            UpdateBrakeSound();
        }
    }

    #endregion


    #region Car Sounds

    public void StopCarEngineSounds()
    {
        isCarEngineRunning = false;
        carEngineEmitter_Drive.Stop();
        tiresSqueals.Stop();

    }

    public void StartCarEngineSounds()
    {
        isCarEngineRunning = true;
        carEngineEmitter_Drive.Play();
    }

    private void UpdateEngineSounds()
    {
        // Update RPM parameter (this is a placeholder, adjust as needed)
        float rpm = CalculateRPM(); // Implement this based on your car's behavior
        carEngineEmitter_Drive.SetParameter("RPM", rpm);

        // Update Speed parameter
        float speed = GetCarSpeed(); // Implement this to get the car's current speed
        carEngineEmitter_Drive.SetParameter("Speed", speed);

        //Update Reverse sound
        if (TimeMachineFactory.Instance.inputProvider.Vertical < 0)
        {
            carEngineEmitter_Drive.SetParameter("Speed", 40f);
            carEngineEmitter_Drive.SetParameter("RPM", rpm / 2);
        }
    }

    private void UpdateTireSound()
    {

        float speed = TimeMachineFactory.Instance.carControllerScript.speedMilesPerHour; // Implement GetCurrentSpeed() in your CarController
        float steeringAngle = carInputProvider.Horizontal; // Implement GetSteeringAngle() in your CarController

        if (speed < 1 && speed > carControllerParameters.StopThreshold)
        {
            tiresSqueals.SetParameter("Speed", fmodParams.slowSpeedThreshold_TireSqueals);
        }
        else if (speed > 2)
        {
            tiresSqueals.SetParameter("Speed", fmodParams.mediumSpeedThreshold_TireSqueals);
        }
        else if (speed > 3)
        {
            tiresSqueals.SetParameter("Speed", fmodParams.fastSpeedThreshold_TireSqueals);
        }

        // Play tire sound only when turning
        if (Mathf.Abs(steeringAngle) > steeringAngle * 0.5f && speed > carControllerParameters.StopThreshold)
        {
            if (!tiresSqueals.IsPlaying())
            {
                tiresSqueals.Play();
            }
        }
        else
        {
            tiresSqueals.Stop();
        }


    }


    void UpdateBrakeSound()
    {
        //if (carInputProvider.Brake>1)
        //{
        //    if (!tiresSqueals.IsPlaying())
        //    {
        //        tiresSqueals.SetParameter("Speed", fmodParams.slowSpeedThreshold_TireSqueals);
        //        tiresSqueals.Play();
        //    }

        //}
        //else
        //{
        //    tiresSqueals.Stop();
        //}
    }

    private float CalculateRPM()
    {
        // Calculate RPM based on car input or car state
        // Placeholder logic
        return carInputProvider.Vertical * 250; // Example: linear mapping
    }

    private float GetCarSpeed()
    {
        // Implement this method to return the car's current speed
        float speed = TimeMachineFactory.Instance.carControllerScript.speedMilesPerHour; // Implement GetCurrentSpeed() in your CarController

        return Mathf.Abs((speed / 4.0f) * 130);
    }

   
    public void DisableCarEngineSound()
    {
        // Stop the engine sound
        if (carEngineEmitter_Drive.IsPlaying())
        {
            carEngineEmitter_Drive.Stop();

        }
    }

    public void EnableCarEngineSound()
    {
        // Start the engine sound
        if (!carEngineEmitter_Drive.IsPlaying())
        {
            carEngineEmitter_Drive.Play();
        }
    }

    #endregion



}
