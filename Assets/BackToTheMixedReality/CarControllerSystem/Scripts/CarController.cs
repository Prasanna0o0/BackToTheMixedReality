using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CarController : MonoBehaviour
{
    #region Car Control Fields

    public CarControllerParameters carControllerParameters;

    [Header("References")]
    public CarInputProvider inputProvider;
    [Tooltip("Set ref in order of FL, FR, RL, RR")]
    public WheelCollider[] WheelColliders;
    [Tooltip("Set ref of wheel meshes in order of FL, FR, RL, RR")]
    public Transform[] Wheels;
    public Transform CenterOfMass;
    public Rigidbody rb;
    public AnimationCurve torqueCurve;

    float initialDrag;
    float initialAngularDrag;

    public Vector3 positionOffset; // Position offset
    public Vector3 rotationOffset; // Rotation offset




    [Header("Data to watch")]
    public float appliedTorqueValue;
    public float currentSpeed; // Current speed of the car
    public float speedMilesPerHour;


    [Header("Bools")]
    public bool isBraking = false;
    public bool isSpeedLimitExceeded = false;
    public bool isCarControlEnabled = true;

    [Serializable]
    public enum CarMovementState
    {
        Stationary,
        MovingForward,
        MovingBackward
    }

    public CarMovementState currentMovementState = CarMovementState.Stationary;


    #endregion


    #region Unity Methods 
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CenterOfMass.localPosition;
        initialDrag = rb.drag;
        initialAngularDrag = rb.angularDrag;
    }



    private void Update()
    {
        UpdateSpeedData();
    }



    private void FixedUpdate()
    {
        //Car Control stuff
        CheckForStateChange();
        Steer();

        // Check if the brake is being applied
        if (inputProvider.Brake > 0)
        {
            Brake();
        }
        else
        {
            ResetBrake();
        }
        // Check if driving forward or in reverse based on vertical input
        if (inputProvider.Vertical > carControllerParameters.inputThreshold)
        {
            Drive();
            ApplyDeceleration();
        }
        else if (inputProvider.Vertical < 0)
        {
            if (currentMovementState == CarMovementState.MovingForward)
            {
                ApplyBrakeToStop();
            }
            else
            {
                Reverse();
            }
        }
    }


    #endregion

    #region Drive Methods

    private void Drive()
    {
        if (!isCarControlEnabled) return;
        float speed = CalculateSpeed(); // Get current speed
        float rpm = CalculateRPM(speed); // Convert speed to RPM
        float torque = CalculateTorque(rpm); // 
        float appliedTorque = torque * inputProvider.Vertical * carControllerParameters.Force;
        appliedTorqueValue = appliedTorque;
        ApplyTorque(appliedTorque);
    }

    private void Reverse()
    {
        if (!isCarControlEnabled) return;

        // Reset brake torque when reversing
        foreach (var wheelCollider in WheelColliders)
        {
            wheelCollider.brakeTorque = 0;
        }
        // Convert speed to m/s (20 mph = 8.94 m/s)
        float maxReverseSpeedMetersPerSecond = carControllerParameters.maxReverseSpeedMph * 0.44704f;

        // Check the current reverse speed
        float currentReverseSpeed = rb.velocity.magnitude * (Vector3.Dot(rb.velocity, transform.forward) < 0 ? -1 : 1);

        if (currentReverseSpeed > -maxReverseSpeedMetersPerSecond)
        {
            float reverseSpeed = CalculateReverseSpeed(); // Calculate the reverse speed
            float rpm = CalculateRPM(reverseSpeed); // Convert reverse speed to RPM
            float torque = CalculateTorque(rpm); // Negate the torque for reverse

            // Apply reduced torque if the speed limit is approached
            float torqueMultiplier = Mathf.Clamp((maxReverseSpeedMetersPerSecond + currentReverseSpeed) / maxReverseSpeedMetersPerSecond, 0, 1);
            ApplyTorque(torque * inputProvider.Vertical * carControllerParameters.Force * torqueMultiplier);
        }
    }

    private void ApplyBrakeToStop()
    {
        foreach (var wheelCollider in WheelColliders)
        {
            wheelCollider.brakeTorque = carControllerParameters.BrakeForce * 5; // Apply stronger brake force to stop quickly
        }
    }
    private void Steer()
    {
        if (!isCarControlEnabled) return;
        WheelColliders[0].steerAngle = WheelColliders[1].steerAngle = inputProvider.Horizontal * carControllerParameters.Angle;
    }

    private void Brake()
    {
        if (!isCarControlEnabled) return;

        isBraking = true;
        ApplyBrakeTorque(carControllerParameters.BrakeForce);
    }

    private void ResetBrake()
    {
        if (!isCarControlEnabled) return;

        isBraking = false;
        ApplyTorque(0);
        ApplyBrakeTorque(0);
        ResetWheelForces();
    }

    private void ApplyTorque(float torque)
    {
        WheelColliders[2].motorTorque = WheelColliders[3].motorTorque = torque;
    }

    private void ApplyBrakeTorque(float brakeTorque)
    {
        foreach (var wheelCollider in WheelColliders)
        {
            wheelCollider.brakeTorque = brakeTorque;
        }
    }

   
    #region Motor Calculation Methods
    public float CalculateTorque(float rpm)
    {
        float normalizedRPM = rpm / carControllerParameters.maxRPM;
        float torqueMultiplier = torqueCurve.Evaluate(normalizedRPM);
        return carControllerParameters.maxTorque * torqueMultiplier;
    }

    public float CalculateRPM(float speed)
    {
        // Convert speed to RPM - this is a simplification
        float rpm = (speed / carControllerParameters.maxSpeed) * carControllerParameters.maxRPM;
        return Mathf.Clamp(rpm, 0, carControllerParameters.maxRPM);
    }

    public float CalculateSpeed()
    {

        return rb.velocity.magnitude; // Speed in meters per second
    }

    private float CalculateReverseSpeed()
    {
        // Assuming reverse speed to be a fraction of maxSpeed for simplicity
        float maxReverseSpeedMetersPerSecond = carControllerParameters.maxReverseSpeedMph / 3.6f;
        return Mathf.Min(currentSpeed, maxReverseSpeedMetersPerSecond);
    }

    private void ApplyNaturalDeceleration()
    {

        if (rb.velocity.magnitude > carControllerParameters.StopThreshold)
        {
            // Rolling resistance
            Vector3 rollingResistanceForce = -carControllerParameters.rollingResistanceCoefficient * rb.velocity.normalized * rb.mass;

            // Air drag (quadratic drag can be used for more realism)
            Vector3 airDragForce = -carControllerParameters.airDragCoefficient * rb.velocity.sqrMagnitude * rb.velocity.normalized;

            // Total deceleration force
            Vector3 totalDecelerationForce = rollingResistanceForce + airDragForce;

            // Apply the deceleration force to the Rigidbody
            rb.AddForce(totalDecelerationForce, ForceMode.Force);
        }
        else
        {
            rb.velocity = Vector3.zero;
            TimeMachineFactory.Instance.carControlEnabler.DisableCarControl();
            StopAllWheels();
        }

    }

    private void ApplyDeceleration()
    {

        if (rb.velocity.magnitude > carControllerParameters.speedLimitInMR)
        {
            // Rolling resistance
            Vector3 rollingResistanceForce = -carControllerParameters.rollingResistanceCoefficient * carControllerParameters.naturalDecelerationCoefficient * rb.velocity.normalized * rb.mass;

            // Air drag (quadratic drag can be used for more realism)
            Vector3 airDragForce = -carControllerParameters.airDragCoefficient * rb.velocity.sqrMagnitude * carControllerParameters.naturalDecelerationCoefficient * rb.velocity.normalized;

            // Total deceleration force
            Vector3 totalDecelerationForce = rollingResistanceForce + airDragForce;

            // Apply the deceleration force to the Rigidbody
            rb.AddForce(totalDecelerationForce, ForceMode.Force);
        }
    }

    #endregion

    #endregion

    #region Car Control Helper Methods

    private void StopAllWheels()
    {
        foreach (var wheel in WheelColliders)
        {
            wheel.motorTorque = 0;
            wheel.brakeTorque = carControllerParameters.BrakeForce * 2; // Apply brake force to stop the wheel
        }
        ApplyTorque(0);
        ApplyTorque(-carControllerParameters.BrakeForce);
    }

    private void ResetWheelForces()
    {
        foreach (var wheel in WheelColliders)
        {
            wheel.motorTorque = 0;
            wheel.brakeTorque = 0;
        }
    }

    private void CheckForStateChange()
    {
        if (inputProvider.Vertical > 0 && currentMovementState != CarMovementState.MovingForward)
        {
            StopAllWheels();
            currentMovementState = CarMovementState.MovingForward;
        }
        else if (inputProvider.Vertical < 0 && currentMovementState != CarMovementState.MovingBackward && IsMovingBackward())
        {
            StopAllWheels();
            currentMovementState = CarMovementState.MovingBackward;
        }
        else if (rb.velocity.magnitude < carControllerParameters.StopThreshold && currentMovementState != CarMovementState.Stationary)
        {
            StopAllWheels();
            currentMovementState = CarMovementState.Stationary;
            GlobalEventsManager.Instance.CancelTimeTravelPrep();
            GlobalEventsManager.Instance.CarStopped();

        }
    }



    void UpdateSpeedData()
    {
        GetSpeedMilesPerHour();
        if (TimeMachineFactory.Instance.uiManager != null)
        {
            TimeMachineFactory.Instance.uiManager.speedText.text = speedMilesPerHour.ToString("0") + " MPH";
        }

    }

    public float GetSpeedMilesPerHour()
    {
        //Calculate and Update Speed 
        currentSpeed = rb.velocity.magnitude;
        speedMilesPerHour = currentSpeed * 2.23694f; // Convert m/s to mph
        return speedMilesPerHour;
    }

   
    public bool IsMovingBackward()
    {
        // Calculate the dot product of the car's forward vector and its velocity
        float dot = Vector3.Dot(rb.transform.forward, rb.velocity);

        // If the dot product is negative, the car is moving backward
        return dot < 0;
    }

    #endregion

}
