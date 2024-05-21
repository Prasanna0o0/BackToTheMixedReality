using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTutorialManager : MonoBehaviour
{
    #region References

    GameInputActions inputActions;

    [SerializeField] GameObject tutorial_spawnObject;

    #endregion


    #region Unity Events
    private void Awake()
    {
        inputActions = new GameInputActions();

        tutorial_spawnObject.SetActive(false);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();

    }

    #endregion

    #region Tutorial Events

    public void ActivateTutorial_SpawnObject()
    {
        tutorial_spawnObject.SetActive(true);

    }

    public void DeActivateTutorial_SpawnObject()
    {
        tutorial_spawnObject.SetActive(false);

    }

    #endregion




}
