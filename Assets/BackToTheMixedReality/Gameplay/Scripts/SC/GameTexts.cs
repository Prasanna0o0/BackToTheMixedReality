using UnityEngine;

[CreateAssetMenu(fileName = "GameTexts", menuName = "ScriptableObjects/GameTexts", order = 1)]
public class GameTexts : ScriptableObject
{
    [Header("Stopwatch")]
    public string stopwatch_Setup_Text;
    public string stopwatch_SetupDone_Text;




    [Space]
    public string timeMachineControlInstructions;

}
