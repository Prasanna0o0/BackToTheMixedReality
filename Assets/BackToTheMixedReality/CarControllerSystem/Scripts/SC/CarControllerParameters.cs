using UnityEngine;

[CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObjects/CarControllerParameters", order = 1)]
public class CarControllerParameters : ScriptableObject
{
    [Header("Car Motor Data")]
    public int Force = 70;
    public int Angle = 30;
    public int BrakeForce = 10;
    [Space(10)] // 10 pixels of spacing here.
    public float rollingResistanceCoefficient = 2f; // Adjust as needed
    public float airDragCoefficient = 3f; // Adjust as needed
    public float StopThreshold = 0.1f; // Speed threshold in meters per second
    public float naturalDecelerationCoefficient = 2f;

    [Space(10)] // 10 pixels of spacing here.
    [Header("Drive Data")]
    public float maxTorque = 60f; // Maximum torque (Nm)
    public float maxRPM = 1000; // Maximum RPM
    public float peakTorqueRPM = 500f; // RPM at which max torque is achieved
    public float maxSpeed = 15f; // Convert km/h to m/s
    public float speedLimitInMR = 2f;
    public float inputThreshold = 0.1f;

    [Space(10)] // 10 pixels of spacing here.
    [Header("Reverse Data")]
    public float reverseTorque = 50f; // Adjust as needed
    public float reverseAccelerationRate = 1f; // Acceleration rate for reversing
    public float maxReverseSpeedMph = 4f; // Max speed in km/h for reverse

    // New field for braking
    public float brakeDecelerationRate = 200f; // Deceleration rate when braking
}