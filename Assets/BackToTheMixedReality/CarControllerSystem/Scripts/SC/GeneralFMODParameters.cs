using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FMODParameters", menuName = "ScriptableObjects/FMODParameters", order = 1)]
public class GeneralFMODParameters : ScriptableObject
{
    public float slowSpeedThreshold_TireSqueals = 5f;
    public float mediumSpeedThreshold_TireSqueals = 45f;
    public float fastSpeedThreshold_TireSqueals = 81f;

}
